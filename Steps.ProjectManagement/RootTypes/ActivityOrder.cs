/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.ProjectManagement.dll              Pattern : Enumeration                         *
*  Type     : ActivityOrder                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Enumerates activity ordering options.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Enumerates activity ordering options.</summary>
  public enum ActivityOrder {

    Default = 'U',

    DueDate = 'D',

    TargetDate = 'T',

    Responsible = 'R',

    ActivityName = 'N',

  }  // enum ActivityOrder

}  // namespace Empiria.Steps.ProjectManagement
