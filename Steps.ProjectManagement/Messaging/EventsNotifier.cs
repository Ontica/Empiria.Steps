/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Messaging services                           Component : Integration Layer                     *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Static class                          *
*  Type     : EventsNotifier                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Methods that serve to integrate with other systems normally using a message queue              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;

using Empiria.Messaging.EMailDelivery;

namespace Empiria.ProjectManagement.Messaging {

  /// <summary>Methods that serve to integrate with other systems normally using a message queue.</summary>
  static public class EventsNotifier {

    #region Public methods

    static internal void ActivityAssigned(Activity activity) {
      if (activity.Deadline == ExecutionServer.DateMaxValue) {
        return;
      }

      var content = EMailContentBuilder.UserAssignedActivityContent(activity.Project, activity,
                                                                    (Person) activity.Responsible);

      SendEmail(content, (Person) activity.Responsible);
    }


    #endregion Public methods

    #region Private methods

    static private void SendEmail(EMailContent content, Person sendToPerson) {
      var sendTo = new SendTo(sendToPerson.EMail, sendToPerson.Alias);

      EMail.Send(sendTo, content);
    }


    #endregion Private methods

  }  // class EventsNotifier

}  // namespace Empiria.ProjectManagement.Messaging
