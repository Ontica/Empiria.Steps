﻿/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Service Provider                      *
*  Type     : UtilityMethods                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Static utility methods for project management services.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.DataTypes.Time;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Static utility methods for project management services.</summary>
  static internal class UtilityMethods {

    #region Internal methods


    static internal DateTime AddWorkingDays(ActivityModel template, DateTime baseDate, int workingDays) {
      EmpiriaCalendar calendar = GetCalendarFor(template);

      return calendar.AddWorkingDays(baseDate, workingDays);
    }


    static internal DateTime? CalculateNewDeadline(ActivityModel template, DateTime baseDate) {
      EmpiriaCalendar calendar = GetCalendarFor(template);

      DateTime? newDeadline = CalculateNewDeadlineImplement(template, baseDate, calendar);

      if (!newDeadline.HasValue) {
        return newDeadline;
      }

      if (calendar.IsWeekendDay(newDeadline.Value) || calendar.IsHoliday(newDeadline.Value)) {
        return calendar.LastWorkingDate(newDeadline.Value, true);

      } else {
        return calendar.NextWorkingDate(newDeadline.Value, true);
      }
    }



    static internal DateTime? CalculateNextPeriodicDate(ActivityModel template, DateTime baseDate) {
      EmpiriaCalendar calendar = GetCalendarFor(template);

      DateTime? newPeriodicDate = CalculateNextPeriodicDateImplement(template, baseDate, calendar);

      if (!newPeriodicDate.HasValue) {
        return null;
      }

      if (calendar.IsWeekendDay(newPeriodicDate.Value) ||  calendar.IsHoliday(newPeriodicDate.Value)) {
        return calendar.LastWorkingDate(newPeriodicDate.Value, true);

      } else {
        return calendar.NextWorkingDate(newPeriodicDate.Value, true);

      }
    }


    #endregion Internal methods


    #region Private methods

    static private int AdjustTermOnDueOnCondition(ActivityModel template, int term) {
      switch (template.DueOnCondition) {

        case "Before":
          return -1 * term;

        case "BeforeStart":
          return -1 * term;

        case "AfterStart":
          return term;

        case "During":
          return term;

        case "BeforeFinish":
          return -1 * term;

        case "AfterFinish":
          return term;

        case "After":
          return term;

        case "From":
          return term;

        default:
          return term;

      } // switch
    }


    static private DateTime? CalculateNewDeadlineImplement(ActivityModel template, DateTime baseDate,
                                                           EmpiriaCalendar calendar) {

      if (baseDate.Year > (DateTime.Today.Year + 10)) {
        return null;
      }

      var dueOnTerm = template.DueOnTerm;

      if (String.IsNullOrWhiteSpace(dueOnTerm)) {
        return null;
      }

      var term = int.Parse(dueOnTerm);

      term = AdjustTermOnDueOnCondition(template, term);

      switch (template.DueOnTermUnit) {

        case "BusinessDays":
          if (term >= 0) {
            return calendar.AddWorkingDays(baseDate, term);
          } else {
            return calendar.SubstractWorkingDays(baseDate, -1 * term);
          }

        case "CalendarDays":
          return baseDate.AddDays(term);

        case "Hours":
          return baseDate.AddHours(term);

        case "Months":
          return baseDate.AddMonths(term);

        case "Years":
          return baseDate.AddYears(term);

        default:
          return null;
      }
    }


    static private DateTime? CalculateNextPeriodicDateImplement(ActivityModel template, DateTime eventDate,
                                                                EmpiriaCalendar calendar) {
      var periodicRule = template.PeriodicRule;

      try {

        if (eventDate.Year > (DateTime.Today.Year + 20)) {
          return null;
        }

        switch (periodicRule.EachUnit) {

          case PeriodicRuleUnit.CalendarDays:
            return GetNextCalendarDay(eventDate, periodicRule.EachValue.Value);

          case PeriodicRuleUnit.Weeks:
            var nextWeekDate = GetNextCalendarDay(eventDate, periodicRule.EachValue.Value * 7);

            if (periodicRule.DueOnType == PeriodicRuleDueOn.OnFixedDayOfWeek) {
              return GetNextDayOfWeekDate(nextWeekDate, periodicRule.DayOfWeek.Value);

            } else if (periodicRule.DueOnType == PeriodicRuleDueOn.AfterTheGivenStep) {
              return nextWeekDate;

            } else {
              return null;
            }

          case PeriodicRuleUnit.Months:

            if (periodicRule.DueOnType == PeriodicRuleDueOn.AfterTheGivenStep) {
              return eventDate.AddMonths(periodicRule.EachValue.Value);

            } else if (periodicRule.DueOnType == PeriodicRuleDueOn.OnFirstBusinessDays) {
              return GetNextMonthBusinessDate(calendar, eventDate,
                                              periodicRule.EachValue.Value,
                                              periodicRule.Day.Value);

            } else if (periodicRule.DueOnType == PeriodicRuleDueOn.OnFirstCalendarDays) {
              return GetNextMonthDate(eventDate,
                                      periodicRule.EachValue.Value,
                                      periodicRule.Day.Value);

            } else if (periodicRule.DueOnType == PeriodicRuleDueOn.AfterFixedPeriodOnFixedDate) {
              return GetNextMonthBracketDate(eventDate, periodicRule.EachValue.Value,
                                             periodicRule.Month.Value, periodicRule.Day.Value);

            } else if (periodicRule.DueOnType == PeriodicRuleDueOn.AfterFixedPeriodOnBusinessDate) {
              return GetNextMonthBracketBusinessDate(calendar, eventDate,
                                                     periodicRule.EachValue.Value,
                                                     periodicRule.Month.Value, periodicRule.Day.Value);

            } else if (periodicRule.DueOnType == PeriodicRuleDueOn.AfterAdjustablePeriodOnFixedDate) {
              return GetNextMonthBracketDate(eventDate, periodicRule.EachValue.Value,
                                             GetAdjustedPeriodMonth(eventDate), periodicRule.Day.Value);

            } else if (periodicRule.DueOnType == PeriodicRuleDueOn.AfterAdjustablePeriodOnBusinessDate) {
              return GetNextMonthBracketBusinessDate(calendar, eventDate,
                                                     periodicRule.EachValue.Value,
                                                     GetAdjustedPeriodMonth(eventDate), periodicRule.Day.Value);

            } else {
              return null;
            }

          case PeriodicRuleUnit.Years:
            if (periodicRule.DueOnType == PeriodicRuleDueOn.AfterTheGivenStep) {
              return eventDate.AddYears(periodicRule.EachValue.Value);

            } else if (periodicRule.DueOnType == PeriodicRuleDueOn.OnFirstBusinessDays) {
              return GetNextYearBusinessDate(calendar, eventDate,
                                             periodicRule.EachValue.Value,
                                             periodicRule.Day.Value);

            } else if (periodicRule.DueOnType == PeriodicRuleDueOn.OnFirstCalendarDays) {
              return GetNextYearCalendarDate(eventDate,
                                             periodicRule.EachValue.Value,
                                             periodicRule.Day.Value);

            } else if (periodicRule.DueOnType == PeriodicRuleDueOn.OnFixedDate) {
              return GetNextYearFixedDate(eventDate, periodicRule.EachValue.Value,
                                          periodicRule.Month.Value, periodicRule.Day.Value);

            } else {
              return null;
            }

          case PeriodicRuleUnit.Manual:
            return null;

          default:
            return null;
        }

      } catch (Exception e) {
        throw new NotImplementedException($"Unable to calculate periodic rule for template rule: {periodicRule}", e);
      }
    }


    static private int GetAdjustedPeriodMonth(DateTime eventDate) {
      int month = eventDate.Month;

      if (month > 1) {
        return month - 1;
      } else {
        return 12;
      }
    }


    static private DateTime GetNextDayOfWeekDate(DateTime date, int dayOfWeek) {
      return date.AddDays(-1 * (int) date.DayOfWeek).AddDays(dayOfWeek);
    }


    static private EmpiriaCalendar GetCalendarFor(ActivityModel template) {
      if (template.EntityId == -1) {
        return EmpiriaCalendar.Default;
      }

      var org = Contacts.Organization.Parse(template.EntityId);

      var calendarName = org.Nickname;

      if (calendarName == "ASEA") {
        return GetASEACalendar(template);
      }

      return EmpiriaCalendar.ParseOrDefault(calendarName);
    }


    static private EmpiriaCalendar GetASEACalendar(ActivityModel template) {
      int[] AseaUsiviCalendarProcedures = { 181, 182, 183, 184, 185, 186, 187, 188, 189, 190 };

      if (EmpiriaMath.IsMemberOf(template.ProcedureId, AseaUsiviCalendarProcedures)) {
        return EmpiriaCalendar.ParseOrDefault("ASEA-USIVI");
      } else {
        return EmpiriaCalendar.ParseOrDefault("ASEA-UGI");
      }

    }


    static private DateTime GetNextCalendarDay(DateTime eventDate, int days) {
      return eventDate.AddDays(days);
    }


    static private DateTime GetNextMonthBusinessDate(EmpiriaCalendar calendar,
                                                     DateTime fromDate, int monthsCount, int businessDays) {
      DateTime date = fromDate.AddMonths(monthsCount - 1);

      DateTime date2 = new DateTime(date.Year, date.Month,
                                   DateTime.DaysInMonth(date.Year, date.Month));

      DateTime withWorkingDaysAdded = calendar.AddWorkingDays(date2, businessDays);

      DateTime withinMonthDate = date2.AddDays(1);

      if (withWorkingDaysAdded.Month == withinMonthDate.Month) {
        return withWorkingDaysAdded;
      } else {
        return calendar.LastWorkingDateWithinMonth(withinMonthDate.Year, withinMonthDate.Month);
      }
    }


    static private DateTime GetNextMonthDate(DateTime fromDate, int monthsCount, int monthDay) {
      DateTime date = fromDate.AddMonths(monthsCount);

      return new DateTime(date.Year, date.Month, monthDay);
    }


    static private DateTime GetNextMonthBracketDate(DateTime fromDate, int bracketMonthsSize,
                                                    int bracketControlMonth, int bracketMonthDueDay) {
      var bracketsBuilder = new MonthBracketsBuilder(bracketMonthsSize, bracketControlMonth);

      MonthBracket bracket = bracketsBuilder.GetBracketFor(fromDate);

      int year = fromDate.Year;

      if (fromDate.Month > bracket.DueMonth) {
        year = year + 1;
      }

      return new DateTime(year, bracket.DueMonth, bracketMonthDueDay);
    }


    static private DateTime GetNextMonthBracketBusinessDate(EmpiriaCalendar calendar,
                                                            DateTime fromDate, int bracketMonthsSize,
                                                            int bracketControlMonth, int businessDays) {
      var bracketsBuilder = new MonthBracketsBuilder(bracketMonthsSize, bracketControlMonth);

      MonthBracket bracket = bracketsBuilder.GetBracketFor(fromDate);

      int year = fromDate.Year;

      if (fromDate.Month > bracket.DueMonth) {
        year = year + 1;
      }

      DateTime date = new DateTime(year, bracket.DueMonth, 1).AddDays(-1);

      DateTime withWorkingDaysAdded = calendar.AddWorkingDays(date, businessDays);

      if (withWorkingDaysAdded.Month == bracket.DueMonth) {
        return withWorkingDaysAdded;
      } else {
        return calendar.LastWorkingDateWithinMonth(year, bracket.DueMonth);
      }
    }


    static private DateTime GetNextYearBusinessDate(EmpiriaCalendar calendar,
                                                    DateTime fromDate, int yearsCount, int businessDays) {

      DateTime date = new DateTime(fromDate.Year + yearsCount - 1, 12, 31);

      return calendar.AddWorkingDays(date, businessDays);
    }


    static private DateTime GetNextYearCalendarDate(DateTime fromDate, int yearsCount, int days) {
      DateTime date = new DateTime(fromDate.Year + yearsCount - 1, 12, 31);

      return date.AddDays(days);
    }


    static private DateTime GetNextYearFixedDate(DateTime fromDate, int yearsCount,
                                                 int month, int day) {
      return new DateTime(fromDate.Year + yearsCount, month, day);
    }


    #endregion Private methods

  }  // class UtilityMethods

}  // namespace Empiria.ProjectManagement.Services
