/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : ActivityTemplateResponseModels               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for ActivityTemplate data.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for ActivityTemplate data.</summary>
  static internal class ActivityTemplateResponseModels {

    #region Response methods

    static internal ICollection ToActivityTemplateResponse(this IList<ProjectItem> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        array.Add(((Activity) item).ToActivityTemplateResponse());
      }
      return array;
    }


    static internal object ToActivityTemplateResponse(this Activity activity) {
      var template = activity.Template;

      return new {
        id = activity.Id,
        uid = activity.UID,
        type = "ObjectType.ProjectItem.ActivityTemplate",
        name = activity.Name,
        theme = activity.Theme,
        notes = activity.Notes,

        activityType = template.ActivityType,
        executionMode = template.ExecutionMode,
        isMandatory = template.IsMandatory,
        isController = template.IsController,

        dueOnTerm = template.DueOnTerm,
        dueOnTermUnit = template.DueOnTermUnit,
        dueOnCondition = template.DueOnCondition,
        dueOnController = template.DueOnControllerId,

        duration = template.Duration,
        durationUnit = template.DurationUnit,
        periodicity = template.Periodicity,

        entity = template.EntityId,
        procedure = template.ProcedureId,
        contractClause = template.ContractClause,
        legalBasis = template.LegalBasis,

        project = new {
          uid = activity.Project.UID,
          name = activity.Project.Name,
        },
        parent = new {
          uid = activity.Parent.UID,
          name = activity.Parent.Name,
          type = activity.Parent.ProjectObjectType.Name,
        },

        position = activity.Position,
        level = activity.Level,
        stage = activity.Stage,
        status = activity.Status
      };
    }

    #endregion Response methods

  }  // class ActivityTemplateResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
