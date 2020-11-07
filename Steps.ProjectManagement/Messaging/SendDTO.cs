/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Messaging services                           Component : Integration Layer                     *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Static class                          *
*  Type     : EventsNotifier                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Methods that serve to integrate with other systems normally using a message queue              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Contacts;
using Empiria.Json;

namespace Empiria.ProjectManagement.Messaging {

  public class SendDTO {

    public Person Person {
      get;
      private set;
    }

    static public SendDTO Parse(JsonObject json) {
      var item = new SendDTO();

      item.Person = (Person) Person.Parse(json.Get<string>("uid"));

      return item;
    }

  }

}
