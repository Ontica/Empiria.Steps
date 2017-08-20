/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Duration                                         License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Data type with methods used to describe and control time duration.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps {

  /// <summary>Data type with methods used to describe and control time duration.</summary>
  public class Duration {

    #region Fields

    private string _string_value = String.Empty;

    #endregion Fields

    #region Constructors and parsers

    public Duration(int value, DurationType type) {
      _string_value = GetStringValue(value, type);

      this.Load();
    }

    private Duration(string value) {
      _string_value = EmpiriaString.TrimAll(value);

      this.Load();
    }

    static public Duration Parse(string value) {
      return new Duration(value);
    }

    static public Duration Empty {
      get {
        return new Duration(String.Empty);
      }
    }

    #endregion Constructors and parsers

    #region Properties

    public DurationType DurationType {
      get;
      private set;
    } = DurationType.Unknown;


    public int Value {
      get;
      private set;
    } = 0;

    #endregion Properties

    #region Methods

    private DurationType GetDurationType(string duration) {
      duration = duration.ToLowerInvariant();

      if (duration == "hours" || duration == "hour") {
        return DurationType.Hours;

      } else if (duration == "days" || duration == "day") {
        return DurationType.Days;

      } else if (duration == "work-days" || duration == "work-day" ||
                 duration == "working-days" || duration == "working-day") {
        return DurationType.WorkingDays;

      } else if (duration == "months" || duration == "month") {
        return DurationType.Months;

      } else if (duration == "years" || duration == "year") {
        return DurationType.Years;

      } else {
        throw Assertion.AssertNoReachThisCode();

      }
    }


    private string GetStringValue(int value, DurationType type) {
      switch (type) {
        case DurationType.Hours:
          return value + " hours";

        case DurationType.Days:
          return value + " days";

        case DurationType.WorkingDays:
          return value + " working-days";

        case DurationType.Months:
          return value + " months";

        case DurationType.Years:
          return value + " years";

        default:
          throw Assertion.AssertNoReachThisCode();
      }
    }


    private void Load() {
      string[] parts = _string_value.Split(' ');

      if (parts.Length == 2) {
        this.Value = int.Parse(parts[0]);
        this.DurationType = GetDurationType(parts[1]);
      }
    }


    public override string ToString() {
      return _string_value;
    }

    #endregion Methods

  } // class Duration

} // namespace Empiria.Steps
