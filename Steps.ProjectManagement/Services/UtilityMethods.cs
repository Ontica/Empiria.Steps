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

using Empiria.DataTypes;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Static utility methods for project management services.</summary>
  internal class UtilityMethods {

    #region Internal methods

    static internal DateTime? CalculateNewDeadline(ActivityModel template, DateTime baseDate) {
      var dueOnTerm = template.DueOnTerm;

      if (String.IsNullOrWhiteSpace(dueOnTerm)) {
        return null;
      }

      var term = int.Parse(dueOnTerm);

      term = AdjustTermOnDueOnCondtion(template, term);

      switch (template.DueOnTermUnit) {
        case "BusinessDays":
          EmpiriaCalendar calendar = GetCalendarFor(template);

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


    static internal DateTime CalculateNextPeriodicDate(ActivityModel template, DateTime eventDate) {
      var periodicRule = template.PeriodicRule;

      switch (periodicRule.RuleType) {
        case "Daily":
          return eventDate.AddDays(1);

        case "OncePerYear-OnFixedDate":
          return GetNextYearDate(eventDate, periodicRule.Month, periodicRule.Day);

        case "Monthly-OnFixedDay":
          return GetNextMonthDate(eventDate, periodicRule.Day);

        case "Monthly-BusinessDays":
          var calendar = GetCalendarFor(template);

          return UtilityMethods.GetNextMonthBusinessDate(calendar, eventDate, periodicRule.Day);

        default:
          return ExecutionServer.DateMaxValue;
      }
    }

    #endregion Internal methods


    #region Private methods

    static private int AdjustTermOnDueOnCondtion(ActivityModel template, int term) {
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


    static private EmpiriaCalendar GetCalendarFor(ActivityModel template) {
      if (template.EntityId != -1) {
        var org = Contacts.Organization.Parse(template.EntityId);

        return EmpiriaCalendar.Parse(org.Nickname);
      } else {
        return EmpiriaCalendar.Default;
      }
    }


    static private DateTime GetNextMonthBusinessDate(EmpiriaCalendar calendar,
                                                     DateTime fromDate, int businessDays) {
      DateTime date = new DateTime(fromDate.Year, fromDate.Month,
                                   DateTime.DaysInMonth(fromDate.Year, fromDate.Month));

      return calendar.AddWorkingDays(date, businessDays);


    }


    static private DateTime GetNextMonthDate(DateTime fromDate, int monthDay) {
      DateTime date = fromDate.AddMonths(1);

      return new DateTime(date.Year, date.Month, monthDay);
    }


    static private DateTime GetNextYearDate(DateTime fromDate, int month, int monthDay) {
      DateTime date = fromDate.AddYears(1);

      return new DateTime(date.Year, month, monthDay);
    }


    #endregion Private methods

  }  // class UtilityMethods

}  // namespace Empiria.ProjectManagement.Services
