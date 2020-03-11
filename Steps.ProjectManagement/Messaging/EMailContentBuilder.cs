/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Messaging services                           Component : Presentation Layer                    *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : User interface provider               *
*  Type     : EMailContentBuilder                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Builds e-mail messages content.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Messaging;

namespace Empiria.ProjectManagement.Messaging {

  /// <summary>Builds e-mail messages content.</summary>
  static internal class EMailContentBuilder {

    #region Public methods


    static internal EMailContent AllPendingActivitiesContent(Project project, Person contact) {
      var body = GetTemplate(ContentTemplateType.AllPendingActivitiesSummary);

      body = SetMessageFields(body, project, contact);

      return new EMailContent($"RegTech Project {project.Name} status", body, true);
    }


    #endregion Public methods


    #region Private methods


    static private string GetActivitiesRowTemplate() {
      string templatesPath = ConfigurationData.GetString("Templates.Path");

      string fileName = System.IO.Path.Combine(templatesPath, $"email.template.activity-row.txt");

      return System.IO.File.ReadAllText(fileName);
    }


    static private string GetActivityRow(Activity item) {
      string temp = GetActivitiesRowTemplate();

      temp = temp.Replace("{{ITEM-NAME}}", item.Name);
      temp = temp.Replace("{{ITEM-DATE}}", item.Deadline.ToString("dd/MMM/yyyy"));
      temp = temp.Replace("{{ITEM-ASSIGNEE}}", !item.Responsible.IsEmptyInstance ? item.Responsible.Alias : String.Empty);
      temp = temp.Replace("{{ITEM-PARENT}}", !item.Parent.IsEmptyInstance ? item.Parent.Name : String.Empty);
      temp = temp.Replace("{{ITEM-THEME}}", item.Theme.Length != 0 ? $" | Topic: {item.Theme}" : String.Empty);
      temp = temp.Replace("{{ITEM-TAG}}", item.Tag.Length != 0 ? $" | Tags: {item.Tag}" : String.Empty);
      temp = temp.Replace("{{ITEM-COLOR}}", GetActivityRowColor(item));

      return temp;
    }


    static private string GetActivityRowColor(ProjectItem item) {
      if (item.Deadline <= DateTime.Today.AddDays(7)) {
        return COLORS.red;

      } else if (item.Deadline <= DateTime.Today.AddDays(14)) {
        return COLORS.amber;

      } else if (item.Deadline <= DateTime.Today.AddDays(28)) {
        return COLORS.green;

      } else {
        return COLORS.gray;
      }
    }


    static private string GetProjectItemsTable(Project project) {
      FixedList<ProjectItem> upcoming = project.GetItems()
                                               .FindAll(x => x is Activity && x.IsUpcoming &&
                                                             !((Activity) x).Responsible.EMail.Contains("talanza"));

      upcoming.Sort((x, y) => x.Deadline.CompareTo(y.Deadline));

      string temp = String.Empty;
      foreach (var item in upcoming) {
        temp += GetActivityRow((Activity) item);
      }

      return temp;
    }


    static private string GetTemplate(ContentTemplateType notificationType) {
      string templatesPath = ConfigurationData.GetString("Templates.Path");

      string fileName = System.IO.Path.Combine(templatesPath, $"email.template.{notificationType}.txt");

      return System.IO.File.ReadAllText(fileName);
    }


    static private string SetMessageFields(string body, Project project, Person contact) {
      body = body.Replace("{{PROJECT-NAME}}", project.Name);
      body = body.Replace("{{TO-NAME}}", contact.FirstName);
      body = body.Replace("{{TOTAL-ACTIVITIES}}", project.GetItems().Count.ToString());

      body = body.Replace("{{ACTIVITIES-TABLE-ROW}}", GetProjectItemsTable(project));

      return body;
    }


    #endregion Private methods


    #region Inner class constants

    private class COLORS {

      public static readonly string green = "#009900";
      public static readonly string amber = "#ff9900";
      public static readonly string red = "#cc0000";
      public static readonly string gray = "#9a9a9a";
      public static readonly string ghost_color = "#ececec";
      public static readonly string default_color = "#3dbab3";
      public static readonly string empty = "";

    }

    #endregion Inner COLORS constants

  }  // class EMailContentBuilder

}  // namespace Empiria.ProjectManagement.Messaging
