﻿/* Empiria Steps *********************************************************************************************
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

      var body = GetTemplate(ContentTemplateType.AllPendingActivitiesSummary);
      var withAssigneeTemplate = GetTemplate(ContentTemplateType.ActivityTableRowWithAssignee);

      body = ParseGeneralFields(body, project, contact);

      body = ParseActivitiesTable(body, activities, withAssigneeTemplate);

      return new EMailContent($"RegTech Project {project.Name} status", body, true);
    }


    static internal EMailContent UserPendingActivitiesContent(Project project,
                                                              FixedList<Activity> activities,
                                                              Person user) {
      var body = GetTemplate(ContentTemplateType.YourPendingActivitiesSummary);
      var noAssigneeRowTemplate = GetTemplate(ContentTemplateType.ActivityTableRowWithoutAssignee);

      body = ParseGeneralFields(body, project, user);
      body = ParseActivitiesTable(body, activities, noAssigneeRowTemplate);

      return new EMailContent($"Your RegTech Project {project.Name} status", body, true);
    }


    #endregion Public methods


    #region Private methods


    static private string GetTemplate(ContentTemplateType contentTemplate) {
      string templatesPath = ConfigurationData.GetString("Templates.Path");

      string fileName = System.IO.Path.Combine(templatesPath, $"email.template.{contentTemplate}.txt");

      return System.IO.File.ReadAllText(fileName);
    }


    static private string ParseActivitiesTable(string body,
                                               FixedList<Activity> activities,
                                               string tableRowTemplate) {
      string tableRows = String.Empty;

      foreach (var item in activities) {
        tableRows += ParseActivityTemplate(tableRowTemplate, item);
      }

      return body.Replace("{{ACTIVITIES-TABLE-ROWS}}", tableRows);
    }


    static private string ParseActivityTemplate(string template, Activity item) {
      string temp = template;

      temp = temp.Replace("{{ITEM-NAME}}", item.Name);
      temp = temp.Replace("{{ITEM-DATE}}", item.Deadline.ToString("dd/MMM/yyyy"));
      temp = temp.Replace("{{ITEM-ASSIGNEE}}", !item.Responsible.IsEmptyInstance ? item.Responsible.Alias : String.Empty);
      temp = temp.Replace("{{ITEM-PARENT}}", !item.Parent.IsEmptyInstance ? item.Parent.Name : String.Empty);
      temp = temp.Replace("{{ITEM-THEME}}", item.Theme.Length != 0 ? $" | Topic: {item.Theme}" : String.Empty);
      temp = temp.Replace("{{ITEM-TAG}}", item.Tag.Length != 0 ? $" | Tags: {item.Tag}" : String.Empty);
      temp = temp.Replace("{{ITEM-COLOR}}", MessagingUtilities.GetActivityRowColor(item));

      return temp;
    }


    static private string ParseGeneralFields(string body, Project project, Person contact) {
      body = body.Replace("{{PROJECT-NAME}}", project.Name);
      body = body.Replace("{{TO-NAME}}", contact.FirstName);

      return body;
    }


    #endregion Private methods

  }  // class EMailContentBuilder

}  // namespace Empiria.ProjectManagement.Messaging