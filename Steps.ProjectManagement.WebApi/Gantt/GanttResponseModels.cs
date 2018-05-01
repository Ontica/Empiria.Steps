/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : GanttResponseModels                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for project items returned for use in the gantt component.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for project entities.</summary>
  static internal class GanttResponseModels {

    #region Collection responses

    /// <summary>Converts a list of project activities as a response useful for the Gantt component.</summary>
    static internal ICollection ToGanttResponse(this IList<ProjectItem> list) {
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

    #endregion Collection responses

    #region Entities responses

    static internal object ToGanttResponse(this Activity activity) {
      return new {
        id = activity.Id,
        type = activity.ProjectObjectType.Name,
        text = activity.Name,
        start_date = (activity.EndDate < ExecutionServer.DateMaxValue ?
                            activity.StartDate : activity.RequestedTime).ToString("yyyy-MM-dd HH:mm"),
        duration = activity.EndDate < ExecutionServer.DateMaxValue ?
                            (int) activity.EndDate.Subtract(activity.StartDate).TotalDays : activity.EstimatedDuration.Value,
        position = activity.Position,
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
        position = summary.Position,
        ragStatus = summary.RagStatus,
        parent = summary.Parent is Summary ? summary.Parent.Id : 0
      };
    }

    #endregion Entities responses

  }  // class GanttResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
