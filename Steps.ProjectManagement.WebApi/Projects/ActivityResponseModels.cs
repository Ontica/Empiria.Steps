/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : ActivityResponseModels                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for project's activities.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for project's activities.</summary>
  static internal class ActivityResponseModels {

    #region Response methods

    static internal object ToResponse(this Activity activity) {
      return new {
        uid = activity.UID,
        type = activity.ProjectObjectType.Name,
        name = activity.Name,
        notes = activity.Notes,
        project = new {
          uid = activity.Project.UID,
          name = activity.Project.Name,
        },
        responsible = new {
          uid = activity.Responsible.UID,
          name = activity.Responsible.Nickname,
        },
        parent = new {
          uid = activity.Parent.UID,
          name = activity.Parent.Name,
          type = activity.Parent.ProjectObjectType.Name,
        },
        estimatedDuration = activity.EstimatedDuration.ToString(),
        startDate = activity.StartDate,
        targetDate = activity.TargetDate,
        endDate = activity.EndDate,
        dueDate = activity.DueDate,
        tags = activity.Tags.Items,
        position = activity.Position,
        ragStatus = activity.RagStatus,
        stage = activity.Stage,
        status = activity.Status
      };
    }


    static internal object ToResponse(this Summary summary) {
      return new {
        uid = summary.UID,
        type = summary.ProjectObjectType.Name,
        name = summary.Name,
        level = summary.Level,
        notes = summary.Notes,
        project = new {
          uid = summary.Project.UID,
          name = summary.Project.Name,
        },
        parent = new {
          uid = summary.Parent.UID,
          name = summary.Parent.Name,
          type = summary.Parent.ProjectObjectType.Name,
        },
        estimatedDuration = summary.EstimatedDuration.ToString(),
        startDate = summary.StartDate,
        targetDate = summary.TargetDate,
        endDate = summary.EndDate,
        dueDate = summary.DueDate,
        position = summary.Position,
        ragStatus = summary.RagStatus,
        stage = summary.Stage,
        status = summary.Status
      };
    }

    #endregion Response methods

  }  // class ActivityResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
