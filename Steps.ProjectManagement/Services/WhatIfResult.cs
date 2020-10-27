/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Information Holder                    *
*  Type     : WhatIfResult                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Contains information about all project items which will be changed as a consequence            *
*             of performing an update operation in a given project item operation known as source.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using System.Collections.Generic;
using System.Linq;


namespace Empiria.ProjectManagement.Services {

  /// <summary>Contains information about all project items which will be changed as a consequence
  /// of performing an update operation in a given project item operation known as source.</summary>
  public class WhatIfResult {

    #region Fields


    private List<ProjectItemStateChange> stateChanges = new List<ProjectItemStateChange>();
    private List<Exception> exceptions = new List<Exception>();


    #endregion Fields


    #region Constructors and parsers


    internal WhatIfResult(ProjectItem source, ProjectItemOperation sourceOperation) {
      this.Source = source;
      this.SourceOperation = sourceOperation;
    }


    #endregion Constructors and parsers


    #region Properties


    public ProjectItem Source {
      get;
    }


    public ProjectItemOperation SourceOperation {
      get;
    }


    public bool HasErrors {
      get {
        return (this.exceptions.Count != 0);
      }
    }


    public FixedList<ProjectItemStateChange> StateChanges {
      get {
        return this.stateChanges.ToFixedList();
      }
    }


    #endregion Properties


    #region Methods


    internal void AddException(Exception exception) {
      this.exceptions.Add(exception);
    }


    internal void AddStateChange(ProjectItemStateChange stateChange) {
      this.stateChanges.Add(stateChange);
    }


    internal void AddStateChanges(IList<ProjectItemStateChange> stateChangesList) {
      this.stateChanges.AddRange(stateChangesList);
    }


    public Exception GetException() {
      return new Exception("This is the exception in WhatIfResult");
    }


    private FixedList<ProjectItem> GetProjectDependencies(ProjectItem projectItem, Activity model) {
      var project = this.Source.Project;

     // return project.GetItems().FindAll(x => x.TemplateId == model.Id);

      return project.GetItems().FindAll(x => x.TemplateId == model.Id &&
                                             x.ProcessID == projectItem.ProcessID &&
                                             x.SubprocessID == projectItem.SubprocessID);
    }


    internal FixedList<ProjectItemStateChange> GetRelatedStateChanges(ProjectItemStateChange stateChange) {
      if (stateChange.Template == null) {
        return new FixedList<ProjectItemStateChange>();
      }

      Activity template = (Activity) stateChange.Template;

      FixedList<Activity> modelDependencies = this.GetContainedModelDependencies(template);

      List<ProjectItemStateChange> list = new List<ProjectItemStateChange>();

      foreach (var model in modelDependencies) {
        FixedList<ProjectItemStateChange> projectItems = this.StateChanges.FindAll(x => x.Template.Id == model.Id);

        list.AddRange(projectItems);
      }

      return list.ToFixedList();
    }


    internal FixedList<Activity> GetContainedModelDependencies(Activity activityModel) {
      if (activityModel.IsEmptyInstance) {
        return new FixedList<Activity>();
      }

      var templateProject = activityModel.Project;

      try {
        List<Activity> dependencies = templateProject.GetItems()
                                                     .Select(x => (Activity) x)
                                                     .ToList();

        return dependencies.FindAll(x => x.Template.DueOnControllerId == activityModel.Id &&
                                         this.StateChanges.Exists(y => y.Template.Id == x.Id))
                           .ToFixedList();
      } catch(Exception e) {
        var ex = new Exception($"GetContainedModelDependencies issue for {activityModel.Id}.", e);

        EmpiriaLog.Error(ex);

        throw ex;
      }
    }


    internal FixedList<Activity> GetUncontainedModelDependencies(Activity activityModel) {
      var templateProject = activityModel.Project;

      List<Activity> dependencies = templateProject.GetItems()
                                                   .Select(x => (Activity) x)
                                                   .ToList();

      return dependencies.FindAll(x => x.Template.DueOnControllerId == activityModel.Id &&
                                       !this.StateChanges.Exists(y => y.Template.Id == x.Id))
                         .ToFixedList();
    }


    internal FixedList<ProjectItem> GetUncontainedRelatedProjectItems(ProjectItem projectItem) {
      if (!projectItem.HasTemplate) {
        return new FixedList<ProjectItem>();
      }

      Activity template = projectItem.GetTemplate();

      FixedList<Activity> modelDependencies = this.GetUncontainedModelDependencies(template);

      List<ProjectItem> list = new List<ProjectItem>();

      foreach (var model in modelDependencies) {
        FixedList<ProjectItem> projectItems = GetProjectDependencies(projectItem, model);

        list.AddRange(projectItems);
      }

      return list.ToFixedList();
    }


    internal void InsertStateChange(int index, ProjectItemStateChange stateChange) {
      this.stateChanges.Insert(index, stateChange);
    }


    internal void InsertStateChange(int index, IList<ProjectItemStateChange> stateChangesList) {
      this.stateChanges.InsertRange(index, stateChangesList);
    }


    #endregion Methods

  }  // class
}  // namespace Empiria.ProjectManagement.Services
