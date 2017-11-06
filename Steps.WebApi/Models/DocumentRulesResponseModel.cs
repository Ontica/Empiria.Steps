/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Response methods                    *
*  Type     : DocumentRulesResponseModel                       License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Response static methods for document rules entities.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Steps.Legal;
using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Steps.WebApi {

  /// <summary>Response static methods for document rules entities.</summary>
  static internal class DocumentRulesResponseModel {

    static internal object ToResponse(this IList<DocumentRule> rulesList, Clause clause) {
      return new {
        uid = clause.UID,
        section = clause.Section,
        clauseNo = clause.Number,
        title = clause.Title,
        contractUID = clause.Contract.UID,
        rules = rulesList.ToResponse(),
      };
    }


    static internal ICollection ToResponse(this IList<DocumentRule> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var rule in list) {
        var item = new {
          uid = rule.UID,
          name = rule.Name,
          description = rule.Description,
          workflowObjectUID = WorkflowObject.Parse(rule.WorkflowObjectId).UID,
        };

        array.Add(item);
      }
      return array;
    }

  }  // class DocumentRulesResponseModel

}  // namespace Empiria.Steps.WebApi
