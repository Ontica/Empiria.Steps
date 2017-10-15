/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : RequirementsController                           License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Gets and sets procedure requirements data.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.Modeling;

namespace Empiria.Steps.WebApi {

  /// <summary>Gets and sets procedure requirements data.</summary>
  public class RequirementsController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/procedures/{procedureUID}/requirements")]
    public CollectionModel GetProcedureRequirements([FromUri] string procedureUID) {
      try {
        var procedure = Procedure.Parse(procedureUID);

        var requirements = procedure.Requirements;

        return new CollectionModel(this.Request, requirements.ToResponse(),
                                   typeof(Requirement).FullName);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

  }  // class RequirementsController

}  // namespace Empiria.Steps.WebApi
