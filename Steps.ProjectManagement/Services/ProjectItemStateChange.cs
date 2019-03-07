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
      this.Operation = operation;

      if (this.Operation == ProjectItemOperation.CreateFromTemplate) {
        this.ProjectItem = ProjectItem.Empty;
        this.Template = item;
      } else {
        this.ProjectItem = item;
        this.Template = ProjectItem.Empty;
      }
    }


    #endregion Constructors and parsers

    #region Properties


    public Project Project {
      get;
      internal set;
    }


    public ProjectItem ProjectItem {
      get;
      internal set;
    }


    public ProjectItem Template {
      get;
    }


    public ProjectItemOperation Operation {
      get;
    }


    public DateTime ActualStartDate {
      get;
      internal set;
    } = ExecutionServer.DateMaxValue;


    public DateTime ActualEndDate {
      get;
      internal set;
    } = ExecutionServer.DateMaxValue;


    public DateTime PlannedEndDate {
      get;
      internal set;
    } = ExecutionServer.DateMaxValue;


    public DateTime Deadline {
      get;
      internal set;
    } = ExecutionServer.DateMaxValue;


    public ProjectItemStateChange Parent {
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
