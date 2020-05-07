/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Service Provider                      *
*  Type     : UtilityMethods                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Static utility methods for project management services.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Runtime.Remoting.Lifetime;
using Empiria.DataTypes;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Static utility methods for project management services.</summary>
  internal class UtilityMethods {

    #region Internal methods

    static internal DateTime? CalculateNewDeadline(ActivityModel template, DateTime baseDate) {
      EmpiriaCalendar calendar = GetCalendarFor(template);

      DateTime? newDeadline = CalculateNewDeadlineImplement(template, baseDate, calendar);

      if (!newDeadline.HasValue) {
        return newDeadline;
      }

      return calendar.NextWorkingDate(newDeadline.Value, true);
    }



    static internal DateTime? CalculateNextPeriodicDate(ActivityModel template, DateTime baseDate) {
      EmpiriaCalendar calendar = GetCalendarFor(template);

      DateTime? newPeriodicDate = CalculateNextPeriodicDateImplement(template, baseDate, calendar);

      if (!newPeriodicDate.HasValue) {
        return null;
      }

      return calendar.NextWorkingDate(newPeriodicDate.Value, true);
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

      if (eventDate.Year > (DateTime.Today.Year + 10)) {
        return null;
      }

      switch (periodicRule.RuleType) {
        case "Daily":
          return eventDate.AddDays(1);

        case "OncePerYear-OnFixedDate":
          return GetNextYearDate(eventDate, periodicRule.Month, periodicRule.Day);

        case "Semi-annual-OnFixedDays":
          return GetNextSemiannualDate(eventDate, periodicRule.Month, periodicRule.Day);

        case "Semi-annual-BusinessDays":
          return GetNextSemiannualBusinessDate(calendar, eventDate, periodicRule.Month, periodicRule.Day);

        case "Monthly-OnFixedDay":
          return GetNextMonthDate(eventDate, periodicRule.Day);

        case "Monthly-BusinessDays":
          return UtilityMethods.GetNextMonthBusinessDate(calendar, eventDate, periodicRule.Day);

        case "After-Given-Activity-Yearly":
          return eventDate.AddYears(1);

        case "After-Given-Activity-Semi-annual":
          return eventDate.AddMonths(6);

        case "After-Given-Activity-Monthly":
          return eventDate.AddMonths(1);

        default:
          return null;
      }
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


    static private DateTime GetNextSemiannualDate(DateTime fromDate, int month, int monthDay) {
      int secondMonth = month > 6 ? month - 6 : month + 6;

      DateTime firstDate = new DateTime(fromDate.Year, Math.Min(month, secondMonth), monthDay);
      DateTime secondDate = new DateTime(fromDate.Year, Math.Max(month, secondMonth), monthDay);

      if (fromDate < firstDate) {
        return firstDate;

      } else if (fromDate < secondDate) {
        return secondDate;

      } else {

        return new DateTime(firstDate.Year + 1, firstDate.Month, monthDay);
      }
    }


    static private DateTime GetNextSemiannualBusinessDate(EmpiriaCalendar calendar,
                                                          DateTime fromDate, int month, int businessDays) {

      int secondMonth = month > 6 ? month - 6 : month + 6;

      DateTime firstDate = new DateTime(fromDate.Year, Math.Min(month, secondMonth), 1).AddDays(-1);
      firstDate = calendar.AddWorkingDays(firstDate, businessDays);

      DateTime secondDate = new DateTime(fromDate.Year, Math.Max(month, secondMonth), 1).AddDays(-1);
      secondDate = calendar.AddWorkingDays(secondDate, businessDays);


      if (fromDate < firstDate) {
        return firstDate;

      } else if (fromDate < secondDate) {
        return secondDate;

      } else {
        firstDate = new DateTime(fromDate.Year + 1, Math.Min(month, secondMonth), 1).AddDays(-1);
        firstDate = calendar.AddWorkingDays(firstDate, businessDays);

        return firstDate;
      }
    }


    static private DateTime GetNextYearDate(DateTime fromDate, int month, int monthDay) {
      DateTime date = fromDate.AddYears(1);

      return new DateTime(date.Year, month, monthDay);
    }


    #endregion Private methods

  }  // class UtilityMethods

}  // namespace Empiria.ProjectManagement.Services
