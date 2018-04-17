/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : ProjectResponseModels                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for project entities.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for project entities.</summary>
  static internal class ProjectResponseModels {

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
          //                         resourceTypeId = process.ResourceType.Id,
          links = process.Links,
          steps = new string[0]     //model.Steps.ToResponse()
        };
        array.Add(item);
      }
      return array;
    }

    static internal ICollection ToResponse(this IList<ProjectObject> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        if (item is Activity) {
          array.Add(((Activity) item).ToResponse());
        } else if (item is Summary) {
          array.Add(((Summary) item).ToResponse());
        }
      }
      return array;
    }


    static internal ICollection ToInboxResponse(this IList<ProjectObject> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        if (item is Activity && ((Activity) item).Responsible.Id != -1) {
          array.Add(((Activity) item).ToInboxResponse());
        } else if (item is Summary) {
          array.Add(((Summary) item).ToResponse());
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
        received = DateTime.Parse("2017-10-" + EmpiriaMath.GetRandom(01, 31)).ToResponse(),
        status = "Active",
        extensionData = new {
          targetDate = activity.TargetDate.ToResponse(),
          dueDate = activity.DueDate.ToResponse(),
          ragStatus = activity.RagStatus,
          tags = activity.Tags.Items,
          stage = activity.Stage
        },
      };
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
    static internal ICollection ToGanttResponse(this IList<ProjectObject> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        if (item is Activity) {
          array.Add(((Activity) item).ToGanttResponse());
        } else if (item is Summary) {
          array.Add(((Summary) item).ToGanttResponse());
        }
      }
      return array;
    }

    #endregion Collections response methods

    #region Single objects response methods

    static internal object ToGanttResponse(this Activity activity) {
      return new {
        id = activity.Id,
        type = activity.ProjectObjectType.Name,
        text = activity.Name,
        start_date = (activity.EndDate < ExecutionServer.DateMaxValue ?
                            activity.StartDate : activity.RequestedTime).ToString("yyyy-MM-dd HH:mm"),
        duration = activity.EndDate <  ExecutionServer.DateMaxValue ?
                            (int) activity.EndDate.Subtract(activity.StartDate).TotalDays : activity.EstimatedDuration.Value,
        ragStatus = activity.RagStatus,
        parent = activity.Parent is Summary ? activity.Parent.Id : 0
      };
    }


    static internal object ToGanttResponse(this Summary summary) {
      return new {
        id = summary.Id,
        type = summary.ProjectObjectType.Name,
        text = summary.Name,
        level = summary.Level,
        start_date = (summary.StartDate < ExecutionServer.DateMaxValue ?
                                  summary.StartDate : summary.RequestedTime).ToString("yyyy-MM-dd HH:mm"),
        duration = summary.EndDate < ExecutionServer.DateMaxValue ?
                           (int) summary.EndDate.Subtract(summary.StartDate).TotalDays : summary.EstimatedDuration.Value,
        ragStatus = summary.RagStatus,
        parent = summary.Parent is Summary ? summary.Parent.Id : 0
      };
    }

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
      //Procedure procedure = null;

      //if (activity.WorkflowObject.ProcedureId != -1) {
      //  procedure = Procedure.Parse(activity.WorkflowObject.ProcedureId);
      //} else {
      //  procedure = Procedure.Empty;
      //}

      return new {
        id = activity.Id,
        uid = activity.UID,
        type = activity.ProjectObjectType.Name,
        name = activity.Name,
        notes = activity.Notes,
        project = new {
          uid = activity.Project.UID,
          name = activity.Project.Name,
        },
        resource = new {
          uid = activity.Resource.UID,
          name = activity.Resource.Name,
        },
        responsible = new {
          uid = activity.Responsible.UID,
          name = activity.Responsible.Nickname,
        },
        requestedBy = new {
          uid = activity.RequestedBy.UID,
          name = activity.RequestedBy.Nickname,
        },
        parent = new {
          id = activity.Parent.Id,
          uid = activity.Parent.UID,
          type = activity.Parent.ProjectObjectType.Name,
          name = activity.Parent.Name,
        },
        procedure = new {
          uid = "Undefined"
        },
        //procedure = new {
        //  uid = procedure.UID,
        //  name = procedure.Name,
        //  code = procedure.Code,
        //  entity = procedure.EntityName
        //},
        estimatedDuration = activity.EstimatedDuration.ToString(),
        startDate = activity.StartDate.ToResponse(),
        targetDate = activity.TargetDate.ToResponse(),
        endDate = activity.EndDate.ToResponse(),
        dueDate = activity.DueDate.ToResponse(),
        ragStatus = activity.RagStatus,
        tags = activity.Tags.Items,
        stage = activity.Stage
      };
    }


    static internal object ToResponse(this Summary summary) {
      return new {
        id = summary.Id,
        uid = summary.UID,
        type = summary.ProjectObjectType.Name,
        name = summary.Name,
        level = summary.Level,
        notes = summary.Notes,
        project = new {
          uid = summary.Project.UID,
          name = summary.Project.Name,
        },
        resource = new {
          uid = summary.Resource.UID,
          name = summary.Resource.Name,
        },
        parent = new {
          id = summary.Parent.Id,
          uid = summary.Parent.UID,
          type = summary.Parent.ProjectObjectType.Name,
          name = summary.Parent.Name,
        },
        estimatedDuration = summary.EstimatedDuration.ToString(),
        startDate = summary.StartDate.ToResponse(),
        targetDate = summary.TargetDate.ToResponse(),
        endDate = summary.EndDate.ToResponse(),
        dueDate = summary.DueDate.ToResponse(),
        ragStatus = summary.RagStatus,
        stage = summary.Stage
      };
    }

    static internal string ToResponse(this DateTime date) {
      if (date == ExecutionServer.DateMaxValue) {
        return String.Empty;
      } else if (date == ExecutionServer.DateMinValue) {
        return String.Empty;
      } else {
        return date.ToString("yyyy-MM-ddTHH:mm:ss");
      }
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
        ragStatus = task.RagStatus,
        tags = task.Tags.Items,
        assignedToUID = task.AssignedTo.UID,
        assignationTime = task.AssignationTime,
        state = task.Status,
        activityUID = task.Activity.UID
      };
    }

    #endregion Single objects response methods

  }  // class ProjectResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
