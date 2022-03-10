/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Messaging services                           Component : Presentation Layer                    *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Enumeration type                      *
*  Type     : ProjectManagementMessenger                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Coordinates notification and dispatching of messaging operations.                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Contacts;

using Empiria.Messaging.EMailDelivery;

namespace Empiria.ProjectManagement.Messaging {

  /// <summary>Coordinates notification and dispatching of messaging operations.</summary>
  public class ProjectManagementMessenger {

    #region Utility methods


    public async System.Threading.Tasks.Task SendAllActivitiesEmail(Project project,
                                                                    Person sendTo) {
      Assertion.AssertObject(project, "project");
      Assertion.AssertObject(sendTo, "sendTo");

      FixedList<Activity> activities = MessagingUtilities.GetAllUpcomingActivities(project);

      if (activities.Count == 0) {
        return;
      }

      EMailContent content = EMailContentBuilder.AllPendingActivitiesContent(project, activities, sendTo);

      await SendEmail(content, sendTo);
    }


    public async System.Threading.Tasks.Task SendByThemeSummaryEmail(Project project,
                                                                     Person sendTo) {
      Assertion.AssertObject(project, "project");
      Assertion.AssertObject(sendTo, "sendTo");

      FixedList<Activity> activities = MessagingUtilities.GetAllUpcomingActivities(project);

      if (activities.Count == 0) {
        return;
      }

      EMailContent content = EMailContentBuilder.SendByThemeSummaryEmail(project, activities, sendTo);

      await SendEmail(content, sendTo);
    }


    public async System.Threading.Tasks.Task SendPersonalActivitiesEmail(Project project,
                                                                         Person sendTo) {
      Assertion.AssertObject(project, "project");
      Assertion.AssertObject(sendTo, "sendTo");

      FixedList<Activity> activities = MessagingUtilities.GetUserUpcomingActivities(project, sendTo);

      if (activities.Count == 0) {
        return;
      }

      EMailContent content = EMailContentBuilder.UserAssignedActivityContent(project, activities[0], sendTo);

      await SendEmail(content, sendTo);
    }


    #endregion Utility methods


    #region Private methods


    private async System.Threading.Tasks.Task SendEmail(EMailContent content,
                                                        Person sendToPerson) {
      var sendTo = new SendTo(sendToPerson.EMail, sendToPerson.Alias);

      await EMail.SendAsync(sendTo, content);
    }


    #endregion Private methods


  }  // class ProjectManagementMessenger

}  // namespace Empiria.ProjectManagement.Messaging
