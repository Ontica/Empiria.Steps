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
using Empiria.Messaging;

namespace Empiria.ProjectManagement.Messaging {

  /// <summary>Coordinates notification and dispatching of messaging operations.</summary>
  public class ProjectManagementMessenger {

    #region Utility methods

    public async System.Threading.Tasks.Task SendEmail(Project project, Person person) {
      Assertion.AssertObject(project, "project");
      Assertion.AssertObject(person, "person");

      var sendTo = new SendTo(person.EMail, person.Alias);

      EMailContent content = EMailContentBuilder.AllPendingActivitiesContent(project, person);

      await EMail.SendAsync(sendTo, content);
    }

    #endregion Utility methods


  }  // class ProjectManagementMessenger

}  // namespace Empiria.ProjectManagement.Messaging
