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
using System.Collections;
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
    public CollectionModel GetProceduresList([FromUri] string filter = "") {
      try {
        var list = Procedure.GetList(filter ?? String.Empty);

        return new CollectionModel(this.Request, BuildResponse(list), typeof(Procedure).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/procedures/{procedure_UID}")]
    public SingleObjectModel GetProcedure([FromUri] string procedure_UID) {
      try {
        base.RequireResource(procedure_UID, "procedure_UID");

        var procedure = Procedure.Parse(procedure_UID);

        return new SingleObjectModel(this.Request, BuildResponse(procedure), typeof(Procedure).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/procedures/{procedure_UID}/bpmn-diagram")]
    public SingleObjectModel GetProcedureBpmnDiagram([FromUri] string procedure_UID) {
      try {
        base.RequireResource(procedure_UID, "procedure_UID");

        var procedure = Procedure.Parse(procedure_UID);

        return new SingleObjectModel(this.Request, procedure.BpmnDiagram.ToResponse(), typeof(WorkflowDefinition.BpmnDiagram).FullName);
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

        return new SingleObjectModel(this.Request, BuildResponse(procedure), typeof(Procedure).FullName);

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

        return new SingleObjectModel(this.Request, BuildResponse(procedure), typeof(Procedure).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

    #region Private methods

    private ICollection BuildResponse(FixedList<Procedure> list) {
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

    private object BuildResponse(Procedure procedure) {
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

        authority = BuildResponse(procedure.Authority),
        legalInfo = procedure.LegalInfo,
        filingCondition = procedure.FilingCondition,
        filingDocuments = procedure.FilingDocuments,
        filingFee = procedure.FilingFee,
        notes = procedure.Notes
      };
    }

    private object BuildResponse(Authority authority) {
      return new {
        entity = new {
          uid = authority.Entity.UID,
          name = authority.Entity.FullName,
        },
        office = new {
          uid = authority.Office.UID,
          name = authority.Office.FullName,
        },
        position = new {
          uid = authority.Position.UID,
          name = authority.Position.FullName,
          phone = authority.Position.Phone
        },
        contact = new {
          uid = authority.Position.Officer.UID,
          name = authority.Position.Officer.FullName,
          email = authority.Position.Officer.EMail
        }
      };
    }

    #endregion Private methods

  }  // class ProcedureController

}  // namespace Empiria.Steps.WebApi
