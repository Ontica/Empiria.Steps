/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps-Legal Integration Services             Component : Integration Layer                     *
*  Assembly : Empiria.Steps.dll                            Pattern   : Information provider                  *
*  Type     : ProcessLegalDataBuilder                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Links legal data form process items with contracts.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;
using Empiria.Governance.Contracts;
using Empiria.ProjectManagement;

namespace Empiria.Steps.Services {

  /// <summary>Provides top-down legal data for a process or a branch.</summary>
  internal class ProcessLegalDataBuilder {

    private readonly FixedList<Activity> _processItems;
    private readonly Contract _contract;
    private Lazy<FixedList<LegalData>> _legalDataList;

    #region Public methods

    internal ProcessLegalDataBuilder(FixedList<Activity> processItems, Contract contract) {
      _processItems = processItems;
      _contract = contract;
      _legalDataList = new Lazy<FixedList<LegalData>>(() => BuildLegalDataList());
    }


    internal FixedList<LegalData> GetLegalDataList() {
      return _legalDataList.Value;
    }


    internal FixedList<LegalData> GetLegalDataList(Clause clause) {
      var list = _legalDataList.Value.FindAll((x) => x.ContractClauseId == clause.Id);

      return list;
    }


    internal void Refresh() {
      _legalDataList = new Lazy<FixedList<LegalData>>(() => BuildLegalDataList());
    }

    #endregion Public methods

    #region Private methods

    private FixedList<LegalData> BuildLegalDataList() {
      var legalBasisList = new List<LegalData>(_processItems.Count);

      foreach (var processItem in _processItems) {
        ActivityModel template = processItem.Template;

        string obligationClause = InitialCleaning(template.ContractClause);

        obligationClause = CleanLegalBasis(obligationClause);

        if (obligationClause.Length == 0) {
          continue;
        }

        var split = obligationClause.Split(';');

        foreach (var item in split) {
          var t = InitialCleaning(item);

          if (t.Length == 0) {
            continue;
          }

          t = CleanLegalBasis(t);

          if (t.Length == 0) {
            continue;
          }
          FixedList<Clause> matchedClauses = _contract.MatchClauses(t);

          if (matchedClauses.Count == 0) {
            var legalData = new LegalData(Clause.Empty, processItem,
                                          template.ContractClause, t, template.LegalBasis);

            legalBasisList.Add(legalData);
          }

          foreach (var match in matchedClauses) {
            var legalData = new LegalData(match, processItem,
                                          template.ContractClause, t, template.LegalBasis);

            if (!legalBasisList.Exists(x => x.ContractClauseId == legalData.ContractClauseId &&
                                            x.ObligationId == legalData.ObligationId)) {
              legalBasisList.Add(legalData);
            }
          }

        } // foreach

      }  // foreach

      return legalBasisList.OrderBy(x => x.Obligation)
                           .ThenBy(x => x.ContractClause)
                           .ToList().ToFixedList();
    }


    static private string InitialCleaning(string legalBasis) {
      var temp = EmpiriaString.TrimAll(legalBasis);

      temp = temp.Replace(",", ", ");
      temp = temp.Replace(")", ") ");
      temp = temp.Replace(" , ", ", ");
      temp = temp.Replace(". ", " ");

      temp = EmpiriaString.TrimAll(temp);

      temp = EmpiriaString.TrimControl(temp);

      temp = EmpiriaString.RemoveEndPunctuation(temp);

      if (temp.StartsWith("No aplica")) {
        return String.Empty;
      }

      if (EmpiriaString.DamerauLevenshteinProximityFactor(temp, "No aplicable") > 0.70m) {
        return String.Empty;
      }

      return EmpiriaString.TrimAll(temp);
    }


    static private string CleanLegalBasis(string legalBasis) {
      var temp = legalBasis;

      temp = temp.Replace("Clausula", "Cláusula");
      temp = temp.Replace("Cláusulas", "Cláusula");
      temp = temp.Replace("Anexos", "Anexo");
      temp = temp.Replace(" y Anexo", "; Anexo");


      if (EmpiriaString.IsQuantity(EmpiriaString.RemoveEndPunctuation(temp.Split(' ')[0]))) {
        temp = "Cláusula " + temp;
      }

      if (temp.StartsWith("Cláusula")) {
        var parts = temp.Split(',');

        var s = String.Empty;

        foreach (var part in parts) {
          var x = EmpiriaString.TrimAll(part);

          if (s.Length == 0) {
            s = part;
            continue;
          }

          if (EmpiriaString.IsQuantity(EmpiriaString.RemoveEndPunctuation(x))) {
            s = $"{s}; {part}";
          } else {
            s = $"{s}, {part}";
          }

        } // foreach
        temp = s;
      }

      temp = EmpiriaString.TrimAll(temp);

      return temp;
    }

    #endregion Private methods

  }  // class ProcessLegalDataProvider

} // namespace Empiria.Steps.Services
