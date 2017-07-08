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
      DataTable table = GeneralDataOperations.GetEntities("vwBPMProcedures",
                                                          filter, "Stage, MSExcelNo, Name");

      return BaseObject.ParseList<Procedure>(table).ToFixedList();
    }

    static internal void WriteProcedure(Procedure o) {
      var op = DataOperation.Parse("writeBPMProcedure",
                o.Id, o.UID, o.Name, o.ShortName, o.Code, o.Notes, o.URL,
                o.Stage, o.Category, o.Theme,
                o.Authority.Entity.Id, o.Authority.Office.Id, o.Authority.Position.Id,
                o.LegalInfo.IsRegulated, o.LegalInfo.Obligation, o.LegalInfo.LegalBasis,
                o.FilingCondition.StartsWhen, o.FilingCondition.StartsWhenNotes, o.FilingCondition.StartsWhenTrigger,
                o.FilingCondition.MaxFilingTerm, o.FilingCondition.MaxFilingTermNotes,
                o.FilingCondition.IssuanceLegalTerm, o.FilingCondition.IssuanceLegalTermNotes,

                o.FilingCondition.HowToFile, o.FilingCondition.AllowsDeferrals,
                o.FilingCondition.DeferralsTermNotes, o.FilingCondition.DeferralsConditionNotes,
                o.FilingDocuments.Notes,
                o.FilingCondition.ValidityTermWhenIssued, o.FilingCondition.SimultaneousDelivery,
                o.FilingFee.FilingFeeType, o.FilingFee.FeeAmount,
                o.FilingFee.Rule, o.FilingFee.LegalBasis, o.Keywords, o.StatusNotes, (char) o.Status,

                o.LegalInfo.ContractClausesAndAnnexes, o.MSExcelNo);

      DataWriter.Execute(op);
    }

  }  // class ProcedureData

}  // namespace Empiria.Steps.Modeling
