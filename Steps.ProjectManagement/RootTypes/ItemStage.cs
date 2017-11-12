/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.ProjectManagement.dll              Pattern : Enumeration                         *
*  Type     : ItemStage                                        License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Represents the stage of a project item.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Represents the stage of a project item.</summary>
  public enum ItemStage {

    Undefined = 'U',

    Backlog = 'B',

    Next = 'N',

    InProcess = 'P',

    Done = 'D',

    Awaiting = 'W',

    Blocked = 'K',

    Deleted = 'X',

  }  // enum ItemStage

}  // namespace Empiria.Steps.ProjectManagement
