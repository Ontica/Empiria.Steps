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


    public WhatIfResult OnComplete(ProjectItem projectItem, DateTime completedDate, bool addNewPeriodics) {
      Assertion.AssertObject(projectItem, "projectItem");

      this.whatIfResult = new WhatIfResult(projectItem, ProjectItemOperation.Complete);

      // Add root state change
      var stateChange = new ProjectItemStateChange(projectItem, ProjectItemOperation.Complete) {
        ActualEndDate = completedDate
      };

      whatIfResult.AddStateChange(stateChange);

      // Update related
      UpdateRelatedProjectItemsDeadlines(projectItem, completedDate);


      if (addNewPeriodics) {
        var newPeriodicResult = ModelingServices.TryGetNextPeriodic(projectItem, completedDate);

        if (newPeriodicResult != null) {
          this.whatIfResult.AddStateChanges(newPeriodicResult.StateChanges);
        }
      }

      return this.whatIfResult;
    }


    #endregion Public methods


    #region Private methods


    private void AddUpdateDeadlineStateChange(ProjectItem projectItem,
                                              DateTime updatedDeadline) {
      var stateChange = new ProjectItemStateChange(projectItem, ProjectItemOperation.UpdateDeadline) {
        Deadline = updatedDeadline
      };

      this.whatIfResult.AddStateChange(stateChange);
    }


    private void UpdateRelatedProjectItemsDeadlines(ProjectItem projectItem, DateTime completedDate) {
      FixedList<ProjectItem> relatedProjectItems = this.whatIfResult.GetUncontainedRelatedProjectItems(projectItem);

      foreach (var item in relatedProjectItems) {
        var template = ((Activity) item.GetTemplate()).Template;

        DateTime? updatedDeadline;

        if (template.IsPeriodic) {
          updatedDeadline = UtilityMethods.CalculateNextPeriodicDate(template, completedDate);

        } else {
          updatedDeadline = UtilityMethods.CalculateNewDeadline(template, completedDate);
        }

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
