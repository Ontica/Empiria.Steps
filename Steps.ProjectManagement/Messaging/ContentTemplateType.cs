/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Messaging services                           Component : Presentation Layer                    *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Enumeration type                      *
*  Type     : ContentTemplateType                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Enumerates the templates used to generate messages content.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.Messaging {

  /// <summary>Enumerates the templates used to generate messages content.</summary>
  internal enum ContentTemplateType {

    ActivityTableRowWithAssignee,

    ActivityTableRowWithoutAssignee,

    AllPendingActivitiesSummary,

    ByThemeSummaryBody,

    ByThemeSummaryTableRow,

    YourPendingActivitiesSummary,

  }  // enum ContentTemplateType

}  // namespace Empiria.ProjectManagement.Messaging
