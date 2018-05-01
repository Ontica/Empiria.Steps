/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : InboxResponseModels                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for task inboxes.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for project entities.</summary>
  static internal class InboxResponseModels {

    #region Response methods

    static internal ICollection ToInboxResponse(this IList<ProjectItem> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        if (item is Activity && ((Activity) item).Responsible.Id != -1) {
          array.Add(((Activity) item).ToInboxResponse());
        }
      }
      return array;
    }


    static internal object ToInboxResponse(this Activity activity) {
      return new {
        uid = activity.UID,
        externalUID = activity.UID,
        type = activity.ProjectObjectType.UnderlyingSystemType.FullName,
        title = activity.Name,
        from = new {
          uid = "ksdjfh374",
          name = "Abelardo García"
        },
        to = new {
          uid = activity.Responsible.UID,
          name = activity.Responsible.Nickname
        },
        description = activity.Notes,
        received = DateTime.Parse("2017-10-" + EmpiriaMath.GetRandom(01, 31)),
        status = "Active",
        extensionData = new {
          targetDate = activity.TargetDate,
          dueDate = activity.DueDate,
          ragStatus = activity.RagStatus,
          tags = activity.Tags.Items,
          stage = activity.Stage
        },
      };
    }

    #endregion Response methods

  }  // class InboxResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
