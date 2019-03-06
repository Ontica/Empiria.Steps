/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Enumeration                           *
*  Type     : ActivityOrderBy                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Enumerates activity ordering options.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Enumerates activity ordering options.</summary>
  public enum ActivityOrderBy {

    Default = 'U',

    Deadline = 'D',

    PlannedEndDate = 'P',

    Responsible = 'R',

    ActivityName = 'N',

  }  // enum ActivityOrderBy

}  // namespace Empiria.ProjectManagement.Services
