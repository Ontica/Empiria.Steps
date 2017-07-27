/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Enumeration                         *
*  Type     : TermTimeUnit                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Represents a time unit used to describe activity due terms.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps {

  /// <summary>Represents a time unit used to describe activity due terms.</summary>
  public enum TermTimeUnit {

    Undefined = -1,

    NA = -2,

    CalendarDays = 1,

    BusinessDays = 2,

    Hours = 10,

  }  // enum TermTimeUnit

}  // namespace Empiria.Steps
