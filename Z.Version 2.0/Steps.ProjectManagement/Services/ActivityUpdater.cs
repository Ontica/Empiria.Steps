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
      Assertion.Require(projectItem, "projectItem");

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

          if (IsSpecialDeadlineCase(item.GetTemplate())) {
            int daysToAdd = GetSpecialDeadlineCaseDaysToAdd(item);

            updatedDeadline = UtilityMethods.AddWorkingDays(template, updatedDeadline.Value, daysToAdd);
          }


          AddUpdateDeadlineStateChange(item, updatedDeadline.Value);

          // Recursive call
          UpdateRelatedProjectItemsDeadlines(item, updatedDeadline.Value);
        }
      }
    }


    private bool IsSpecialDeadlineCase(Activity template) {
      return (template.Id == 347127 || template.Id == 374969);
    }


    private int GetSpecialDeadlineCaseDaysToAdd(ProjectItem item) {
      ProjectItem adder1 = SeekRelatedParent(item, 374966);
      ProjectItem adder2 = SeekRelatedParent(item, 347125);


      return GetDaysToAdd(adder1) + GetDaysToAdd(adder2);
    }


    private ProjectItem SeekRelatedParent(ProjectItem item, int seekForTemplateId) {
      var parent = item.Parent;

      while (true) {
        if (parent.IsEmptyInstance) {
          return ProjectItem.Empty;
        }

        var branch = parent.GetBranch();

        var relatedParent = branch.Find((x) => x.TemplateId == seekForTemplateId);

        if (relatedParent != null) {
          return relatedParent;
        }

        parent = parent.Parent;
      }
    }


    private int GetDaysToAdd(ProjectItem adder) {
      if (adder.IsEmptyInstance) {
        return 0;
      }
      if (adder.Status != StateEnums.ActivityStatus.Completed) {
        return 0;
      }

      return adder.Deadline.Subtract(adder.ActualEndDate).Days;
    }

    #endregion Private methods


  }  // class ActivityUpdater

}  // namespace Empiria.ProjectManagement.Services
