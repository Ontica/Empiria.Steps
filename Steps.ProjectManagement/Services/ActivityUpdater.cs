/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Service Provider                      *
*  Type     : ActivityUpdater                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Updates project activities based on project model rules.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Updates project activities based on project model rules.</summary>
  internal class ActivityUpdater {

    #region Fields

    private WhatIfResult whatIfResult = null;

    #endregion Fields


    #region Constructors and parsers


    public ActivityUpdater() {
      // no-op
    }


    #endregion Constructors and parsers


    #region Public methods


    public WhatIfResult OnComplete(ProjectItem projectItem, DateTime completedDate) {
      Assertion.AssertObject(projectItem, "projectItem");

      this.whatIfResult = new WhatIfResult(projectItem, ProjectItemOperation.Complete);

      // Create root
      var stateChange = new ProjectItemStateChange(projectItem, ProjectItemOperation.Complete);

      stateChange.ActualEndDate = completedDate;

      whatIfResult.AddStateChange(stateChange);

      // Update related
      UpdateRelatedProjectItemsDeadlines(projectItem, completedDate);

      // Create next periodic
      if (projectItem.TemplateId != 0) {
        var template = projectItem.GetTemplate().Template;

        if (template.ExecutionMode == "Periodic" && !template.PeriodicRule.IsEmptyInstance) {

          var newPeriodic = new ProjectItemStateChange(projectItem.GetTemplate(), ProjectItemOperation.CreateFromTemplate);

          newPeriodic.Project = projectItem.Project;

          newPeriodic.Replaces = projectItem;

          newPeriodic.Deadline = UtilityMethods.CalculatePeriodicDate(template, projectItem.Deadline);

          whatIfResult.AddStateChange(newPeriodic);
        }
      }

      return this.whatIfResult;
    }


    #endregion Public methods


    #region Private methods


    private void AddUpdateDeadlineStateChange(ProjectItem projectItem,
                                              DateTime updatedDeadline) {
      var stateChange = new ProjectItemStateChange(projectItem, ProjectItemOperation.UpdateDeadline);

      stateChange.Deadline = updatedDeadline;

      whatIfResult.AddStateChange(stateChange);
    }


    private FixedList<Activity> GetModelDependencies(Activity activityModel) {
      var templateProject = activityModel.Project;

      List<Activity> dependencies = templateProject.GetItems()
                                                   .Select(x => (Activity) x)
                                                   .ToList();

      return dependencies.FindAll(x => x.Template.DueOnControllerId == activityModel.Id &&
                                       !this.whatIfResult.StateChanges.Exists(y => y.Template.Id == x.Id))
                         .ToFixedList();
    }


    private FixedList<ProjectItem> GetProjectDependencies(Activity model) {
      var project = this.whatIfResult.Source.Project;

      return project.GetItems().FindAll(x => x.TemplateId == model.Id);
    }


    private FixedList<ProjectItem> GetRelatedProjectItems(ProjectItem projectItem) {
      if (!projectItem.HasTemplate) {
        return new FixedList<ProjectItem>();
      }

      Activity template = projectItem.GetTemplate();

      FixedList<Activity> modelDependencies = GetModelDependencies(template);

      List<ProjectItem> list = new List<ProjectItem>();

      foreach (var model in modelDependencies) {
        FixedList<ProjectItem> projectItems = GetProjectDependencies(model);

        list.AddRange(projectItems);
      }

      return list.ToFixedList();
    }


    private void UpdateRelatedProjectItemsDeadlines(ProjectItem projectItem, DateTime completedDate) {
      FixedList<ProjectItem> relatedProjectItems = GetRelatedProjectItems(projectItem);

      foreach (var item in relatedProjectItems) {
        var template = ((Activity) item.GetTemplate()).Template;

        DateTime? updatedDeadline = UtilityMethods.CalculateNewDeadline(template, completedDate);

        if (updatedDeadline.HasValue) {
          AddUpdateDeadlineStateChange(item, updatedDeadline.Value);

          // Recursive call
          UpdateRelatedProjectItemsDeadlines(item, updatedDeadline.Value);
        }
      }
    }


    #endregion Private methods


  }  // class ActivityUpdater

}  // namespace Empiria.ProjectManagement.Services
