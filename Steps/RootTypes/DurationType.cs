/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Enumeration                         *
*  Type     : DurationType                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes a duration type.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps {

  /// <summary>Describes a duration type.</summary>
  public enum DurationType {

    Unknown = -1,

    Hours = 1,

    Days = 2,

    WorkingDays = 3,

    Months = 4,

    Years = 5,

  }  // enum DurationType

}  // namespace Empiria.Steps
