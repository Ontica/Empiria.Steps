/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Knowledge Services                   Component : Data Integration                        *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Data Services                           *
*  Type     : StepsDataRepository                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data repository with methods used to read and write steps data objects.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

using Empiria.ProjectManagement;
using Empiria.Steps.Design.DataObjects;


namespace Empiria.Steps.Design.Integration {

  /// <summary>Data repository with methods used to read and write steps data objects.</summary>
  static internal class StepsDataRepository {

    #region Methods

    internal static FixedList<StepDataObject> GetActionDataObjects(ProjectItem activity) {
      if (activity.IsEmptyInstance) {
        return new FixedList<StepDataObject>();
      }

      var sql = $"SELECT * FROM STStepsDataObjects WHERE " +
                $"(ActivityId = {activity.Id}) AND (StepDataObjectStatus <> 'X')";

      var op = DataOperation.Parse(sql);

      var list = DataReader.GetList<StepDataObject>(op);

      if (activity.HasTemplate) {
        var templateDataObjects = StepsDataRepository.GetDataObjects(activity.GetTemplate());

        list.AddRange(templateDataObjects.FindAll(x => !list.Contains(x)));
      }

      return list.ToFixedList();
    }


    internal static FixedList<StepDataObject> GetDataObjects(ProjectItem step) {
      var sql = $"SELECT * FROM STStepsDataObjects WHERE " +
                $"(StepId = {step.Id}) AND (ActivityId = -1) AND (StepDataObjectStatus <> 'X')";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<StepDataObject>(op);
    }


    internal static FixedList<StepDataObject> GetFormsData(int dataItemId) {
      var sql = $"SELECT * FROM STStepsDataObjects WHERE " +
                $"(DataItemId = {dataItemId}) AND (StepDataObjectStatus <> 'X')";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<StepDataObject>(op);
    }


    internal static void WriteStepDataObject(StepDataObject o) {
      var op = DataOperation.Parse("writeSTStepDataObject", o.Id, o.UID,
                    o.DataItem.Id, o.Step.Id, o.Activity.Id,
                    o.MediaFile.Id, o.FormId, o.FormData.ToString(),
                    o.Configuration.ToString(), o.ExtensionData.ToString(),
                    (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteStepData(StepDataHolder o) {
      var op = DataOperation.Parse("writeSTStep", o.Id, o.UID, o.StepType.Id,
                    o.StepKind, o.StepName, o.Notes, o.Themes, o.Tags, o.Keywords,
                    o.ExtensionData.ToString(), o.ForeignLanguageData.ToString(),
                    o.Constraints, o.ExecutionContext,
                    o.DataModels, o.DefinedMacro, o.MicroWorkflowModel, o.Accessibility,
                    o.DrivenMode, o.FlowControl, o.WorkItemType, o.StepRole, o.ProcessStage,
                    o.IsOptional, o.LoopControl, o.PeriodicityRule.ToString(), o.EstimatedDuration.ToString(),
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
