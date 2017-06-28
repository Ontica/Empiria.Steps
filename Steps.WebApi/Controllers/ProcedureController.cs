/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : ProcedureController                              License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Gets and sets procedure data.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.Modeling;

namespace Empiria.Steps.WebApi {

  /// <summary>Gets and sets procedure data.</summary>
  public class ProcedureController : WebApiController {

    #region Public APIs

    [HttpGet, AllowAnonymous]
    [Route("v1/procedures")]
    public CollectionModel GetProceduresList([FromUri] string filter = "") {
      try {
        var list = Procedure.GetList(filter ?? String.Empty);

        return new CollectionModel(this.Request, list);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet, AllowAnonymous]
    [Route("v1/procedures/{procedure_UID}")]
    public SingleObjectModel GetProcedure([FromUri] string procedure_UID) {
      try {
        base.RequireResource(procedure_UID, "procedure_UID");

        var procedure = Procedure.Parse(procedure_UID);

        return new SingleObjectModel(this.Request, procedure);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPost, AllowAnonymous]
    [Route("v1/procedures")]
    public SingleObjectModel CreateProcedure([FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var procedure = new Procedure(bodyAsJson);

        procedure.Save();

        return new SingleObjectModel(this.Request, procedure);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPut, HttpPatch, AllowAnonymous]
    [Route("v1/procedures/{procedure_UID}")]
    public SingleObjectModel UpdateProcedure([FromUri] string procedure_UID,
                                             [FromBody] object body) {
      try {
        base.RequireResource(procedure_UID, "procedure_UID");
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var procedure = Procedure.Parse(procedure_UID);

        procedure.Update(bodyAsJson);

        procedure.Save();

        return new SingleObjectModel(this.Request, procedure);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class ProcedureController

}  // namespace Empiria.Steps.WebApi
