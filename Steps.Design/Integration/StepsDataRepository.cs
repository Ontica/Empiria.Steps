/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Knowledge Services                   Component : Data Integration                        *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Data Services                           *
*  Type     : StepsDataRepository                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data repository with methods used to read and write steps definition data.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Steps.Design.Integration {

  /// <summary>Data repository with methods used to read and write steps definition data.</summary>
  static internal class StepsDataRepository {

    #region Methods

    //static internal FixedList<StepData> GetList(string keywords) {
    //  var op = DataOperation.Parse("writeSTStep", o.Id, o.UID

    //  return DataReader.GetPlainObjectFixedList<StepData>(filtxer, sort).ToFixedList();
    //}


    static internal void WriteStepData(StepDataHolder o) {
      var op = DataOperation.Parse("writeSTStep", o.Id, o.UID, o.StepType.Id,
                    o.StepKind, o.StepName, o.Notes, o.Themes, o.Tags, o.Keywords,
                    o.ExtensionData.ToString(), o.Constraints, o.ExecutionContext,
                    o.DataModels, o.DefinedMacro, o.MicroWorkflowModel, o.Accessibility,
                    o.DrivenMode, o.FlowControl, o.WorkItemType, o.StepRole, o.ProcessStage,
                    o.IsOptional, o.LoopControl, o.PeriodicityRule, o.EstimatedDuration.ToString(),
                    o.DerivedFromStepId, o.NewVersionOfStepId, o.ReferenceOfStepId, o.ParentStepId,
                    o.StepPosition, o.BaseStepId, o.OwnerId, o.ProcedureEntityId, o.ProcedureId,
                    (char) o.Status, o.OldProjectObjectId);

      DataWriter.Execute(op);
    }


    static internal void WriteStepRelationData(StepRelationDataHolder o) {
      var op = DataOperation.Parse("writeSTStepRelation", o.Id, o.UID, o.StepRelationType.Id,
              o.RelationKind, o.RelationName, o.Keywords, o.ExtensionData.ToString(),
              o.Constraints, o.ExecutionContext, o.DataModels, o.DueOnRule.ToString(),
              o.Accessibility, o.DrivenMode, o.FlowControl, o.WorkSequenceKind, o.RelationRole,
              o.SourceId, o.TargetId, o.RelationIndex, o.RelationInstanceId, o.DerivedFromRelationId,
              o.OwnerId, (char) o.Status, o.FromDate, o.ToDate);

      DataWriter.Execute(op);
    }


    #endregion Methods

  }  // class StepsDataRepository

}  // namespace Empiria.Steps.Knowledge
