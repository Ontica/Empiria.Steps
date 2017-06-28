/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Data Service                        *
*  Type     : ProcedureData                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Process definition data read and write methods.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Steps.Modeling {

  static internal class ProcedureData {

    static internal FixedList<Procedure> GetProcedureList(string filter = "") {
      DataTable table = GeneralDataOperations.GetEntities("BPMProcedures",
                                                          filter, "Stage, StageInnerNo, ProcedureName");

      return BaseObject.ParseList<Procedure>(table).ToFixedList();
    }

    static internal void WriteProcedure(Procedure o) {
      var op = DataOperation.Parse("writeBPMProcedure",
                        o.Id, o.UID, o.Name, o.Obligation, o.URL, o.Entity,
                        o.Authority, o.AuthorityContact, o.IsRegulated, o.Requirements,
                        o.Stage, o.StageInnerNo, o.Category, o.Theme, o.ContractClausesAndAnnexes,
                        o.LegalBasis, o.StartsWhen, o.MaxComplianceTerm, o.EmissionLegalTerm,
                        o.Deferrals, o.DeferralsTerm, o.Cost, o.CostLegalBasis,
                        o.ValidityTermWhenEmitted, o.SimultaneousDelivery, o.Notes);

      DataWriter.Execute(op);
    }

  }  // class ProcedureData

}  // namespace Empiria.Steps.Modeling
