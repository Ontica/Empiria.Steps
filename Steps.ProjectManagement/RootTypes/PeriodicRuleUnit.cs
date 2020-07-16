/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                            Component : Domain Layer                         *
*  Assembly : Empiria.ProjectManagement.dll                 Pattern   : Enumerated Type                      *
*  Type     : PeriodicRuleUnit                              License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Time units used to describe periodic activities.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement {

  /// <summary>Time units used to describe periodic activities.</summary>
  public enum PeriodicRuleUnit {

    CalendarDays,

    Weeks,

    Months,

    Years,

    Manual

  }

}
