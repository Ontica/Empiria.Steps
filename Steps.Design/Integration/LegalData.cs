/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps-Legal Integration Services             Component : Integration Layer                     *
*  Assembly : Empiria.Steps.dll                            Pattern   : Information holder                    *
*  Type     : LegalData                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides top-down legal data for a process or a branch.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Governance.Contracts;
using Empiria.ProjectManagement;

namespace Empiria.Steps.Services {

  public class LegalData {

    public LegalData(Clause contractClause, ProjectItem processItem,
                     string obligationClause, string detectedClause, string legalBasis) {
      this.ContractClause =  $"{contractClause.Number} {contractClause.Title}";

      this.DetectedClause = detectedClause;
      this.ContractClauseId = contractClause.Id;
      this.ObligationClause = obligationClause;

      this.LegalBasis = legalBasis;
      this.Obligation = processItem.Name;
      this.ObligationId = processItem.Id;
      this.Parent = processItem.Parent.Name;
      this.Project = processItem.Project.Name;
      this.StepUID = processItem.UID;
    }


    public string Name {
      get {
        return this.Obligation;
      }
    }

    public string ContractClause {
      get;
    }

    public string DetectedClause {
      get;
    }

    public int ContractClauseId {
      get;
    }

    public string ObligationClause {
      get;
    }

    public string LegalBasis {
      get;
    }

    public string Obligation {
      get;
    }

    public int ObligationId {
      get;
    }

    public string Parent {
      get;
    }

    public string Project {
      get;
    }

    public string StepUID {
      get;
    }

  }  // class LegalData

}  // namespace Empiria.Steps.Services
