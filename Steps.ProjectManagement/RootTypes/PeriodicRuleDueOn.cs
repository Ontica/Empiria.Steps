/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                            Component : Domain Layer                         *
*  Assembly : Empiria.ProjectManagement.dll                 Pattern   : Enumerated Type                      *
*  Type     : PeriodicRuleDueOn                             License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Enumerates the due on rule for periodic activities.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement {

  /// <summary>Enumerates the due on rule for periodic activities.</summary>
  public enum PeriodicRuleDueOn {

    AfterTheGivenStep,

    OnFixedDate,

    OnFixedDayOfWeek,

    OnFirstCalendarDays,

    OnFirstBusinessDays

  }

}
