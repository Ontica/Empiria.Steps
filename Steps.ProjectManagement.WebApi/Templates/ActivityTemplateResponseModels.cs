﻿/* Empiria Steps *********************************************************************************************
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

      string dueOnControllerName = "";

      if (template.DueOnControllerId > 0) {
        dueOnControllerName = Activity.Parse(template.DueOnControllerId).Name;
      }

      return new {
        id = activity.Id,
        uid = activity.UID,
        type = ActivityTemplateController.ACTIVITY_TEMPLATE_TYPE_NAME,
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
        dueOnControllerName,
        dueOnRuleAppliesForAllContracts = template.DueOnRuleAppliesForAllContracts,

        duration = template.Duration,
        durationUnit = template.DurationUnit,

        periodicityRule = ToPeriodicityRule(template.PeriodicRule),

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
        status = activity.Status,

        foreignLanguage = new {
          name = activity.ForeignLanguageData.Name,
          notes = activity.ForeignLanguageData.Notes,
          contractClause = activity.ForeignLanguageData.ContractClause,
          legalBasis = activity.ForeignLanguageData.LegalBasis,
        }
      };
    }

    private static object ToPeriodicityRule(PeriodicRuleData periodicRule) {
      if (periodicRule.IsEmptyInstance) {
        return null;
      }

      return new {
        each = new {
          value = periodicRule.EachValue,
          unit = periodicRule.EachUnit,
        },

        dueOn = !periodicRule.DueOnType.HasValue ? null :
                  new {
                    type = periodicRule.DueOnType,
                    month = periodicRule.Month,
                    dayOfWeek = periodicRule.DayOfWeek,
                    day = periodicRule.Day
                  },

        notes = periodicRule.Notes,
      };
    }

    #endregion Response methods

  }  // class ActivityTemplateResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
