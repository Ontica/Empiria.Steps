/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Integration Types                       *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Data Services                           *
*  Type     : StepDataHolder                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data structure used to hold data for Step entities.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.DataTypes;
using Empiria.Json;

using Empiria.StateEnums;

namespace Empiria.Steps.Design.Integration {

  /// <summary>Data structure used to hold data for Step entities.</summary>
  internal class StepDataHolder : BaseObject {

    #region Constructors and parsers

    internal StepDataHolder() {

    }


    static internal StepDataHolder Parse(string uid) {
      return BaseObject.ParseKey<StepDataHolder>(uid);
    }


    #endregion Constructors and parsers

    #region Fields

    public StepType StepType {
      get;
      internal set;
    } = StepType.Task;


    [DataField("StepKind")]
    public string StepKind {
      get;
      internal set;
    } = String.Empty;


    [DataField("StepName")]
    public string StepName {
      get;
      internal set;
    } = String.Empty;


    [DataField("StepNotes")]
    public string Notes {
      get;
      internal set;
    } = String.Empty;


    [DataField("Themes")]
    public string Themes {
      get;
      internal set;
    } = String.Empty;


    [DataField("Tags")]
    public string Tags {
      get;
      internal set;
    } = String.Empty;


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.StepName, this.Tags, this.Themes, this.Notes);
      }
    }


    [DataField("StepExtData")]
    public JsonObject ExtensionData {
      get;
      internal set;
    } = new JsonObject();


    [DataField("ForeignLangData")]
    public JsonObject ForeignLanguageData {
      get;
      internal set;
    } = new JsonObject();


    [DataField("Constraints")]
    public string Constraints {
      get;
      internal set;
    } = String.Empty;


    [DataField("ExecutionContext")]
    public string ExecutionContext {
      get;
      internal set;
    } = String.Empty;


    [DataField("DataModels")]
    public string DataModels {
      get;
      internal set;
    } = String.Empty;


    [DataField("DefinedMacro")]
    public string DefinedMacro {
      get;
      internal set;
    } = String.Empty;


    [DataField("MicroWorkflowModel")]
    public string MicroWorkflowModel {
      get;
      internal set;
    } = String.Empty;


    [DataField("Accessibility")]
    public string Accessibility {
      get;
      internal set;
    } = String.Empty;


    [DataField("DrivenMode")]
    public string DrivenMode {
      get;
      internal set;
    } = String.Empty;


    [DataField("FlowControl")]
    public string FlowControl {
      get;
      internal set;
    } = String.Empty;


    [DataField("WorkItemType")]
    public string WorkItemType {
      get;
      internal set;
    } = String.Empty;


    [DataField("StepRole")]
    public string StepRole {
      get;
      internal set;
    } = String.Empty;


    [DataField("ProcessStage")]
    public string ProcessStage {
      get;
      internal set;
    } = String.Empty;


    [DataField("IsOptional")]
    public bool IsOptional {
      get;
      internal set;
    }


    [DataField("LoopControl")]
    public string LoopControl {
      get;
      internal set;
    } = String.Empty;


    [DataField("PeriodicityRule")]
    public JsonObject PeriodicityRule {
      get;
      internal set;
    } = new JsonObject();


    [DataField("EstimatedDuration")]
    public Duration EstimatedDuration {
      get;
      internal set;
    } = Duration.Empty;


    [DataField("DerivedFromStepId")]
    public int DerivedFromStepId {
      get;
      internal set;
    } = -1;


    [DataField("NewVersionOfStepId")]
    public int NewVersionOfStepId {
      get;
      internal set;
    } = -1;


    [DataField("ReferenceOfStepId")]
    public int ReferenceOfStepId {
      get;
      internal set;
    } = -1;


    [DataField("ParentStepId")]
    public int ParentStepId {
      get;
      internal set;
    } = -1;


    [DataField("StepPosition")]
    public int StepPosition {
      get;
      internal set;
    } = -1;


    [DataField("BaseStepId")]
    public int BaseStepId {
      get;
      internal set;
    } = -1;


    [DataField("OwnerId")]
    public int OwnerId {
      get;
      internal set;
    } = -1;


    [DataField("ProcedureEntityId")]
    public int ProcedureEntityId {
      get;
      internal set;
    } = -1;


    [DataField("ProcedureId")]
    public int ProcedureId {
      get;
      internal set;
    } = -1;


    [DataField("DesignStatus", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get;
      internal set;
    } = EntityStatus.Pending;


    [DataField("OldProjectObjectId")]
    public int OldProjectObjectId {
      get;
      internal set;
    } = -1;


    #endregion Fields

    #region Methods

    protected override void OnSave() {
      StepsDataRepository.WriteStepData(this);
    }

    #endregion Methods

  }  // class StepDataHolder

}  // namespace Empiria.Steps.Design.Integration
