/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Information Holder                    *
*  Type     : ProjectItemStateChange                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Holds information about a project item operation and its associated data.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.StateEnums;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Holds information about a project item operation and its associated data.</summary>
  public class ProjectItemStateChange {

    #region Constructors and parsers


    internal ProjectItemStateChange(ProjectItem item, ProjectItemOperation operation) {
      this.ProjectItem = item;
      this.Operation = operation;
    }


    #endregion Constructors and parsers

    #region Properties


    public ProjectItem ProjectItem {
      get;
    }


    public ProjectItemOperation Operation {
      get;
    }


    public DateTime ActualStartDate {
      get;
      internal set;
    }


    public DateTime ActualEndDate {
      get;
      internal set;
    }


    public DateTime PlannedEndDate {
      get;
      internal set;
    }


    public DateTime Deadline {
      get;
      internal set;
    }


    public ActivityStatus Status {
      get;
      internal set;
    }


    #endregion Properties

  }  // class ProjectItemStateChange

}  // namespace Empiria.ProjectManagement.Services
