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
      var o = new PeriodicRuleData();

      o.RuleType = data.Get<string>("ruleType");
      o.Month = data.Get<int>("month", o.Month);
      o.Day = data.Get<int>("day", o.Day);

      return o;
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


    public int Day {
      get;
      private set;
    }


    public int Month {
      get;
      private set;
    }


    public string RuleType {
      get;
      private set;
    } = String.Empty;


    #endregion Fields

  }  // class PeriodicRuleData

}  // namespace Empiria.ProjectManagement
