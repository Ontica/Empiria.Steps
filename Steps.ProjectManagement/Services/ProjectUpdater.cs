/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Service provider                      *
*  Type     : ProjectUpdater                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides project management activity's state update services.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;


namespace Empiria.ProjectManagement.Services {


  /// <summary>Provides project management activity's state update services</summary>
  static public class ProjectUpdater {

    #region Services


    static public FixedList<ProjectItem> Complete(ProjectItem projectItem, DateTime completedDate) {
      Assertion.AssertObject(projectItem, "projectItem");

      WhatIfResult result = ModelingServices.WhatIfCompleted(projectItem, completedDate, false);

      if (result.HasErrors) {
        throw result.GetException();
      }

      if (!String.IsNullOrWhiteSpace(projectItem.ProcessID)) {
        ProjectItemData.ResetSubprocessID(projectItem.ProcessID);
      }

      StoreChanges(result);

      CreateNextPeriodicIfNecessary(projectItem, completedDate);
      ProjectItemData.ClearProcessID();

      projectItem.Project.Refresh();


      return projectItem.Project.GetItems();
    }


    static public FixedList<ProjectItem> CreateActivitiesFromModel(Activity activityModel,
                                                                   Project project,
                                                                   DateTime eventDate,
                                                                   ProjectItem insertionPoint,
                                                                   TreeItemInsertionRule insertionRule) {
      Assertion.AssertObject(activityModel, "activityModel");
      Assertion.AssertObject(project, "project");
      Assertion.AssertObject(insertionPoint, "insertionPoint");

      WhatIfResult result = ModelingServices.WhatIfCreatedFromEvent(activityModel, project, eventDate);

      if (result.HasErrors) {
        throw result.GetException();
      }

      ProjectItemData.ResetProcessID();

      StoreChanges(result);

      ProjectItemData.ClearProcessID();

      if (result.StateChanges.Count > 0) {

        if (!insertionPoint.IsEmptyInstance && insertionRule != TreeItemInsertionRule.AsTreeRootAtEnd) {
          project.MoveTo(result.StateChanges[0].ProjectItem, insertionRule, insertionPoint);

        } else if (insertionPoint.IsEmptyInstance && (insertionRule == TreeItemInsertionRule.AsTreeRootAtStart ||
                                                      insertionRule == TreeItemInsertionRule.AsTreeRootAtEnd)) {
          project.MoveTo(result.StateChanges[0].ProjectItem, insertionRule, insertionPoint);
        }
      }
      return project.GetItems();
    }


    static public void Reactivate(ProjectItem projectItem) {
      Assertion.AssertObject(projectItem, "projectItem");

      WhatIfResult result = ModelingServices.WhatIfReactivated(projectItem);

      if (result.HasErrors) {
        throw result.GetException();
      }

      StoreChanges(result);
    }


    #endregion Services


    #region Private methods

    static private Activity CreateFromTemplate(ProjectItemStateChange stateChange) {
      Assertion.AssertObject(stateChange, "stateChange");
      Assertion.AssertObject(stateChange.Project, "stateChange.Project");
      Assertion.AssertObject(stateChange.Template, "stateChange.Template");

      var json = new JsonObject();

      json.Add("name", stateChange.Template.Name);
      json.Add("notes", stateChange.Template.Notes);

      json.Add("deadline", stateChange.Deadline);
      json.Add("plannedEndDate", stateChange.PlannedEndDate);

      json.Add("templateId", stateChange.Template.Id);

      Activity activity = stateChange.Project.AddActivity(json);

      if (stateChange.Replaces != null) {
        activity.SetParentAndPosition(stateChange.Replaces.Parent, stateChange.Replaces.Position);

      } else if (stateChange.Parent != null && !stateChange.Parent.ProjectItem.IsEmptyInstance) {
        activity.SetAndSaveParent(stateChange.Parent.ProjectItem);
      }

      return activity;
    }


    static private void CreateNextPeriodicIfNecessary(ProjectItem projectItem, DateTime completedDate) {
      var nextPeriodicResult = ModelingServices.TryGetNextPeriodic(projectItem, completedDate);

      if (nextPeriodicResult == null) {
        return;
      }

      StoreChanges(nextPeriodicResult);

      projectItem.Project.MoveTo(nextPeriodicResult.StateChanges[0].ProjectItem,
                                 TreeItemInsertionRule.AsSiblingAfterInsertionPoint, projectItem);
    }


    static private void StoreChanges(WhatIfResult result) {
      foreach (var stateChange in result.StateChanges) {

        switch (stateChange.Operation) {

          case ProjectItemOperation.Complete:
            stateChange.ProjectItem.Complete(stateChange.ActualEndDate);
            break;

          case ProjectItemOperation.UpdateDeadline:
            stateChange.ProjectItem.SetDeadline(stateChange.Deadline);
            break;

          case ProjectItemOperation.CreateFromTemplate:
            stateChange.ProjectItem = CreateFromTemplate(stateChange);
            break;

          case ProjectItemOperation.Reactivate:
            stateChange.ProjectItem.Reactivate();
            break;

          default:
            throw Assertion.AssertNoReachThisCode($"Unrecognized WhatIfResult operation {stateChange.Operation}");

        }  // switch

      }  // foreach

    }


    #endregion Private methods

  }  // class ProjectUpdater

}  // namespace Empiria.ProjectManagement.Services
