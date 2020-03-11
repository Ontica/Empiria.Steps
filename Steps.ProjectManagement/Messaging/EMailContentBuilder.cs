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


    static internal EMailContent AllPendingActivitiesContent(Project project,
                                                             FixedList<Activity> activities,
                                                             Person contact) {

      var body = GetMainTemplate(ContentTemplateType.AllPendingActivitiesSummary);

      body = SetMessageFields(body, project, activities, contact);

      return new EMailContent($"RegTech Project {project.Name} status", body, true);
    }


    static internal EMailContent UserPendingActivitiesContent(Project project,
                                                              FixedList<Activity> activities,
                                                              Person user) {
      var body = GetMainTemplate(ContentTemplateType.YourPendingActivitiesSummary);

      body = SetMessageFields(body, project, activities, user);

      return new EMailContent($"Your RegTech Project {project.Name} status", body, true);
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
      temp = temp.Replace("{{ITEM-COLOR}}", MessagingUtilities.GetActivityRowColor(item));

      return temp;
    }


    static private string GetMainTemplate(ContentTemplateType notificationType) {
      string templatesPath = ConfigurationData.GetString("Templates.Path");

      string fileName = System.IO.Path.Combine(templatesPath, $"email.template.{notificationType}.txt");

      return System.IO.File.ReadAllText(fileName);
    }


    static private string GetProjectItemsTable(FixedList<Activity> activities) {
      string temp = String.Empty;

      foreach (var item in activities) {
        temp += GetActivityRow(item);
      }

      return temp;
    }


    static private string SetMessageFields(string body, Project project,
                                           FixedList<Activity> activities, Person contact) {
      body = body.Replace("{{PROJECT-NAME}}", project.Name);
      body = body.Replace("{{TO-NAME}}", contact.FirstName);
      body = body.Replace("{{TOTAL-ACTIVITIES}}", project.GetItems().Count.ToString());

      body = body.Replace("{{ACTIVITIES-TABLE-ROW}}", GetProjectItemsTable(activities));

      return body;
    }


    #endregion Private methods

  }  // class EMailContentBuilder

}  // namespace Empiria.ProjectManagement.Messaging
