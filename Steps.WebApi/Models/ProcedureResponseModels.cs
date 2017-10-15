/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Response methods                    *
*  Type     : ProcedureResponseModels                            License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Response static methods for procedures entities.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Steps.Modeling;

namespace Empiria.Steps.WebApi {

  /// <summary>Response static methods for procedures entities.</summary>
  static internal class ProcedureResponseModels {

    static internal ICollection ToResponse(this IList<Procedure> list) {
     ArrayList array = new ArrayList(list.Count);

      foreach (var procedure in list) {
        var item = new {
          uid = procedure.UID,
          name = procedure.Name,
          shortName = procedure.ShortName,
          code = procedure.Code,
          theme = procedure.Theme,
          executionMode = procedure.ExecutionMode,
          projectType = procedure.ProjectType,
          officialUrl = procedure.OfficialURL,
          regulationUrl = procedure.RegulationURL,
          entity = procedure.Authority.Entity.Alias,
          office = procedure.Authority.Office.FullName
          //status = Enum.GetName(typeof(GeneralObjectStatus), procedure.Status),
        };
        array.Add(item);
      }
      return array;
    }


    static internal object ToResponse(this Procedure procedure) {
      return new {
        uid = procedure.UID,
        name = procedure.Name,
        shortName = procedure.ShortName,
        code = procedure.Code,
        theme = procedure.Theme,

        executionMode = procedure.ExecutionMode,
        projectType = procedure.ProjectType,
        officialUrl = procedure.OfficialURL,
        regulationUrl = procedure.RegulationURL,

        entityName = procedure.EntityName,
        authorityName = procedure.AuthorityName,
        authorityTitle = procedure.AuthorityTitle,
        authorityContact = procedure.AuthorityContact,

        authority = procedure.Authority.ToResponse(),
        legalInfo = procedure.LegalInfo,
        filingCondition = procedure.FilingCondition,
        filingFee = procedure.FilingFee,
        requirements = procedure.Requirements.ToResponse(),
        notes = procedure.Notes
      };
    }

    static internal object ToShortResponse(this Procedure procedure) {
      return new {
        uid = procedure.UID,
        shortName = procedure.ShortName,
        name = procedure.Name,
        code = procedure.Code,
        theme = procedure.Theme,
        executionMode = procedure.ExecutionMode,
        projectType = procedure.ProjectType,
        officialUrl = procedure.OfficialURL,
        regulationUrl = procedure.RegulationURL,
        entity = procedure.Authority.Entity.Nickname
      };
    }

  }  // class ProcedureResponseModels

}  // namespace Empiria.Steps.WebApi
