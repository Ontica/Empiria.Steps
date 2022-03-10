/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Process Management                           Component : Domain services                       *
*  Assembly : Empiria.Workflow.dll                         Pattern   : Partitioned domain object             *
*  Type     : BaseProcessObject                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Abstract class that describes a process object, like an activity, event or gateway.            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Collections;
using Empiria.Json;
using Empiria.Ontology;

namespace Empiria.Workflow.Definition {

  /// <summary>Abstract class that describes a process object, like an activity, event or gateway.</summary>
  [PartitionedType(typeof(ProcessObjectType))]
  public abstract class BaseProcessObject : BaseObject {

    #region Constructors and parsers

    protected BaseProcessObject(ProcessObjectType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public BaseProcessObject Empty {
      get {
        return BaseObject.ParseEmpty<BaseProcessObject>();
      }
    }


    #endregion Constructors and parsers

    #region Properties

    public ProcessObjectType ProcessObjectType {
      get {
        return (ProcessObjectType) base.GetEmpiriaType();
      }
    }


    [DataField("Name")]
    public string Name {
      get;
      private set;
    }


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }


    [DataField("Theme")]
    public string Theme {
      get;
      private set;
    }


    [DataField("Tags")]
    public TagsCollection Tags {
      get;
      private set;
    } = TagsCollection.Empty;


    [DataField("ExtData")]
    protected internal JsonObject ExtensionData {
      get;
      private set;
    } = new JsonObject();


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Name, this.Theme, this.Tags.ToString(), this.Process.Name);
      }
    }


    [DataField("Tags")]
    public TagsCollection ProcessingTags {
      get;
      private set;
    } = TagsCollection.Empty;


    [DataField("ActivityType")]
    public string ActivityType {
      get;
      private set;
    } = String.Empty;


    [DataField("ExecutionMode")]
    public string ExecutionMode {
      get;
      private set;
    } = String.Empty;


    [DataField("IsMandatory")]
    public bool IsMandatory {
      get;
      private set;
    } = false;


    [DataField("IsController")]
    public bool IsController {
      get;
      private set;
    } = false;


    [DataField("DueOnTerm")]
    public string DueOnTerm {
      get;
      private set;
    } = String.Empty;


    [DataField("DueOnTermUnit")]
    public string DueOnTermUnit {
      get;
      private set;
    } = String.Empty;


    [DataField("DueOnCondition")]
    public string DueOnCondition {
      get;
      private set;
    } = String.Empty;


    [DataField("DueOnController")]
    public int DueOnControllerId {
      get;
      private set;
    } = -1;


    [DataField("DueOnRuleAppliesForAllContracts")]
    public string DueOnRuleAppliesForAllContracts {
      get;
      private set;
    } = String.Empty;


    [DataField("Duration")]
    public string Duration {
      get;
      private set;
    } = String.Empty;


    [DataField("DurationUnit")]
    public string DurationUnit {
      get;
      private set;
    } = String.Empty;


    [DataField("Periodicity")]
    public string Periodicity {
      get;
      private set;
    } = String.Empty;


    [DataField("EntityId")]
    public int EntityId {
      get;
      private set;
    } = -1;


    [DataField("ProcedureId")]
    public int ProcedureId {
      get;
      private set;
    } = -1;


    [DataField("ContractClause")]
    public string ContractClause {
      get;
      private set;
    } = String.Empty;


    [DataField("LegalBasis")]
    public string LegalBasis {
      get;
      private set;
    } = String.Empty;


    [DataField("ObjectPosition")]
    public int Position {
      get;
      private set;
    }


    [DataField("BaseProcessId")]
    public Process Process {
      get;
      private set;
    }


    #endregion Properties


  } // class BaseProcessObject

} // namespace Empiria.Workflow.Definition
