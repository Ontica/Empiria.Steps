/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : DocumentRulesController                          License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : API to get and set documents rules data.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.Legal;

namespace Empiria.Steps.WebApi {

  /// <summary>API to get and set documents rules data.</summary>
  public class DocumentRulesController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/contracts/{contractUID}/clauses/{clauseUID}/rules")]
    public SingleObjectModel GetContractClauseRulesList([FromUri] string contractUID,
                                                        [FromUri] string clauseUID) {
      try {
        var contract = Contract.Parse(contractUID);

        Clause clause = contract.GetClause(clauseUID);

        FixedList<DocumentRule> rules = clause.Rules;

        return new SingleObjectModel(this.Request, rules.ToResponse(clause),
                                     typeof(DocumentRule).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

    #region UPDATE methods

    [HttpPost]
    [Route("v1/contracts/rules/update-all")]
    public void UpdateAllContractsRules() {
      try {
        DocumentRule.UpdateAll();

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class DocumentRulesController

}  // namespace Empiria.Steps.WebApi
