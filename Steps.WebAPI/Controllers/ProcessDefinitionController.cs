/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : ProcessDefinitionController                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Provides services that gets process definition models.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

namespace Empiria.Steps.WebApi {

  /// <summary>Provides services that gets or sets process definition models.</summary>
  public class ProcessDefinitionController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/process-definitions")]
    public CollectionModel GetProcessDefinitionList() {
      try {
        throw new NotImplementedException();

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/process-definitions/{processDef_ID}")]
    public SingleObjectModel GetProcessDefinition(string processDef_ID) {
      try {
        throw new NotImplementedException();

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class ProcessDefinitionController

}  // namespace Empiria.Steps.WebApi
