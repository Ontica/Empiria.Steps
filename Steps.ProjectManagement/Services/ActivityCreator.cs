/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Service Provider                      *
*  Type     : ActivityCreator                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Creates project activities based on project model rules.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Creates project activities based on project model rules.</summary>
  internal class ActivityCreator {

    #region Fields

    private Project targetProject;

    private WhatIfResult whatIfResult = null;

    #endregion Fields


    #region Constructors and parsers


    public ActivityCreator(Project targetProject) {
      Assertion.AssertObject(targetProject, "targetProject");

      this.targetProject = targetProject;
    }


    #endregion Constructors and parsers


    #region Public methods


    public WhatIfResult CreateFromEvent(Activity activityModel, DateTime eventDate) {
      Assertion.AssertObject(activityModel, "activityModel");

      // Create root
      this.whatIfResult = new WhatIfResult(activityModel, ProjectItemOperation.CreateFromTemplate);

      this.CreateBranchFromTemplate(activityModel);

      // Set root dates;

      var stateChange = this.whatIfResult.StateChanges[0];

      stateChange.Deadline = eventDate;
      stateChange.Project = this.targetProject;
      // Append any other external dependencies of the activity model tree
      var dependencies = this.whatIfResult.GetUncontainedModelDependencies(activityModel);

      foreach (var dependency in dependencies) {
        this.CreateBranchFromTemplate(dependency);
      }

      // ToDo: Change this by a recursive strategy
      for (int i = 0; i < 10; i++) {
        this.RecalculateDates();
      }

      this.CalculatePeriodicDates(eventDate);

      return this.whatIfResult;
    }


    #endregion Public methods


    #region Private methods


    private void CalculatePeriodicDates(DateTime eventDate) {

      foreach (var stateChange in this.whatIfResult.StateChanges) {

        if (stateChange.Deadline != ExecutionServer.DateMaxValue) {
          continue;
        }

        var template = ((Activity) stateChange.Template).Template;

        if (!template.IsPeriodic) {
          continue;
        }

        stateChange.Deadline = UtilityMethods.CalculateNextPeriodicDate(template, eventDate);


      }  // foreach

    }


    private void CreateBranchFromTemplate(Activity activityModel) {
      var modelBranch = activityModel.GetBranch();

      foreach (var modelItem in modelBranch) {

        var stateChange = new ProjectItemStateChange(modelItem, ProjectItemOperation.CreateFromTemplate);

        stateChange.Project = this.targetProject;

        if (this.whatIfResult.StateChanges.Count != 0) {
          var parent = this.whatIfResult.StateChanges.Find(x => x.Template.Id == modelItem.Parent.Id);

          if (parent != null) {
            stateChange.ParentStateChange = parent;
          } else {
            stateChange.ParentStateChange = this.whatIfResult.StateChanges[0];
          }
        }  // if

        this.whatIfResult.AddStateChange(stateChange);

      }  // foreach

    }


    private void RecalculateDates() {

      foreach (var stateChange in this.whatIfResult.StateChanges) {

        if (stateChange.Deadline != ExecutionServer.DateMaxValue) {
          continue;
        }

        var template = ((Activity) stateChange.Template).Template;

        if (template.DueOnControllerId == -1) {
          continue;
        }

        var controller = this.whatIfResult.StateChanges.Find(x => x.Template.Id == template.DueOnControllerId);


        if (controller == null) {
          continue;
        }

        if (controller.Deadline == ExecutionServer.DateMaxValue) {
          continue;
        }

        DateTime? deadline = UtilityMethods.CalculateNewDeadline(template, controller.Deadline);

        if (deadline.HasValue) {
          stateChange.Deadline = deadline.Value;
        }
      }  // foreach

    }


    #endregion Private methods


  }  // class ActivityCreator

}  // namespace Empiria.ProjectManagement.Services
