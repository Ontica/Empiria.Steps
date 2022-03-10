/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Enumeration                           *
*  Type     : ProjectItemProcessMatchResult                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : List of possible matching status between a ProjectItem and its associated process changes.     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.Services {

  /// <summary>List of possible matching status between a ProjectItem and its associated process changes.</summary>
  public enum ProjectItemProcessMatchResult {

    Unknown,


    MatchedEqual,


    MatchedWithDataChanges,


    MatchedWithDeadlineChanges,


    MatchedWithDeadlineAndDataChanges,


    DeletedFromProject,


    DeletedFromProcess,


    OnlyInProject,


    OnlyInProcess,


    OrphanInProject,


    NoProgrammingRule


  }  // ProjectItemProcessMatchResult

}  // namespace Empiria.ProjectManagement.Services
