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
using System.Collections;
using System.Collections.Generic;

using Empiria.Contacts;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for project's activities.</summary>
  static internal class ActivityResponseModels {

    #region Response methods


    static internal ICollection ToResponse(this IList<ProjectItem> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        var itemResponse = item.ToResponse();

        array.Add(itemResponse);
      }
      return array;
    }


    static internal object ToResponse(this ProjectItem projectItem) {
      if (projectItem is Activity) {
        return ((Activity) projectItem).ToResponse();

      } else if (projectItem is Summary) {
        return ((Summary) projectItem).ToResponse();

      } else if (projectItem is Task) {
        return ((Task) projectItem).ToResponse();

      } else {
        throw Assertion.AssertNoReachThisCode($"Unhandled ToResponse() type for " +
                                              $"{projectItem.GetType().FullName}.");

      }
    }


    static internal object ToResponse(this Activity activity) {
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
        parent = new {
          uid = activity.Parent.UID,
          name = activity.Parent.Name,
          type = activity.Parent.ProjectObjectType.Name,
        },

        estimatedDuration = activity.EstimatedDuration.ToJson(),
        deadline = activity.Deadline,
        plannedEndDate = activity.PlannedEndDate,
        actualStartDate = activity.ActualStartDate,
        actualEndDate = activity.ActualEndDate,

        warnDays = activity.WarnDays,
        warnType = activity.WarnType,

        theme = activity.Theme,
        tags = activity.Tags.Items,

        position = activity.Position,
        level = activity.Level,
        stage = activity.Stage,
        status = activity.Status,

        responsible = activity.Responsible.ToShortResponse(),
        assignedDate = activity.AssignedDate,
        assignedBy = activity.AssignedBy.ToShortResponse(),
        template = activity.HasTemplate ?
                        activity.GetTemplate().ToActivityTemplateResponse() : new object()
      };
    }


    static internal object ToResponse(this Summary summary) {
      return new {
        id = summary.Id,
        uid = summary.UID,
        type = summary.ProjectObjectType.Name,
        name = summary.Name,
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
        deadline = summary.Deadline,
        plannedEndDate = summary.PlannedEndDate,
        actualStartDate = summary.ActualStartDate,
        actualEndDate = summary.ActualEndDate,

        warnDays = summary.WarnDays,
        warnType = summary.WarnType,

        theme = summary.Theme,
        tags = summary.Tags.Items,

        position = summary.Position,
        level = summary.Level,
        stage = summary.Stage,
        status = summary.Status,

        responsible = Contact.Empty.ToShortResponse(),
        assignedDate = "",
        assignedBy = Contact.Empty.ToShortResponse(),
        template = new object()
      };
    }


    #endregion Response methods


  }  // class ActivityResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
