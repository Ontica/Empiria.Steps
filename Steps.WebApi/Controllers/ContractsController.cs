/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : ContractsController                              License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Web api methods used to search and edit contracts.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.Legal;
using Empiria.Steps.Modeling;

namespace Empiria.Steps.WebApi {

  /// <summary>Web api methods used to search and edit contracts.</summary>
  public class ContractsController : WebApiController {

    #region Contracts Api

    [HttpGet]
    [Route("v1/contracts")]
    public CollectionModel GetContracts() {
      try {
        var contractDocumentType = LegalDocumentType.Contract;

        var contracts = Contract.GetList(contractDocumentType);

        return new CollectionModel(this.Request, BuildResponse(contracts),
                                   contractDocumentType.Name);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/contracts/{contractUID}")]
    public SingleObjectModel GetContract([FromUri] string contractUID) {
      try {

        var contract = Contract.Parse(contractUID);

        return new SingleObjectModel(this.Request, BuildResponse(contract),
                                     typeof(Contract).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Contracts Api

    #region Contract clauses Api

    [HttpGet]
    [Route("v1/contracts/{contractUID}/clauses")]
    public CollectionModel GetContractClausesList([FromUri] string contractUID) {
      try {
        var contract = Contract.Parse(contractUID);

        FixedList<Clause> clausesList = contract.Clauses;

        return new CollectionModel(this.Request, BuildResponse(clausesList),
                                   typeof(Clause).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/contracts/{contractUID}/clauses/{clauseUID}")]
    public SingleObjectModel GetContractClause([FromUri] string contractUID,
                                               [FromUri] string clauseUID) {
      try {
        var contract = Contract.Parse(contractUID);

        Clause clause = contract.GetClause(clauseUID);

        return new SingleObjectModel(this.Request, BuildResponseFull(clause),
                                     typeof(Clause).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/contracts/{contractUID}/clauses")]
    public SingleObjectModel CreateContractClause([FromUri] string contractUID,
                                                  [FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var contract = Contract.Parse(contractUID);

        Clause clause = contract.AddClause(bodyAsJson);

        return new SingleObjectModel(this.Request, BuildResponse(clause),
                                     typeof(Clause).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/contracts/{contractUID}/clauses/{clauseUID}")]
    public SingleObjectModel UpdateContractClause([FromUri] string contractUID,
                                                  [FromUri] string clauseUID,
                                                  [FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var contract = Contract.Parse(contractUID);

        Clause clause = contract.GetClause(clauseUID);

        clause.Update(bodyAsJson);

        return new SingleObjectModel(this.Request, BuildResponse(clause),
                                     typeof(Contract).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Contract clauses Api

    #region Contract clause related procedures Api

    [HttpPost]
    [Route("v1/contracts/{contractUID}/clauses/{clauseUID}/related-procedures")]
    public SingleObjectModel CreateContractClauseRelatedProcedure([FromUri] string contractUID,
                                                                  [FromUri] string clauseUID,
                                                                  [FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var contract = Contract.Parse(contractUID);

        Clause clause = contract.GetClause(clauseUID);

        RelatedProcedure relatedProcedure = clause.AddRelatedProcedure(bodyAsJson);

        return new SingleObjectModel(this.Request, BuildResponse(relatedProcedure),
                                     typeof(RelatedProcedure).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/contracts/{contractUID}/clauses/{clauseUID}/related-procedures/{relatedProcedureUID}")]
    public SingleObjectModel UpdateContractClauseRelatedProcedure([FromUri] string contractUID,
                                                                  [FromUri] string clauseUID,
                                                                  [FromUri] string relatedProcedureUID,
                                                                  [FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var contract = Contract.Parse(contractUID);

        Clause clause = contract.GetClause(clauseUID);

        RelatedProcedure relatedProcedure = clause.GetRelatedProcedure(relatedProcedureUID);

        relatedProcedure.Update(bodyAsJson);

        return new SingleObjectModel(this.Request, BuildResponse(relatedProcedure),
                                     typeof(RelatedProcedure).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Contract clause related procedures Api

    #region Private methods

    private ICollection BuildResponse(FixedList<Contract> list) {
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


    private object BuildResponse(Contract legalDocument) {
      return new {
        uid = legalDocument.UID,
        name = legalDocument.Name,
        url = legalDocument.Url,
        clauses = BuildResponse(legalDocument.Clauses),
      };
    }


    private ICollection BuildResponse(FixedList<Clause> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var documentItem in list) {
        var item = BuildResponse(documentItem);

        array.Add(item);
      }
      return array;
    }


    private object BuildResponse(Clause documentItem) {
      return new {
        uid = documentItem.UID,
        clauseNo = documentItem.Number,
        title = documentItem.Title,
        text = documentItem.Text,
        sourcePageNo = documentItem.DocumentPageNo
      };
    }


    private object BuildResponseFull(Clause documentItem) {
      return new {
        uid = documentItem.UID,
        clauseNo = documentItem.Number,
        title = documentItem.Title,
        text = documentItem.Text,
        sourcePageNo = documentItem.DocumentPageNo,
        notes = documentItem.Notes,
        status = documentItem.Status,
        relatedProcedures = BuildResponse(documentItem.RelatedProcedures),
        contract = new {
          uid = documentItem.Contract.UID,
          name = documentItem.Contract.Name,
          url = documentItem.Contract.Url
        }
      };
    }


    private ICollection BuildResponse(FixedList<RelatedProcedure> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var relatedProcedure in list) {
        var item = BuildResponse(relatedProcedure);

        array.Add(item);
      }
      return array;
    }


    private object BuildResponse(RelatedProcedure relatedProcedure) {
      return new {
        uid = relatedProcedure.UID,
        procedure = BuildResponse(relatedProcedure.Procedure),
        maxFilingTerm = relatedProcedure.MaxFilingTerm,
        maxFilingTermType = relatedProcedure.MaxFilingTermType,
        startsWhen = relatedProcedure.StartsWhen,
        startsWhenTrigger = relatedProcedure.StartsWhenTrigger,
        notes = relatedProcedure.Notes
      };
    }


    private object BuildResponse(Procedure procedure) {
      return new {
        uid = procedure.UID,
        shortName = procedure.ShortName,
        name = procedure.Name,
        code = procedure.Code,
        entity = procedure.Authority.Entity.Nickname
      };
    }

    #endregion Private methods

  }  // class ContractsController

}  // namespace Empiria.Steps.WebApi
