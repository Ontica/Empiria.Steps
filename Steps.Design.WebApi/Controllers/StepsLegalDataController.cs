/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps-Legal Integration Services             Component : Web Api                               *
*  Assembly : Empiria.Steps.WebApi.dll                     Pattern   : Controller                            *
*  Type     : StepsLegalDataController                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web api methods that return steps related legal data.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Steps.Services;

namespace Empiria.Steps.WebApi {

  /// <summary>Web api methods that return steps related legal data.</summary>
  public class StepsLegalDataController : WebApiController {

    #region Export methods

    [HttpGet]
    [Route("v3/empiria-steps/processes/legal-data")]
    public CollectionModel ProcessLegalData() {
      try {
        FixedList<LegalData> legalData = StepsLegalDataServices.ProcessLegalData();

        return new CollectionModel(this.Request, legalData);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v3/empiria-steps/legal-data/{clauseUID}")]
    public CollectionModel ProcessLegalData([FromUri] string clauseUID) {
      try {
        FixedList<LegalData> legalData = StepsLegalDataServices.ContractClauseLegalData(clauseUID);

        return new CollectionModel(this.Request, legalData);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v3/empiria-steps/processes/{processUID:guid}/legal-data/{branchUID?}")]
    public CollectionModel ProcessLegalData([FromUri] string processUID,
                                            [FromUri] string branchUID) {
      try {
        FixedList<LegalData> legalData = StepsLegalDataServices.ProcessLegalData(processUID, branchUID);

        return new CollectionModel(this.Request, legalData);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v3/empiria-steps/legal-data/{legalItemUID}/obligations")]
    public CollectionModel GetContractClauseRulesList([FromUri] string legalItemUID) {
      try {
        FixedList<LegalData> legalData = StepsLegalDataServices.ContractClauseLegalData(legalItemUID);

        return new CollectionModel(this.Request, legalData);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Export methods

  }  // class StepsLegalDataController

}  // namespace Empiria.Steps.WebApi
