/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Enumeration                           *
*  Type     : ProjectItemOperation                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : List of operations that can be performed on a ProjectItem.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.Services {

  /// <summary>List of operations that can be performed on a ProjectItem.</summary>
  public enum ProjectItemOperation {

    CreateFromTemplate,

    Complete,

    Reactivate

  }  // enum ProjectItemOperation

}  // namespace Empiria.ProjectManagement.Services
