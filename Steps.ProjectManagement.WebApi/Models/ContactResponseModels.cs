/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : ContactResponseModels                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for contact entities.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Contacts;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for contact entities.</summary>
  static internal class ContactResponseModels {

    static internal ICollection ToResponse(this IList<Contact> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var contact in list) {
        var item = new {
          uid = contact.UID,
          name = contact.FullName,
          shortName = contact.Nickname
        };
        array.Add(item);
      }
      return array;
    }

  }  // class ContactResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
