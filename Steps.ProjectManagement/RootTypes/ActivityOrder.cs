/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Enumeration                         *
*  Type     : ActivityOrder                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Enumerates activity ordering options.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement {

  /// <summary>Enumerates activity ordering options.</summary>
  public enum ActivityOrder {

    Default = 'U',

    Deadline = 'D',

    PlannedEndDate = 'P',

    Responsible = 'R',

    ActivityName = 'N',

  }  // enum ActivityOrder

}  // namespace Empiria.ProjectManagement
