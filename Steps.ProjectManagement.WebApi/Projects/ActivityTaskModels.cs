﻿/* Empiria Steps *********************************************************************************************
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
        id = task.Id,
        uid = task.UID,
        type = task.ProjectObjectType.Name,
        name = task.Name,
        notes = task.Notes,

        project = new {
          uid = task.Project.UID,
          name = task.Project.Name,
        },

        parent = new {
          uid = task.Activity.UID,
          name = task.Activity.Name,
          type = task.Activity.ProjectObjectType.Name,
        },

        activity = new {
          uid = task.Activity.UID,
          name = task.Activity.Name,
          type = task.Activity.ProjectObjectType.Name,
        },

        estimatedDuration = task.EstimatedDuration.ToJson(),
        deadline = task.Deadline,
        plannedEndDate = task.PlannedEndDate,
        actualStartDate = task.ActualStartDate,
        actualEndDate = task.ActualEndDate,

        trafficLight = new {
            type = task.TrafficLightType,
            days = task.TrafficLightType == "CalendarDays" ? (int?) task.TrafficLightDays : null
          },


        reminder = task.ReminderData.ToObject(),

        //reminder = new {
        //  days = task.ReminderDays,
        //  sendAlertsTo = task.SendAlertsTo.ToObject(),
        //  sendAlertsToEmails = task.SendAlertsToEMails,
        //},


        theme = task.Theme,
        resource = task.Resource,

        tags = task.Tag,

        position = task.Position,
        level = task.Level,
        stage = task.Stage,
        status = task.Status,

        responsible = task.Responsible.ToShortResponse(),
        entity = string.Empty,
        assignedDate = task.AssignedDate,
        assignedBy = task.AssignedBy.ToShortResponse(),
        template = new object(),

        foreignLanguage = new {
          name = task.ForeignLanguageData.Name,
          notes = task.ForeignLanguageData.Notes,
        }
      };
    }

    #endregion Responses

  }  // class ActivityTaskModels

}  // namespace Empiria.ProjectManagement.WebApi
