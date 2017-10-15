/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Response methods                    *
*  Type     : DocumentResponseModel                            License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Response static methods for documents.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Steps.Legal;

namespace Empiria.Steps.WebApi {

  /// <summary>Response static methods for documents.</summary>
  static internal class DocumentResponseModel {

    static internal ICollection ToResponse(this IList<Document> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var document in list) {
        var item = new {
          uid = document.UID,
          type = document.DocumentType,
          name = document.Name,
          code = document.Code,
          description = document.Description,
          observations = document.Observations,
          url = document.Url,
          sampleUrl = document.SampleURL,
          instructionsUrl = document.InstructionsUrl
        };
        array.Add(item);
      }
      return array;
    }

  }  // class DocumentResponseModel

}  // namespace Empiria.Steps.WebApi
