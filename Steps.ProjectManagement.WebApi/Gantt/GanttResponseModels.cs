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

    #region Responses

    /// <summary>Converts a list of project activities as a response useful for the Gantt component.</summary>
    static internal ICollection ToGanttResponse(this IList<ProjectItem> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        array.Add(item.ToGanttResponse());
      }
      return array;
    }


    static internal object ToGanttResponse(this ProjectItem projectItem) {
      return new {
        uid = projectItem.UID,
        id = projectItem.Id,
        type = projectItem.ProjectObjectType.Name,
        text = projectItem.Name,
        start_date = CalculateGanttItemStartDate(projectItem).ToString("yyyy-MM-dd HH:mm"),
        duration = CalculateGanttItemDurationInDays(projectItem),
        position = projectItem.Position,
        level = projectItem.Level,
        ragStatus = projectItem.RagStatus,
        parent = projectItem.Parent is Summary ? projectItem.Parent.Id : 0
      };
    }

    #endregion Responses

    #region Auxiliary methods

    private static int CalculateGanttItemDurationInDays(ProjectItem projectItem) {
      if (projectItem.EndDate < ExecutionServer.DateMaxValue) {
        return (int) projectItem.EndDate.Subtract(projectItem.StartDate).TotalDays;

      } else if (!projectItem.EstimatedDuration.IsEmptyInstance) {
        return projectItem.EstimatedDuration.Value;

      } else {
        return 7;
      }
    }


    private static DateTime CalculateGanttItemStartDate(ProjectItem projectItem) {
      if (projectItem.StartDate < ExecutionServer.DateMaxValue) {
        return projectItem.StartDate;

      } else {
        return DateTime.Today.AddDays(-1 * projectItem.EstimatedDuration.Value - 7);
      }
    }

    #endregion Auxiliary methods

  }  // class GanttResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
