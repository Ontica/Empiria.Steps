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
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.Legal;

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

        return new CollectionModel(this.Request, contracts.ToResponse(),
                                   contractDocumentType.Name);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/contracts/{contractUID}")]
    public SingleObjectModel GetContract([FromUri] string contractUID,
                                         [FromUri] string keywords = "") {
      try {
        keywords = keywords ?? String.Empty;

        var contract = Contract.Parse(contractUID);

        FixedList<Clause> clausesList = contract.GetClauses(keywords);

        return new SingleObjectModel(this.Request, contract.ToResponse(clausesList),
                                     typeof(Contract).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Contracts Api

    #region Contract clauses Api

    [HttpGet]
    [Route("v1/contracts/{contractUID}/clauses")]
    public CollectionModel GetContractClausesList([FromUri] string contractUID,
                                                  [FromUri] string keywords = "") {
      try {
        keywords = keywords ?? String.Empty;

        var contract = Contract.Parse(contractUID);

        FixedList<Clause> clausesList = contract.GetClauses(keywords);

        return new CollectionModel(this.Request, clausesList.ToResponse(),
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

        return new SingleObjectModel(this.Request, clause.ToResponse(),
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

        return new SingleObjectModel(this.Request, clause.ToShortResponse(),
                                     typeof(Clause).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/contracts/{contractUID}/clauses/update-all")]
    public void UpdateAllContractClause() {
      try {

        Clause.UpdateAll();
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

        return new SingleObjectModel(this.Request, clause.ToResponse(),
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

        return new SingleObjectModel(this.Request, relatedProcedure.ToResponse(),
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

        return new SingleObjectModel(this.Request, relatedProcedure.ToResponse(),
                                     typeof(RelatedProcedure).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Contract clause related procedures Api

  }  // class ContractsController

}  // namespace Empiria.Steps.WebApi
