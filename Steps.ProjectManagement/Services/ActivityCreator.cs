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
using System.Collections.Generic;
using System.Linq;

using Empiria.DataTypes;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Creates project activities based on project model rules.</summary>
  internal class ActivityCreator {

    #region Fields

    private Project targetProject;

    // private List<Activity> createdActivities = new List<Activity>();
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
      var dependencies = this.GetModelDependencies(activityModel);

      foreach (var dependency in dependencies) {
        this.CreateBranchFromTemplate(dependency);
      }

      // ToDo: Change this by a recursive strategy
      for (int i = 0; i < 10; i++) {
        this.RecalculateDates();
      }

      return this.whatIfResult;
    }


    #endregion Public methods


    #region Private methods


    private int AdjustTermOnDueOnCondtion(ActivityModel template, int term) {
      switch (template.DueOnCondition) {
        case "Before":
          return -1 * term;

        case "BeforeStart":
          return -1 * term;

        case "AfterStart":
          return term;

        case "During":
          return term;

        case "BeforeFinish":
          return -1 * term;

        case "AfterFinish":
          return term;

        case "After":
          return term;

        case "From":
          return term;

        default:
          return term;

      } // switch

    }


    private DateTime? CalculateNewDeadline(ActivityModel template, DateTime baseDate) {
      var dueOnTerm = template.DueOnTerm;

      if (String.IsNullOrWhiteSpace(dueOnTerm)) {
        return null;
      }

      var term = int.Parse(dueOnTerm);

      term = AdjustTermOnDueOnCondtion(template, term);

      switch (template.DueOnTermUnit) {
        case "BusinessDays":
          EmpiriaCalendar calendar = GetCalendarFor(template);

          if (term >= 0) {
            return calendar.AddWorkingDays(baseDate, term);
          } else {
            return calendar.SubstractWorkingDays(baseDate, -1 * term);
          }

        case "CalendarDays":
          return baseDate.AddDays(term);

        case "Hours":
          return baseDate.AddHours(term);

        case "Months":
          return baseDate.AddMonths(term);

        case "Years":
          return baseDate.AddYears(term);

        default:
          return null;
      }

    }


    private void CreateBranchFromTemplate(Activity activityModel) {
      var modelBranch = activityModel.GetBranch();

      foreach (var modelItem in modelBranch) {

        var stateChange = new ProjectItemStateChange(modelItem, ProjectItemOperation.CreateFromTemplate);

        stateChange.Project = this.targetProject;

        if (this.whatIfResult.StateChanges.Count != 0) {
          var parent = this.whatIfResult.StateChanges.Find(x => x.Template.Id == modelItem.Parent.Id);

          if (parent != null) {
            stateChange.Parent = parent;
          } else {
            stateChange.Parent = this.whatIfResult.StateChanges[0];
          }
        }  // if
        this.whatIfResult.AddStateChange(stateChange);

      }  // foreach

    }


    private EmpiriaCalendar GetCalendarFor(ActivityModel template) {
      if (template.EntityId != -1) {
        var org = Contacts.Organization.Parse(template.EntityId);

        return EmpiriaCalendar.Parse(org.Nickname);
      } else {
        return EmpiriaCalendar.Default;
      }
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

        DateTime? deadline = CalculateNewDeadline(template, controller.Deadline);

        if (deadline.HasValue) {
          stateChange.Deadline = deadline.Value;
        }
      }  // foreach

    }


    #endregion Private methods


  }  // class ActivityCreator

}  // namespace Empiria.ProjectManagement.Services
