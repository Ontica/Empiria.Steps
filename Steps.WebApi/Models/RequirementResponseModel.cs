/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Response methods                    *
*  Type     : RequirementResponseModel                         License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Response static methods for procedure requirements.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Steps.Modeling;

namespace Empiria.Steps.WebApi {

  /// <summary>Response static methods for procedure requirements.</summary>
  static internal class RequirementResponseModel {

    static internal ICollection ToResponse(this IList<Requirement> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var requirement in list) {
        var item = new {
          uid = requirement.UID,
          name = requirement.Name,
          type = requirement.RequirementType,
          appliesTo = requirement.AppliesTo,
          copies = requirement.FillingCopies,
          conditions = requirement.AdditionalConditions,
          notes = requirement.Notes,
          observations = requirement.Observations,
          sourceUrl = requirement.SourceUrl,
          sampleUrl = requirement.SampleUrl,
          instructionsUrl = requirement.InstructionsUrl,
        };
        array.Add(item);
      }
      return array;
    }

  }  // class RequirementResponseModel

}  // namespace Empiria.Steps.WebApi
