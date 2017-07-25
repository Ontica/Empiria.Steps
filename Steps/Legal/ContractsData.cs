/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Data Service                        *
*  Type     : ContractsData                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Data read and write methods for contracts their clauses and annexes.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Steps.Legal {

  /// <summary>Data read and write methods for contracts and their clauses and annexes.</summary>
  static internal class ContractsData {

    static internal void WriteClause(Clause o) {
      var op = DataOperation.Parse("writeBPMLegalDocumentItems",
                        o.Id, o.UID, o.ContractId, o.ItemTypeId,
                        o.Title, o.Number, o.Text, -1, o.DocumentPageNo,
                        o.Notes, o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteRelatedProcedure(RelatedProcedure o) {
      var op = DataOperation.Parse("writeBPMRelatedProcedure",
                        o.Id, o.UID, o.LegalDocumentItemId, o.Procedure.Id,
                        o.ExtensionData.ToString(), o.MaxFilingTerm, o.MaxFilingTermType,
                        o.StartsWhen, o.StartsWhenTrigger, o.ItemPosition,
                        o.Notes, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class ContractsData

}  // namespace Empiria.Steps.Legal
