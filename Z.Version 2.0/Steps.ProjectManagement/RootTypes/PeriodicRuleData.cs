/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : DTO                                 *
*  Type     : PeriodicRuleData                                 License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Holds configuration data ued to calculate periodic activities.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.ProjectManagement {

  /// <summary>Holds configuration data ued to calculate periodic activities.</summary>
  public class PeriodicRuleData {

    #region Constructors and parsers


    static internal PeriodicRuleData Parse(JsonObject data) {
      if (data.HasValue("ruleType")) {
        return ParseFromOldModel(data);
      }

      return ParseNewDataModel(data);
    }


    static private PeriodicRuleData ParseNewDataModel(JsonObject data) {
      var o = new PeriodicRuleData();

      o.EachUnit = data.Get<PeriodicRuleUnit>("each/unit");

      if (data.HasValue("each/value")) {
        o.EachValue = data.Get<int>("each/value");
      }

      if (data.HasValue("notes")) {
        o.Notes = data.Get<string>("notes");
      }

      if (!data.HasValue("dueOn")) {
        return o;
      }

      var dueOnJson = data.Slice("dueOn");

      o.DueOnType = dueOnJson.Get<PeriodicRuleDueOn>("type");

      if (dueOnJson.HasValue("month")) {
        o.Month = dueOnJson.Get<int>("month");
      }

      if (dueOnJson.HasValue("day")) {
        o.Day = dueOnJson.Get<int>("day");
      }

      if (dueOnJson.HasValue("dayOfWeek")) {
        o.DayOfWeek = dueOnJson.Get<int>("dayOfWeek");
      }

      return o;
    }


    static private PeriodicRuleData ParseFromOldModel(JsonObject data) {
      var ruleType = data.Get<string>("ruleType");

      switch (ruleType) {

        case "Daily":
          return new PeriodicRuleData() {
            EachUnit = PeriodicRuleUnit.CalendarDays,
            EachValue = 1
          };

        case "OncePerYear-OnFixedDate":
          return new PeriodicRuleData() {
            EachUnit = PeriodicRuleUnit.Years,
            EachValue = 1,

            DueOnType = PeriodicRuleDueOn.OnFixedDate,
            Month = data.Get<int>("month"),
            Day = data.Get<int>("day")
          };

        case "Semi-annual-OnFixedDays":
          return new PeriodicRuleData() {
            EachUnit = PeriodicRuleUnit.Months,
            EachValue = 6,

            DueOnType = PeriodicRuleDueOn.OnFixedDate,
            Month = data.Get<int>("month"),
            Day = data.Get<int>("day"),
          };

        case "Semi-annual-BusinessDays":
          return new PeriodicRuleData() {
            EachUnit = PeriodicRuleUnit.Months,
            EachValue = 6,

            DueOnType = PeriodicRuleDueOn.OnFirstBusinessDays,
            Month = data.Get<int>("month"),
            Day = data.Get<int>("day"),
          };

        case "Monthly-OnFixedDay":
          return new PeriodicRuleData() {
            EachUnit = PeriodicRuleUnit.Months,
            EachValue = 1,

            DueOnType = PeriodicRuleDueOn.OnFirstCalendarDays,
            Day = data.Get<int>("day")
          };

        case "Monthly-BusinessDays":
          return new PeriodicRuleData() {
            EachUnit = PeriodicRuleUnit.Months,
            EachValue = 1,

            DueOnType = PeriodicRuleDueOn.OnFirstBusinessDays,
            Day = data.Get<int>("day")
          };

        case "After-Given-Activity-Yearly":
          return new PeriodicRuleData() {
            EachUnit = PeriodicRuleUnit.Years,
            EachValue = 1,

            DueOnType = PeriodicRuleDueOn.AfterTheGivenStep,
          };

        case "After-Given-Activity-Semi-annual":
          return new PeriodicRuleData() {
            EachUnit = PeriodicRuleUnit.Months,
            EachValue = 6,

            DueOnType = PeriodicRuleDueOn.AfterTheGivenStep
          };

        case "After-Given-Activity-Monthly":
          return new PeriodicRuleData() {
            EachUnit = PeriodicRuleUnit.Months,
            EachValue = 1,

            DueOnType = PeriodicRuleDueOn.AfterTheGivenStep
          };

        case "Manual":
          return new PeriodicRuleData() {
            EachUnit = PeriodicRuleUnit.Manual
          };

        default:
          throw Assertion.EnsureNoReachThisCode($"Invalid periodic rule type '{ruleType}'." +
                                                $"Json data: {data.ToString()}.");
      }
    }


    static public PeriodicRuleData Empty {
      get {
        var empty = new PeriodicRuleData();

        empty.IsEmptyInstance = true;

        return empty;
      }
    }


    #endregion Constructors and parsers


    #region Fields


    public bool IsEmptyInstance {
      get;
      private set;
    } = false;


    public PeriodicRuleUnit EachUnit {
      get;
      private set;
    }


    public int? EachValue {
      get;
      private set;
    }


    public PeriodicRuleDueOn? DueOnType {
      get;
      private set;
    }


    public int? Day {
      get;
      private set;
    }


    public int? Month {
      get;
      private set;
    }


    public int? DayOfWeek {
      get;
      private set;
    }


    public string Notes {
      get;
      internal set;
    }


    #endregion Fields

    #region Methods

    private JsonObject GetJsonForDueOnPart() {
      var dueOnJson = new JsonObject();

      dueOnJson.Add("type", this.DueOnType.ToString());

      if (this.Month.HasValue) {
        dueOnJson.Add("month", this.Month.Value);
      }

      if (this.Day.HasValue) {
        dueOnJson.Add("day", this.Day.Value);
      }

      if (this.DayOfWeek.HasValue) {
        dueOnJson.Add("dayOfWeek", this.DayOfWeek.Value);
      }

      return dueOnJson;
    }


    private JsonObject GetJsonForEachPart() {
      var each = new JsonObject();

      each.Add("unit", this.EachUnit.ToString());

      if (this.EachValue.HasValue) {
        each.Add("value", this.EachValue.Value);
      }

      return each;
    }


    public JsonObject ToJson() {
      var json = new JsonObject();

      json.Add("each", this.GetJsonForEachPart());

      if (this.DueOnType.HasValue) {
        json.Add("dueOn", this.GetJsonForDueOnPart());
      }

      json.AddIfValue("notes", this.Notes);

      return json;
    }


    public override string ToString() {
      return this.ToJson().ToString();
    }


    #endregion Methods

  }  // class PeriodicRuleData

}  // namespace Empiria.ProjectManagement
