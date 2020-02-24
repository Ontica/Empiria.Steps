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
        this.Project = Project.Empty;
      } else if (this.Operation == ProjectItemOperation.UpdateProcessChanges) {
        this.ProjectItem = item;
        this.Template = item.GetTemplate();
        this.Project = item.Project;
      } else {
        this.ProjectItem = item;
        this.Template = ProjectItem.Empty;
        this.Project = item.Project;
      }
    }


    #endregion Constructors and parsers

    #region Properties


    public string UID {
      get;
    } = Guid.NewGuid().ToString();


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


    public ProjectItemProcessMatchResult ProcessMatchResult {
      get;
      internal set;
    } = ProjectItemProcessMatchResult.Unknown;


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


    public int ItemLevel {
      get {
        if (this.Operation == ProjectItemOperation.UpdateProcessChanges) {
          return this.ProjectItem.Level;
        }

        if (this.Parent != null) {
          return this.Parent.ItemLevel + 1;
        } else {
          return 1;
        }
      }
    }


    public string Name {
      get;
      internal set;
    }


    public string Notes {
      get;
      internal set;
    }


    public ProjectItemStateChange Parent {
      get;
      internal set;
    }


    public ProjectItem Replaces {
      get;
      internal set;
    }


    public ActivityStatus Status {
      get;
      internal set;
    }


    public string Theme {
      get;
      internal set;
    }


    #endregion Properties

  }  // class ProjectItemStateChange

}  // namespace Empiria.ProjectManagement.Services
