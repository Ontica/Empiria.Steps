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

using Empiria.Contacts;

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
        activityUID = task.Activity.UID,
        name = task.Name,
        notes = task.Notes,
        estimatedDuration = task.EstimatedDuration.ToString(),
        startDate = task.StartDate,
        targetDate = task.TargetDate,
        endDate = task.EndDate,
        dueDate = task.DueDate,
        tags = task.Tags.Items,
        position = task.Position,
        ragStatus = task.RagStatus,
        stage = task.Stage,
        status = task.Status,
        responsible = task.Responsible.ToShortResponse(),
        assignedDate = task.AssignedDate,
        assignedBy = task.AssignedBy.ToShortResponse(),
      };
    }

    #endregion Responses

  }  // class ActivityTaskModels

}  // namespace Empiria.ProjectManagement.WebApi
