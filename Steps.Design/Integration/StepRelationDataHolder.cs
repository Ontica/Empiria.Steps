/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Integration Types                       *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Data Services                           *
*  Type     : StepRelationDataHolder                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data structure used to hold data for StepRelation relationships.                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Json;

using Empiria.StateEnums;

namespace Empiria.Steps.Design.Integration {

  /// <summary>Data structure used to hold data for StepRelation relationships.</summary>
  internal class StepRelationDataHolder : BaseObject {

    #region Constructors and parsers

    internal StepRelationDataHolder() {

    }

    internal StepRelationDataHolder Parse(string uid) {
      return BaseObject.ParseKey<StepRelationDataHolder>(uid);
    }

    #endregion Constructors and parsers

    #region Fields

    public StepRelationType StepRelationType {
      get;
      internal set;
    } = StepRelationType.Dependency;


    public string RelationKind {
      get;
      internal set;
    } = String.Empty;


    public string RelationName {
      get;
      internal set;
    } = String.Empty;


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.RelationName);
      }
    }


    public JsonObject ExtensionData {
      get;
      internal set;
    } = new JsonObject();


    public string Constraints {
      get;
      internal set;
    } = String.Empty;


    public JsonObject ExecutionContext {
      get;
      internal set;
    } = new JsonObject();


    public string DataModels {
      get;
      internal set;
    } = String.Empty;


    public JsonObject DueOnRule {
      get;
      internal set;
    } = JsonObject.Empty;


    public string Accessibility {
      get;
      internal set;
    } = String.Empty;


    public string DrivenMode {
      get;
      internal set;
    } = String.Empty;


    public string FlowControl {
      get;
      internal set;
    } = String.Empty;


    public string WorkSequenceKind {
      get;
      internal set;
    } = "FinishToFinish";


    public string RelationRole {
      get;
      internal set;
    } = String.Empty;


    public int SourceId {
      get;
      internal set;
    } = -1;


    public int TargetId {
      get;
      internal set;
    } = -1;


    public int RelationIndex {
      get;
      internal set;
    }


    public int RelationInstanceId {
      get;
      internal set;
    } = -1;


    public int DerivedFromRelationId {
      get;
      internal set;
    } = -1;


    public int OwnerId {
      get;
      internal set;
    } = -1;


    public EntityStatus Status {
      get;
      internal set;
    } = EntityStatus.Pending;


    public DateTime FromDate {
      get;
      internal set;
    } = ExecutionServer.DateMinValue;


    public DateTime ToDate {
      get;
      internal set;
    } = ExecutionServer.DateMaxValue;


    #endregion Fields

    #region Methods

    protected override void OnSave() {
      StepsDataRepository.WriteStepRelationData(this);
    }

    #endregion Methods

  }  // class StepRelationDataHolder

}  // namespace Empiria.Steps.Design.Integration
