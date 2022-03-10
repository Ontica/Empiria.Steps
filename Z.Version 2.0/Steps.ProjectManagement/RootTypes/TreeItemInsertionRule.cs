/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                            Component : Domain Layer                         *
*  Assembly : Empiria.ProjectManagement.dll                 Pattern   : Enumerated Type                      *
*  Type     : TreeItemInsertionRule                         License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Enumerates the different kinds of insertion rules (or hints) for project tree items.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement {

  /// <summary>Enumerates the different kinds of insertion rules (or hints) for project tree items.</summary>
  public enum TreeItemInsertionRule {

    AsChildAsFirstNode,

    AsChildAsLastNode,

    AsChildAtPosition,

    AsSiblingAfterInsertionPoint,

    AsSiblingBeforeInsertionPoint,

    AsTreeRootAtEnd,

    AsTreeRootAtStart,

    AtRelativePosition

  }  // enum TreeItemInsertionRule

} // namespace Empiria.ProjectManagement
