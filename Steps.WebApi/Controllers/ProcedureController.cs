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

    [HttpGet]
    [Route("v1/procedures")]
    public CollectionModel GetProceduresList([FromUri] string filter = "",
                                             [FromUri] string keywords = "") {
      try {
        filter = filter ?? String.Empty;
        keywords = keywords ?? String.Empty;

        var list = Procedure.GetList(filter, keywords);

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Procedure).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/procedures/{procedureUID}")]
    public SingleObjectModel GetProcedure([FromUri] string procedureUID) {
      try {
        base.RequireResource(procedureUID, "procedureUID");

        var procedure = Procedure.Parse(procedureUID);

        return new SingleObjectModel(this.Request, procedure.ToResponse(),
                                     typeof(Procedure).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/procedures/{procedureUID}/bpmn-diagram")]
    public SingleObjectModel GetProcedureBpmnDiagram([FromUri] string procedureUID) {
      try {
        base.RequireResource(procedureUID, "procedureUID");

        var procedure = Procedure.Parse(procedureUID);

        return new SingleObjectModel(this.Request, procedure.BpmnDiagram.ToResponse(),
                                     typeof(WorkflowDefinition.BpmnDiagram).FullName);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/procedures")]
    public SingleObjectModel CreateProcedure([FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var procedure = new Procedure(bodyAsJson);

        procedure.Save();

        return new SingleObjectModel(this.Request, procedure.ToResponse(),
                                     typeof(Procedure).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPost]
    [Route("v1/procedures/update-all")]
    public void UpdateAllProcedures() {
      try {

        Procedure.UpdateAll();
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPut, HttpPatch]
    [Route("v1/procedures/{procedureUID}")]
    public SingleObjectModel UpdateProcedure([FromUri] string procedureUID,
                                             [FromBody] object body) {
      try {
        base.RequireResource(procedureUID, "procedureUID");
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var procedure = Procedure.Parse(procedureUID);

        procedure.Update(bodyAsJson);

        procedure.Save();

        return new SingleObjectModel(this.Request, procedure.ToResponse(), typeof(Procedure).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class ProcedureController

}  // namespace Empiria.Steps.WebApi
