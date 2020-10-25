/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps-Legal Integration Services             Component : Service Layer                         *
*  Assembly : Empiria.Steps.dll                            Pattern   : Static services                       *
*  Type     : StepsLegalDataServices                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Services to provide steps related legal data.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Governance.Contracts;
using Empiria.ProjectManagement;

namespace Empiria.Steps.Services {

  /// <summary>Services to provide steps related legal data.</summary>
  static public class StepsLegalDataServices {

    static public FixedList<LegalData> ContractClauseLegalData(string clauseUID) {
      Assertion.AssertObject(clauseUID, "clauseUID");

      var clause = Clause.Parse(clauseUID);

      ProcessLegalDataBuilder provider = GetLegalDataProvider();

      return provider.GetLegalDataList(clause);
    }


    static public FixedList<LegalData> ProcessLegalData() {
      ProcessLegalDataBuilder provider = GetLegalDataProvider();

      return provider.GetLegalDataList();
    }


    static public FixedList<LegalData> ProcessLegalData(string processUID, string branchUID) {
      Assertion.AssertObject(processUID, "processUID");

      var process = Project.Parse(processUID);

      ProjectItem branch = GetProjectItem(branchUID);

      FixedList<Activity> processItems = GetProcessItemsList(process, branch);

      Contract contract = GetTargetContract();

      var legalDataProvider = new ProcessLegalDataBuilder(processItems, contract);

      return legalDataProvider.GetLegalDataList();
    }


    #region Utility methods

    static private ProcessLegalDataBuilder GetLegalDataProvider() {
      var templates = Project.GetTemplatesList();

      FixedList<Activity> processItems = GetProcessItemsList(templates);

      Contract contract = GetTargetContract();

      return new ProcessLegalDataBuilder(processItems, contract);
    }


    static private ProjectItem GetProjectItem(string branchUID) {
      if (String.IsNullOrEmpty(branchUID)) {
        return ProjectItem.Empty;
      } else {
        return ProjectItem.Parse(branchUID);
      }
    }

    static private FixedList<Activity> GetProcessItemsList(Project process,
                                                           ProjectItem branch) {
      FixedList<ProjectItem> list;

      if (branch.IsEmptyInstance) {
        list = process.GetItems();
      } else {
        list = process.GetBranch(branch);
      }

      return list.ConvertAll(x => (Activity) x)
                 .ToFixedList();
    }


    private static FixedList<Activity> GetProcessItemsList(FixedList<Project> projectsList) {
      List<Activity> list = new List<Activity>();

      foreach (var project in projectsList) {
        var items = project.GetItems()
                           .ConvertAll(x => (Activity) x);

        list.AddRange(items);
      }

      return list.ToFixedList();
    }


    static private Contract GetTargetContract() {
      return Contract.Parse("56101bbf-487d-4b09-b8af-6d6bfdf4b415");
    }

    #endregion Utility methods

  }  // class StepsLegalDataServices

}  // namespace Empiria.Steps.Services
