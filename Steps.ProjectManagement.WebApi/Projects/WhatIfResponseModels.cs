﻿/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : WhatIfResponseModels                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for WhatIfResult entities.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.ProjectManagement.Services;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>RResponse static methods for WhatIfResult entities.</summary>
  static internal class WhatIfResponseModels {

    #region Collection responses


    static internal ICollection ToResponse(this IList<ProjectProcess> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var process in list) {
        var itemResponse = process.ToResponse();

        array.Add(itemResponse);

      }
      return array;
    }


    static internal ICollection ToResponse(this IList<ProjectItemStateChange> list) {
      ArrayList array = new ArrayList(list.Count);

      for (int position = 0; position < list.Count; position++) {
        var itemResponse = list[position].ToResponse(position);

        array.Add(itemResponse);

      }
      return array;
    }


    #endregion Collection responses

    #region Entities responses


    static internal object ToResponse(this ProjectProcess process) {
      return new {
        uid = process.UniqueID,
        name = process.Name,
        startActivity = process.StartActivity.ToResponse()
      };
    }


    static internal object ToResponse(this WhatIfResult result) {
      bool createFromTemplateResult = result.SourceOperation == ProjectItemOperation.CreateFromTemplate;

      return new {
        sourceOperation = result.SourceOperation,
        source = createFromTemplateResult ? ((Activity)result.Source).ToActivityTemplateResponse() : result.Source.ToResponse(),
        stateChanges = result.StateChanges.ToResponse(),
        hasErrors = result.HasErrors,
        exception = result.HasErrors ? result.GetException().Message : ""
      };
    }


    static internal object ToResponse(this ProjectItemStateChange stateChange, int position) {
      bool createFromTemplate = stateChange.Operation == ProjectItemOperation.CreateFromTemplate;

      var template = createFromTemplate ? ((Activity) stateChange.Template) : Activity.Empty;

      var activity = !createFromTemplate ? stateChange.ProjectItem : ProjectItem.Empty;

      return new {
        operation = stateChange.Operation,
        uid = stateChange.UID,
        position,
        parentUID = stateChange.ParentStateChange != null ? stateChange.ParentStateChange.UID : String.Empty,
        level = stateChange.ItemLevel,

        name = createFromTemplate ? template.Name : (stateChange.Name ?? activity.Name),
        theme = createFromTemplate ? template.Theme : (stateChange.Theme ?? activity.Theme),
        notes = createFromTemplate ? template.Notes : (stateChange.Notes ?? activity.Notes),

        currentDeadline = !activity.IsEmptyInstance ? activity.Deadline : ExecutionServer.DateMaxValue,

        deadline = stateChange.Deadline,

        plannedEndDate = stateChange.PlannedEndDate,
        actualStartDate = stateChange.ActualStartDate,
        actualEndDate = stateChange.ActualEndDate,

        activityUID = !createFromTemplate ? activity.UID : String.Empty,
        projectUID = createFromTemplate ? stateChange.Project.UID : activity.Project.UID,
        templateUID = !template.IsEmptyInstance ? template.UID : String.Empty,
        matchResult = stateChange.ProcessMatchResult.ToString()
      };
    }

    #endregion Entities responses

  }  // class WhatIfResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
