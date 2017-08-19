/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Response methods                    *
*  Type     : ProjectResponseMethods                           License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Response static methods for project entities.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Steps.ProjectManagement;
using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Steps.WebApi {

  /// <summary>Response static methods for project entities.</summary>
  static internal class ProjectResponseMethods {

    #region Collections response methods

    static internal ICollection ToResponse(this IList<Project> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var project in list) {
        var item = new {
          uid = project.UID,
          name = project.Name,
          notes = project.Notes,
          ownerUID = project.Owner.UID,
          managerUID = project.Manager.UID,
          status = project.Status
        };
        array.Add(item);
      }
      return array;
    }


    static internal ICollection ToResponse(this IList<ProjectModel> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var model in list) {
        var process = model.BaseProcess;
        var item = new {
          uid = process.UID,
          type = process.WorkflowObjectType.Name,
          name = process.Name,
          notes = process.Notes,
          ownerUID = process.Owner.UID,
          resourceTypeId = process.ResourceType.Id,
          links = process.Links,
          steps = model.Steps.ToResponse()
        };
        array.Add(item);
      }
      return array;
    }


    static internal ICollection ToResponse(this IList<ProcessActivity> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var activity in list) {
        var item = new {
          uid = activity.UID,
          taskType = activity.TaskType,
          involvedParty = activity.InvolvedParty.IsEmptyInstance
                                        ? String.Empty : activity.InvolvedParty.Alias,
          stepNo = activity.InnerTag,
          name = activity.Name,
          notes = activity.Notes,
          ownerUID = activity.Owner.UID,
          resourceTypeId = activity.ResourceType.Id,
          status = activity.Status,
          links = activity.Links
        };
        array.Add(item);
      }
      return array;
    }


    static internal ICollection ToResponse(this IList<Task> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var task in list) {
        var item = task.ToResponse();

        array.Add(item);
      }
      return array;
    }

    /// <summary>Converts a list of project activities as a response useful for the Gantt component.</summary>
    static internal ICollection ToGanttResponse(this IList<Activity> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var activity in list) {
        var item = new {
          id = activity.Id,
          type = activity.ProjectObjectType.Name,
          text = activity.Name,
          start_date = activity.EstimatedStart.ToString("yyyy-MM-dd HH:mm"),
          duration = activity.EstimatedEnd.Subtract(activity.EstimatedStart).Days,
          progress = activity.CompletionProgress,
          parent = activity.Parent is Activity ? activity.Parent.Id : 0
        };
        array.Add(item);
      }
      return array;
    }

    #endregion Collections response methods

    #region Single objects response methods

    static internal object ToResponse(this Project project) {
      return new {
        uid = project.UID,
        name = project.Name,
        notes = project.Notes,
        ownerUID = project.Owner.UID,
        managerUID = project.Manager.UID,
        status = project.Status
      };
    }


    static internal object ToResponse(this Activity activity) {
      return new {
        id = activity.Id,
        uid = activity.UID,
        name = activity.Name,
        notes = activity.Notes,
        estimatedStart = activity.EstimatedStart,
        estimatedEnd = activity.EstimatedEnd,
        estimatedDuration = activity.EstimatedDuration,
        completionProgress = activity.CompletionProgress,
        requestedByUID = activity.RequestedBy.UID,
        requestedTime = activity.RequestedTime,
        responsibleUID = activity.Responsible.UID,
        parentId = activity.Parent.Id,
        projectUID = activity.Project.UID
      };
    }


    static internal object ToResponse(this Task task) {
      return new {
        uid = task.UID,
        name = task.Name,
        notes = task.Notes,
        estimatedStart = task.EstimatedStart,
        estimatedEnd = task.EstimatedEnd,
        estimatedDuration = task.EstimatedDuration,
        completionProgress = task.CompletionProgress,
        assignedToUID = task.AssignedTo.UID,
        assignationTime = task.AssignationTime,
        state = task.Status,
        activityUID = task.Activity.UID
      };
    }

    #endregion Single objects response methods

  }  // class ProjectResponseMethods

}  // namespace Empiria.Steps.WebApi
