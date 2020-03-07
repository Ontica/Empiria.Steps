/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Service provider                      *
*  Type     : ProcessUpdater                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides project management inner process update and merge changes services.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Provides project management inner process update and merge changes services.</summary>
  public class ProcessUpdater {

    #region Constructors and parsers


    public ProcessUpdater(ProjectItem rootProjectItem) {
      Assertion.AssertObject(rootProjectItem, "rootProjectItem");

      this.RootProjectItem = rootProjectItem;
    }


    #endregion Constructors and parsers


    #region Fields


    Activity ProcessDefinitionRootItem {
      get {
        return this.RootProjectItem.GetTemplate();
      }
    }


    Project Project {
      get {
        return this.RootProjectItem.Project;
      }
    }


    ProjectItem RootProjectItem {
      get;
    }


    #endregion Fields


    #region Public Methods


    internal WhatIfResult OnUpdateProcess() {
      WhatIfResult current = GetWhatIfResultWithRootProjectItemBranch();

      WhatIfResult newModelResult = GetNewModelResult();

      WhatIfResult merged = GetMergeWhatIfResult(current.StateChanges, newModelResult.StateChanges);

      AddNewModelActivitiesToCurrentResult(merged, newModelResult);

      UpdatedDeadlinesForCurrentCompletedActivities(merged);

      return merged;
    }


    public FixedList<ProjectItem> UpdatedWithLastProcessChanges() {
      WhatIfResult result = this.OnUpdateProcess();

      if (result.HasErrors) {
        throw result.GetException();
      }

      StoreChanges(result);

      MoveProjectItemsAccordingToInsertionRules(result);

      return result.StateChanges.ConvertAll(x => x.ProjectItem)
                   .ToFixedList();
    }


    #endregion Public Methods


    #region Private methods


    static private ProjectItemProcessMatchResult AddDeadlineChangeToMatchResult(ProjectItemProcessMatchResult processMatchResult) {
      switch (processMatchResult) {

        case ProjectItemProcessMatchResult.MatchedEqual:
          return ProjectItemProcessMatchResult.MatchedWithDeadlineChanges;

        case ProjectItemProcessMatchResult.MatchedWithDataChanges:
          return ProjectItemProcessMatchResult.MatchedWithDeadlineAndDataChanges;

        default:
          return processMatchResult;
      }
    }


    private void AddNewModelActivitiesToCurrentResult(WhatIfResult current,
                                                      WhatIfResult newModelResult) {
      try {
        var notIncludedList = newModelResult.StateChanges.FindAll(x => !current.StateChanges.Contains(y => y.Template == x.Template));

        foreach (var notIncluded in notIncludedList) {
          notIncluded.ProcessMatchResult = ProjectItemProcessMatchResult.OnlyInProcess;

          notIncluded.ProcessID = this.RootProjectItem.ProcessID;
          notIncluded.SubprocessID = this.RootProjectItem.SubprocessID;

          int index = SetNewActivityInsertionRule(notIncluded, current, newModelResult);

          if (index >= 0) {
            current.InsertStateChange(index, notIncluded);
          } else {
            current.AddStateChange(notIncluded);
          }
        }
      } catch (Exception e) {
        EmpiriaLog.Error(e);
        throw e;
      }
    }


    static private bool DataChanged(ProjectItemStateChange currentItem,
                                    ProjectItemStateChange matchedInNewModel) {
      ProjectItem projectItem = currentItem.ProjectItem;

      if (projectItem.Status == StateEnums.ActivityStatus.Completed) {
        return false;
      }

      // ToDo: Parametrize hardcoded constants
      if (projectItem.Name.Contains("Vista") || projectItem.Name.Contains("Jaguar")) {
        return false;
      }

      // ToDo: Parametrize hardcoded constant
      if (projectItem.TemplateId == 109643) {
        return false;
      }

      if (projectItem.Name != matchedInNewModel.Template.Name) {
        return true;
      }

      if (projectItem.Notes != matchedInNewModel.Template.Notes) {
        return true;
      }

      if (projectItem.Theme != matchedInNewModel.Template.Theme) {
        return true;
      }

      return false;
    }


    static private bool DeadlineChanged(ProjectItemStateChange currentItem,
                                        ProjectItemStateChange matchedInNewModel) {
      ProjectItem projectItem = currentItem.ProjectItem;

      if (matchedInNewModel.Deadline == ExecutionServer.DateMaxValue) {
        return false;
      }

      if (projectItem.Deadline == matchedInNewModel.Deadline) {
        return false;
      }

      return true;
    }


    static private bool HasProgrammingRule(ProjectItemStateChange currentItem,
                                           ProjectItemStateChange matchedInNewModel) {
      if (DeadlineChanged(currentItem, matchedInNewModel) &&
          ((Activity) matchedInNewModel.Template).Template.IsPeriodic) {
        return false;
      }

      return true;
    }


    private WhatIfResult GetMergeWhatIfResult(FixedList<ProjectItemStateChange> current,
                                              FixedList<ProjectItemStateChange> newModel) {
      WhatIfResult merge = new WhatIfResult(this.RootProjectItem, ProjectItemOperation.UpdateProcessChanges);

      merge.AddStateChanges(current);

      foreach (var item in merge.StateChanges) {
        if (item.ProjectItem.HasTemplate) {
          TryMatch(item, newModel);
        } else {
          item.ProcessMatchResult = ProjectItemProcessMatchResult.OnlyInProject;
        }
      }

      return merge;
    }


    private WhatIfResult GetNewModelResult() {
      return ModelingServices.WhatIfCreatedFromEvent(this.ProcessDefinitionRootItem,
                                                     this.Project, this.RootProjectItem.Deadline);
    }


    private WhatIfResult GetWhatIfResultWithRootProjectItemBranch() {
      WhatIfResult result = new WhatIfResult(this.RootProjectItem, ProjectItemOperation.UpdateProcessChanges);

      FixedList<ProjectItem> currentActivities = this.RootProjectItem.GetBranch();

      foreach (var projectItem in currentActivities) {
        var stateChange = new ProjectItemStateChange(projectItem, ProjectItemOperation.UpdateProcessChanges);

        result.AddStateChange(stateChange);
      }

      return result;
    }


    private void MoveProjectItemsAccordingToInsertionRules(WhatIfResult result) {
      foreach (var item in result.StateChanges) {
        if (item.HasInsertionRule) {
          if (item.ParentStateChange != null && item.InsertionPoint.IsEmptyInstance) {
            item.InsertionPoint = item.ProjectItem;
          }
          this.Project.MoveTo(item.ProjectItem, item.InsertionRule, item.InsertionPoint, item.InsertionPosition);
        }
      }
    }


    static private ProjectItemProcessMatchResult RemoveDeadlineChangeFromMatchResult(ProjectItemProcessMatchResult processMatchResult) {
      switch (processMatchResult) {

        case ProjectItemProcessMatchResult.MatchedWithDeadlineAndDataChanges:
          return ProjectItemProcessMatchResult.MatchedWithDataChanges;

        case ProjectItemProcessMatchResult.MatchedWithDeadlineChanges:
          return ProjectItemProcessMatchResult.MatchedEqual;

        default:
          return processMatchResult;
      }
    }


    static private int SetNewActivityInsertionRule(ProjectItemStateChange toInsertActivity,
                                                   WhatIfResult current, WhatIfResult newModelResult) {

      int position = -1;

      var parent = current.StateChanges.Find(x => !x.ProjectItem.IsEmptyInstance &&
                                                   x.Template.UID == toInsertActivity.Template.Parent.UID);

      if (parent != null) {

        var newSiblings = current.StateChanges.FindAll(x => x.ProjectItem.IsEmptyInstance &&
                                                            x.ParentStateChange.UID == parent.UID);

        var parentIndex = current.StateChanges.IndexOf(parent);

        var currentSiblings = current.StateChanges.FindAll(x => !x.ProjectItem.IsEmptyInstance &&
                                                                 x.ProjectItem.Parent.UID == parent.ProjectItem.UID);

        if (currentSiblings.Count != 0) {
          toInsertActivity.InsertionPoint = parent.ProjectItem;
          toInsertActivity.InsertionRule = TreeItemInsertionRule.AsChildAsLastNode;

          position = parentIndex + currentSiblings.Count + newSiblings.Count + 1;

          EmpiriaLog.Trace($"Rule 0: {toInsertActivity.Template.Name} at position {position}.");

        } else {
          toInsertActivity.ParentStateChange = parent;

          toInsertActivity.InsertionPoint = parent.ProjectItem;
          toInsertActivity.InsertionRule = TreeItemInsertionRule.AsChildAsLastNode;

          position = parentIndex + newSiblings.Count + 1;

          EmpiriaLog.Trace($"Rule 1: {toInsertActivity.Template.Name} at position {position}.");
        }

      } else {

        Assertion.AssertObject(toInsertActivity.ParentStateChange, "ParentStateChange is null");

        var newParent = current.StateChanges.Find(x => x.UID == toInsertActivity.ParentStateChange.UID);

        var newParentIdx = current.StateChanges.IndexOf(newParent);

        var currentSiblings = current.StateChanges.FindAll(x => x.ParentStateChange != null &&
                                                                x.ParentStateChange.UID == toInsertActivity.ParentStateChange.UID);

        toInsertActivity.ParentStateChange = newParent;

        toInsertActivity.InsertionPoint = ProjectItem.Empty;
        toInsertActivity.InsertionRule = TreeItemInsertionRule.AsChildAsLastNode;

        position = newParentIdx + currentSiblings.Count + 1;

        EmpiriaLog.Trace($"Rule 2: {toInsertActivity.Template.Name} at position {position}.");
      }

      return position;

    }


    static private void SetUpdatedData(ProjectItemStateChange currentItem,
                                       ProjectItemStateChange matchedInNewModel) {
      ProjectItem projectItem = currentItem.ProjectItem;

      if (projectItem.Name != matchedInNewModel.Template.Name) {
        currentItem.Name = matchedInNewModel.Template.Name;
      }

      if (projectItem.Notes != matchedInNewModel.Template.Notes) {
        currentItem.Notes = matchedInNewModel.Template.Notes;
      }

      if (projectItem.Theme != matchedInNewModel.Template.Theme) {
        currentItem.Theme = matchedInNewModel.Template.Theme;
      }
    }


   static private void StoreChanges(WhatIfResult result) {

      foreach (var stateChange in result.StateChanges) {
        ProjectItem projectItem = stateChange.ProjectItem;

        switch (stateChange.ProcessMatchResult) {

          case ProjectItemProcessMatchResult.MatchedWithDataChanges:
            projectItem.SetData(stateChange);
            projectItem.Save();
            break;

          case ProjectItemProcessMatchResult.MatchedWithDeadlineAndDataChanges:
            projectItem.SetData(stateChange);
            projectItem.SetDeadline(stateChange.Deadline);
            projectItem.Save();
            break;

          case ProjectItemProcessMatchResult.MatchedWithDeadlineChanges:
            projectItem.SetDeadline(stateChange.Deadline);
            projectItem.Save();
            break;

          case ProjectItemProcessMatchResult.OnlyInProcess:
            stateChange.ProjectItem = ProjectUpdater.CreateFromTemplate(stateChange,
                                                                        stateChange.InsertionPosition);
            break;

          default:
            break;

        }  // switch

      }  // foreach

    }


    static private void TryMatch(ProjectItemStateChange currentItem,
                                 FixedList<ProjectItemStateChange> newModel) {
      ProjectItem projectItem = currentItem.ProjectItem;

      var matchedInNewModel = newModel.Find(x => x.Template.Id == projectItem.TemplateId);

      if (matchedInNewModel == null) {
        if (projectItem.TemplateId != 0) {
          currentItem.ProcessMatchResult = ProjectItemProcessMatchResult.OrphanInProject;
        } else {
          currentItem.ProcessMatchResult = ProjectItemProcessMatchResult.OnlyInProject;
        }
        return;
      }

      bool hasProgrammingRule = HasProgrammingRule(currentItem, matchedInNewModel);
      bool dataChanged = DataChanged(currentItem, matchedInNewModel);
      bool deadlineChanged = DeadlineChanged(currentItem, matchedInNewModel);

      if (!hasProgrammingRule) {

        if (deadlineChanged) {
          currentItem.Deadline = matchedInNewModel.Deadline;
        }

        currentItem.ProcessMatchResult = ProjectItemProcessMatchResult.NoProgrammingRule;

      } else if (deadlineChanged && dataChanged) {
        currentItem.Deadline = matchedInNewModel.Deadline;
        SetUpdatedData(currentItem, matchedInNewModel);

        currentItem.ProcessMatchResult = ProjectItemProcessMatchResult.MatchedWithDeadlineAndDataChanges;

      } else if (deadlineChanged && !dataChanged) {
        currentItem.Deadline = matchedInNewModel.Deadline;

        currentItem.ProcessMatchResult = ProjectItemProcessMatchResult.MatchedWithDeadlineChanges;

      } else if (!deadlineChanged && dataChanged) {
        SetUpdatedData(currentItem, matchedInNewModel);

        currentItem.ProcessMatchResult = ProjectItemProcessMatchResult.MatchedWithDataChanges;

      } else if (!deadlineChanged && !dataChanged) {
        currentItem.ProcessMatchResult = ProjectItemProcessMatchResult.MatchedEqual;

      } else {
        Assertion.AssertNoReachThisCode("Programming error. Should be impossible to reach this code.");
      }
    }


    static private void UpdatedDeadlinesForCurrentCompletedActivities(WhatIfResult current) {
      var currentCompletedActivities =
                current.StateChanges.FindAll(x => x.ProjectItem.ActualEndDate != ExecutionServer.DateMaxValue &&
                                                  x.ProjectItem.Status == StateEnums.ActivityStatus.Completed);

      foreach (var completedActivityStateChange in currentCompletedActivities) {
        UpdateRelatedProjectItemsDeadlines(current, completedActivityStateChange,
                                           completedActivityStateChange.ProjectItem.ActualEndDate);
      }  // foreach

    }


    static private void UpdateRelatedProjectItemsDeadlines(WhatIfResult current,
                                                           ProjectItemStateChange stateChange,
                                                           DateTime completedDate) {
      var projectItem = stateChange.ProjectItem;

      FixedList<ProjectItemStateChange> relatedStateChangesList = current.GetRelatedStateChanges(stateChange);

      foreach (var relatedStateChange in relatedStateChangesList) {
        var model = ((Activity) relatedStateChange.Template).Template;

        DateTime? updatedDeadline = UtilityMethods.CalculateNewDeadline(model, completedDate);

        if (!updatedDeadline.HasValue) {
          continue;
        }

        if (relatedStateChange == null) {
          continue;
        }

        if (updatedDeadline.Value != relatedStateChange.ProjectItem.Deadline) {
          relatedStateChange.Deadline = updatedDeadline.Value;
          relatedStateChange.ProcessMatchResult = AddDeadlineChangeToMatchResult(relatedStateChange.ProcessMatchResult);

        } else {
          relatedStateChange.Deadline = ExecutionServer.DateMaxValue;
          relatedStateChange.ProcessMatchResult = RemoveDeadlineChangeFromMatchResult(relatedStateChange.ProcessMatchResult);
        }

        // Recursive call
        UpdateRelatedProjectItemsDeadlines(current, relatedStateChange, updatedDeadline.Value);
      }
    }


    #endregion Private methods


  }  // class ProcessUpdater

}  // namespace Empiria.ProjectManagement.Services
