﻿/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.dll                                Pattern : Enumeration                         *
*  Type     : ProjectItemStatus                                License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Represents the status of a project item.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Represents the status of a project item.</summary>
  public enum ProjectItemStatus {

    Undefined = 'U',

    Inactive = 'I',

    Active = 'A',

    Suspended = 'S',

    Completed = 'C',

    Deleted = 'X',

  }  // enum ProjectItemStatus

}  // namespace Empiria.Steps.ProjectManagement
