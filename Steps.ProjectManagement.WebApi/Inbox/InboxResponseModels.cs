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
        if (item is Activity) {
          array.Add(((Activity) item).ToInboxResponse());
        } else if (item is Task) {
          array.Add(((Task) item).ToInboxResponse());
        }
      }
      return array;
    }


    static internal object ToInboxResponse(this Activity activity) {
      return new {
        uid = activity.UID,
        externalUID = activity.UID,
        type = activity.ProjectObjectType.UnderlyingSystemType.FullName,
        project = new {
          uid = activity.Project.UID,
          name = activity.Project.Name,
        },
        title = activity.Name,
        from = new {
          uid = activity.AssignedBy.UID,
          name = activity.AssignedBy.Alias
        },
        to = new {
          uid = activity.Responsible.UID,
          name = activity.Responsible.Alias
        },
        description = activity.Notes,
        theme = activity.Theme,
        tags = activity.Tags.Items,
        received = activity.AssignedDate,
        status = activity.Status,
        extensionData = new {
          deadline = activity.Deadline,
          plannedEndDate = activity.PlannedEndDate,
          stage = activity.Stage
        },
      };
    }

    static internal object ToInboxResponse(this Task task) {
      return new {
        uid = task.UID,
        externalUID = task.UID,
        type = task.ProjectObjectType.UnderlyingSystemType.FullName,
        project = new {
          uid = task.Project.UID,
          name = task.Project.Name,
        },
        title = task.Name,
        from = new {
          uid = task.AssignedBy.UID,
          name = task.AssignedBy.Alias
        },
        to = new {
          uid = task.Responsible.UID,
          name = task.Responsible.Alias
        },
        description = task.Notes,
        theme = task.Theme,
        tags = task.Tags.Items,
        received = task.AssignedDate,
        status = task.Status,
        extensionData = new {
          deadline = task.Deadline,
          plannedEndDate = task.PlannedEndDate,
          stage = task.Stage
        },
      };
    }

    #endregion Response methods

  }  // class InboxResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
