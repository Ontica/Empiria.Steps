/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : ActivityTaskModels                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for project activity tasks (check or to do lists).                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for project activity tasks (check or to do lists).</summary>
  static internal class ActivityTaskModels {

    #region Responses

    static internal ICollection ToResponse(this IList<Task> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var task in list) {
        var item = task.ToResponse();

        array.Add(item);
      }
      return array;
    }


    static internal object ToResponse(this Task task) {
      return new {
        uid = task.UID,
        name = task.Name,
        notes = task.Notes,
        startDate = task.StartDate,
        targetDate = task.TargetDate,
        endDate = task.EndDate,
        dueDate = task.DueDate,
        estimatedDuration = task.EstimatedDuration.ToString(),
        position = task.Position,
        ragStatus = task.RagStatus,
        tags = task.Tags.Items,
        assignedToUID = task.AssignedTo.UID,
        assignationTime = task.AssignationTime,
        state = task.Status,
        activityUID = task.Activity.UID
      };
    }

    #endregion Responses

  }  // class ActivityTaskModels

}  // namespace Empiria.ProjectManagement.WebApi
