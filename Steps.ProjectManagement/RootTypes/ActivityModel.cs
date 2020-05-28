/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : DTO                                 *
*  Type     : ActivityModel                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : A project activity model DTO.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.ProjectManagement {

  /// <summary>A project activity model DTO.</summary>
  public class ActivityModel {

    #region Constructors and parsers

    private ActivityModel() {

    }


    static internal ActivityModel Parse(JsonObject data) {
      var o = new ActivityModel();

      o.ActivityType = data.Get<string>("activityType", o.ActivityType);
      o.ExecutionMode = data.Get<string>("executionMode", o.ExecutionMode);
      o.IsMandatory = data.Get<bool>("isMandatory", o.IsMandatory);
      o.IsController = data.Get<bool>("isController", o.IsController);

      o.DueOnTerm = data.Get<string>("dueOnTerm", o.DueOnTerm);
      o.DueOnTermUnit = data.Get<string>("dueOnTermUnit", o.DueOnTermUnit);
      o.DueOnCondition = data.Get<string>("dueOnCondition", o.DueOnCondition);
      o.DueOnControllerId = data.Get<int>("dueOnController", o.DueOnControllerId);
      o.DueOnRuleAppliesForAllContracts =
                  data.Get<string>("dueOnRuleAppliesForAllContracts", o.DueOnRuleAppliesForAllContracts);

      o.Duration = data.Get<string>("duration", o.Duration);
      o.DurationUnit = data.Get<string>("durationUnit", o.DurationUnit);
      o.Periodicity = data.Get<string>("periodicity", o.Periodicity);

      o.EntityId = data.Get<int>("entity", o.EntityId);
      o.ProcedureId = data.Get<int>("procedure", o.ProcedureId);

      o.ContractClause = data.Get<string>("contractClause", o.ContractClause);
      o.LegalBasis = data.Get<string>("legalBasis", o.LegalBasis);


      if (!data.HasValue("periodicityRule")) {
        o.PeriodicRule = PeriodicRuleData.Empty;
        o.PeriodicityRuleJsonData = JsonObject.Empty;
      } else {
        var periodicRuleJson = data.Slice("periodicityRule");
        o.PeriodicityRuleJsonData = periodicRuleJson;
        o.PeriodicRule = PeriodicRuleData.Parse(periodicRuleJson);
      }

      return o;
    }


    static public ActivityModel Empty {
      get {
        return new ActivityModel();
      }
    }


    #endregion Constructors and parsers

    #region Properties

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

    public PeriodicRuleData PeriodicRule {
      get;
      private set;
    } = PeriodicRuleData.Empty;


    public JsonObject PeriodicityRuleJsonData {
      get;
      private set;
    }


    public bool IsPeriodic {
      get {
        return this.ExecutionMode == "Periodic" && !this.PeriodicRule.IsEmptyInstance;
      }
    }


    [DataField("Entity")]
    public int EntityId {
      get;
      private set;
    } = -1;


    [DataField("Procedure")]
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


    #endregion Properties

  } // class ActivityModel

} // namespace Empiria.ProjectManagement
