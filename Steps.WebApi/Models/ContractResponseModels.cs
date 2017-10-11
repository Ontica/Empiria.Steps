/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Response methods                    *
*  Type     : ContractResponseModels                           License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Response static methods for contract entities.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Steps.Legal;

namespace Empiria.Steps.WebApi {

  /// <summary>Response static methods for contract entities.</summary>
  static internal class ContractResponseModels {

    static internal ICollection ToResponse(this IList<Contract> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var legalDocument in list) {
        var item = new {
          uid = legalDocument.UID,
          name = legalDocument.Name,
          url = legalDocument.Url
        };
        array.Add(item);
      }
      return array;
    }

    static internal object ToResponse(this Contract contract,
                                      IList<Clause> clausesList) {
      return new {
        uid = contract.UID,
        name = contract.Name,
        url = contract.Url,
        clauses = clausesList.ToResponse(),
      };
    }


    static internal ICollection ToResponse(this IList<Clause> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var documentItem in list) {
        var item = documentItem.ToResponse();

        array.Add(item);
      }
      return array;
    }


    static internal object ToShortResponse(this Clause documentItem) {
      return new {
        uid = documentItem.UID,
        contractUID = documentItem.Contract.UID,
        section = documentItem.Section,
        clauseNo = documentItem.Number,
        title = documentItem.Title,
        text = documentItem.Text,
        sourcePageNo = documentItem.DocumentPageNo
      };
    }

    static internal object ToResponse(this Clause clause) {
      return new {
        uid = clause.UID,
        contractUID = clause.Contract.UID,
        section = clause.Section,
        clauseNo = clause.Number,
        title = clause.Title,
        text = clause.Text,
        sourcePageNo = clause.DocumentPageNo,
        notes = clause.Notes,
        status = clause.Status,
        relatedProcedures = clause.RelatedProcedures.ToResponse(),
        contract = new {
          uid = clause.Contract.UID,
          name = clause.Contract.Name,
          url = clause.Contract.Url
        }
      };
    }


    static internal ICollection ToResponse(this IList<RelatedProcedure> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var relatedProcedure in list) {
        var item = relatedProcedure.ToResponse();

        array.Add(item);
      }
      return array;
    }

    static internal object ToResponse(this RelatedProcedure relatedProcedure) {
      return new {
        uid = relatedProcedure.UID,
        procedure = relatedProcedure.Procedure.ToShortResponse(),
        //maxFilingTerm = relatedProcedure.MaxFilingTerm,
        //maxFilingTermType = relatedProcedure.MaxFilingTermType,
        //startsWhen = relatedProcedure.StartsWhen,
        //startsWhenTrigger = relatedProcedure.StartsWhenTrigger,
        notes = relatedProcedure.Notes
      };
    }

  }  // class ContractResponseModels

}  // namespace Empiria.Steps.WebApi
