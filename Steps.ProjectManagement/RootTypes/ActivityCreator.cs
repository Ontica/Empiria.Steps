/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Domain services                       *
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
using Empiria.Json;

namespace Empiria.ProjectManagement {

  /// <summary>Creates project activities based on project model rules.</summary>
  public class ActivityCreator {

    #region Fields

    private Project targetProject;

    private List<Activity> createdActivities = new List<Activity>();

    #endregion Fields


    #region Constructors and parsers


    public ActivityCreator(Project targetProject) {
      Assertion.AssertObject(targetProject, "targetProject");

      this.targetProject = targetProject;
    }


    #endregion Constructors and parsers


    #region Public methods


    public void CreateFromEvent(Activity activityModel, DateTime eventDate) {
      Assertion.AssertObject(activityModel, "activityModel");

      this.createdActivities = new List<Activity>();

      this.CreateBranchFromTemplate(activityModel);

      // Set root dates
      var json = new JsonObject();

      json.Add("deadline", eventDate);
      json.Add("plannedEndDate", eventDate);

      this.createdActivities[0].Update(json);

      // Append any other external dependencies of the activity model tree
      var dependencies = this.GetModelDependencies(activityModel);

      foreach (var dependency in dependencies) {

        this.CreateBranchFromTemplate(dependency);

      }

      // ToDo: Change this by a recursive strategy
      for (int i = 0; i < 5; i++) {
        this.RecalculateDates();
      }

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

          EmpiriaCalendar calendar = GetCalendar(template);

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

        var json = new JsonObject();

        json.Add("name", modelItem.Name);
        json.Add("notes", modelItem.Notes);

        json.Add("templateId", modelItem.Id);

        var activity = this.targetProject.AddActivity(json);

        if (this.createdActivities.Count != 0) {
          var parent = this.createdActivities.Find(x => x.TemplateId == modelItem.Parent.Id);

          if (parent != null) {
            activity.SetAndSaveParent(parent);
          } else {
            activity.SetAndSaveParent(this.createdActivities[0]);
          }

        }

        this.createdActivities.Add(activity);
      }

    }


    private EmpiriaCalendar GetCalendar(ActivityModel template) {
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
                                       !this.createdActivities.Exists(y => y.TemplateId == x.Id))
                         .ToFixedList();
    }


    private void RecalculateDates() {

      foreach (var activity in this.createdActivities) {

        if (activity.Deadline != ExecutionServer.DateMaxValue) {
          continue;
        }

        var template = Activity.Parse(activity.TemplateId).Template;

        if (template.DueOnControllerId == -1) {
          continue;
        }

        var controller = targetProject.GetItems().Find(x => x.TemplateId == template.DueOnControllerId);


        if (controller == null) {
          continue;
        }

        if (controller.Deadline == ExecutionServer.DateMaxValue) {
          continue;
        }


        DateTime? deadline = CalculateNewDeadline(template, controller.Deadline);

        if (!deadline.HasValue) {
          continue;
        }

        var json = new JsonObject();

        json.Add("deadline", deadline.Value);

        activity.Update(json);

      }

    }


    #endregion Private methods

  }  // class ActivityCreator

}  // namespace Empiria.ProjectManagement
