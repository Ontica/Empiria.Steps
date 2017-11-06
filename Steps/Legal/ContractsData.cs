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

using Empiria.Data;

namespace Empiria.Steps.Legal {

  /// <summary>Data read and write methods for contracts and their clauses and annexes.</summary>
  static internal class ContractsData {

    static internal void WriteClause(Clause o) {
      var op = DataOperation.Parse("writeBPMDocumentItems",
                        o.Id, o.UID, o.ContractId, o.Section, o.Number,
                        o.Ordering, o.Title, o.DocumentPageNo, o.Text,
                        o.Notes, o.Keywords, o.Status);

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

    static internal void WriteDocumentRule(DocumentRule o) {
      var op = DataOperation.Parse("writeLISDocumentRule",
                        o.Id, o.UID, o.DocumentId, o.DocumentItemId, o.Position,
                        o.Name, (char) o.RuleType, o.ReferenceRuleId, o.Description,
                        o.Notes, o.AppliesTo, o.Verb, o.Action, o.WhenPredicate,
                        o.ActionTimeCondition, o.ExtensionData.ToString(),
                        o.Tags, o.Keywords, o.WorkflowObjectId, o.DocumentItems);

      DataWriter.Execute(op);
    }

  }  // class ContractsData

}  // namespace Empiria.Steps.Legal
