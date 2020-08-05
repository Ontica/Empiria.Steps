/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                        Component : Web Api                               *
*  Assembly : Empiria.Steps.Design.WebApi.dll              Pattern   : Controller                            *
*  Type     : UtilityController                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API to access general purpose services.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Threading.Tasks;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

using Empiria.Cognition;

namespace Empiria.Steps.Design.WebApi {

  /// <summary>Public API to access general purpose services.</summary>
  public class UtilityController : WebApiController {

    #region Post methods

    [HttpPost]
    [Route("v3/steps/services/translator/to-english")]
    public async Task<SingleObjectModel> TranslateToEnglish([FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var sourceText = bodyAsJson.Get<string>("text");

        string translatedText = await TextTranslator.Translate(sourceText, Language.English);

        return new SingleObjectModel(this.Request, translatedText);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion Post methods

  }  // class UtilityController

}  // namespace Empiria.Steps.Design.WebApi
