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
using System.Collections.Generic;
using Empiria.Contacts;
using Empiria.Json;

using Empiria.Messaging.EMailDelivery;

namespace Empiria.ProjectManagement.Messaging {

  /// <summary>Methods that serve to integrate with other systems normally using a message queue.</summary>
  static public class EventsNotifier {

    static readonly bool SEND_FAKE_EMAIL = false;

    #region Public methods

    static public void ActivityAssigned(Activity activity) {
      if (activity.Deadline == ExecutionServer.DateMaxValue) {
        return;
      }

      var content = EMailContentBuilder.UserAssignedActivityContent(activity.Project, activity,
                                                                    (Person) activity.Responsible);

      SendEmail(content, (Person) activity.Responsible);
    }


    static public void RemindActivity(Activity activity, JsonObject sendTo) {
      EmpiriaLog.Critical(sendTo.ToString());

      if (activity.Deadline == ExecutionServer.DateMaxValue) {
        return;
      }

      var content = EMailContentBuilder.RemindActivityContent(activity.Project, activity);

      FixedList<Person> contacts = GetContacts(sendTo, "sendAlertsTo");
      SendEmail(content, contacts);


      string[] additional = GetAddresses(sendTo, "sendAlertsToEMails");
      SendEmail(content, additional);
    }


    static public void SendNotification(Activity activity, JsonObject sendTo,
                                        string title, string text) {
      var content = EMailContentBuilder.SendNotificationContent(activity.Project, activity, title, text);

      FixedList<Person> contacts = GetContacts(sendTo, "registered");
      SendEmail(content, contacts);

      string[] additional = GetAddresses(sendTo, "additional");
      SendEmail(content, additional);

    }


    #endregion Public methods

    #region Private methods


    static private void SendEmail(EMailContent content, string address) {
      var sendTo = new SendTo(address);

      if (SEND_FAKE_EMAIL) {
        sendTo = new SendTo("jmcota@ontica.org");
      }
      EMail.Send(sendTo, content);
    }


    static private void SendEmail(EMailContent content, Person sendToPerson) {
      var sendTo = new SendTo(sendToPerson.EMail, sendToPerson.Alias);

      if (SEND_FAKE_EMAIL) {
        sendTo = new SendTo("jmcota@ontica.org", sendToPerson.Alias);
      }
      EMail.Send(sendTo, content);
    }


    static private void SendEmail(EMailContent content, string[] recipents) {
      foreach (var address in recipents) {
        if (EMail.IsValidAddress(address)) {
          SendEmail(content, address);
        }
      }
    }


    static private void SendEmail(EMailContent content, FixedList<Person> contacts) {
      foreach (var contact in contacts) {
        SendEmail(content, contact);
      }
    }


    static private FixedList<Person> GetContacts(JsonObject sendTo, string fieldName) {
      var list = new List<Person>();

      if (!sendTo.Contains(fieldName)) {
        return list.ToFixedList();
      }

      var sendToList = sendTo.GetList<SendDTO>(fieldName);

      foreach (var item in sendToList) {
        list.Add(item.Person);
      }

      return list.ToFixedList();
    }


    private static string[] GetAddresses(JsonObject sendTo, string fieldName) {
      if (sendTo.IsEmptyInstance) {
        return new string[0];
      }

      if (!sendTo.HasValue(fieldName)) {
        return new string[0];
      }

      var value = sendTo.Get<string>(fieldName);

      value = value.Replace(",", " ");
      value = value.Replace(";", " ");
      value = EmpiriaString.TrimAll(value);

      return value.Split(' ');
    }

    #endregion Private methods

  }  // class EventsNotifier

}  // namespace Empiria.ProjectManagement.Messaging
