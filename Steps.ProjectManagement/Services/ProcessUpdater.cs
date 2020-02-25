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


    public ProcessUpdater(ProjectItem projectItem) {
      this.ProjectItem = projectItem;
    }


    #endregion Constructors and parsers


    #region Fields


    Activity ProcessDefinitionRootItem {
      get {
        return this.ProjectItem.GetTemplate();
      }
    }


    Project Project {
      get {
        return this.ProjectItem.Project;
      }
    }


    ProjectItem ProjectItem {
      get;
    }


    #endregion Fields


    #region Public Methods


    internal WhatIfResult OnUpdateProcess() {
      WhatIfResult newModelResult = GetNewModelResult();

      WhatIfResult current = GetWhatIfResultWithCurrentUnmatchedActivities();

      WhatIfResult merged = GetMergeWhatIfResult(current.StateChanges, newModelResult.StateChanges);

      ApplyCurrentCompletedActivities(merged);

      return merged;
    }


    static public FixedList<ProjectItem> UpdatedWithLastProcessChanges(ProjectItem projectItem) {
      var updater = new ProcessUpdater(projectItem);

      WhatIfResult result = updater.OnUpdateProcess();

      if (result.HasErrors) {
        throw result.GetException();
      }

      StoreChanges(result);

      return result.StateChanges.ConvertAll(x => x.ProjectItem)
                   .ToFixedList();
    }


    #endregion Public Methods


    #region Private methods


    static private void ApplyCurrentCompletedActivities(WhatIfResult current) {
      var currentCompletedActivities =
                current.StateChanges.FindAll(x => x.ProjectItem.ActualEndDate != ExecutionServer.DateMaxValue &&
                                                  x.ProjectItem.Status == StateEnums.ActivityStatus.Completed);

      foreach (var completedActivity in currentCompletedActivities) {
        var onCompleted = ModelingServices.WhatIfCompleted(completedActivity.ProjectItem,
                                                           completedActivity.ProjectItem.ActualEndDate, false);

        foreach (var change in onCompleted.StateChanges) {
          var lookup = current.StateChanges.Find(x => x.ProjectItem.UID == change.ProjectItem.UID);

          if (lookup != null && change.Deadline != lookup.ProjectItem.Deadline &&
              change.Operation == ProjectItemOperation.UpdateDeadline) {
            lookup.ActualEndDate = change.ActualEndDate;
            lookup.Deadline = change.Deadline;
            lookup.ProcessMatchResult = ProjectItemProcessMatchResult.MatchedWithDeadlineChanges;
          }  // if

        }  // foreach

      }  // foreach
    }


    static private bool DataChanged(ProjectItemStateChange currentItem,
                                    ProjectItemStateChange matchedInNewModel) {
      ProjectItem projectItem = currentItem.ProjectItem;

      if (projectItem.Status != StateEnums.ActivityStatus.Pending) {
        return false;
      }

      // ToDo: Parametrize hardcoded constants
      if (projectItem.Name.Contains("Vista") || projectItem.Name.Contains("Jaguar")) {
        return false;
      }

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
      WhatIfResult merge = new WhatIfResult(this.ProjectItem, ProjectItemOperation.UpdateProcessChanges);

      merge.AddStateChanges(current);

      foreach (var item in merge.StateChanges) {
        if (item.ProjectItem.HasTemplate) {
          TryMatch(item, newModel);
        } else {
          item.ProcessMatchResult = ProjectItemProcessMatchResult.OnlyInProject;
        }
      }

      return merge;

      //while (true) {
      //  if (newModelIndex == newModel.Count && currentIndex == current.Count) {
      //    break;

      //  } else if (newModelIndex < newModel.Count && currentIndex == current.Count) {
      //    newModel[newModelIndex].ProcessMatchResult = ProjectItemProcessMatchResult.OnlyInProcess;

      //    merge.AddStateChange(newModel[newModelIndex]);

      //    newModelIndex++;
      //  } else if (newModelIndex == newModel.Count && currentIndex < current.Count) {
      //    current[currentIndex].ProcessMatchResult = ProjectItemProcessMatchResult.OnlyInProject;

      //    merge.AddStateChange(current[currentIndex]);

      //    currentIndex++;
      //  } else if (newModelIndex < newModel.Count && currentIndex < current.Count) {
      //    ProjectItemStateChange mergedStateChange = GetMergedStateChange(newModel[newModelIndex], current[currentIndex],
      //                                                                    ref newModelIndex, ref currentIndex);

      //    merge.AddStateChange(mergedStateChange);

      //  }  // if

      //}  // while

      //return merge;
    }


    static private ProjectItemStateChange GetMergedStateChange(ProjectItemStateChange currentState,
                                                               ProjectItemStateChange newModelState,
                                                               ref int newModelIndex, ref int currentIndex) {

      if (newModelState.Template.Id == currentState.ProjectItem.TemplateId) {
        newModelIndex++;
        currentIndex++;

        return new ProjectItemStateChange(currentState.ProjectItem, ProjectItemOperation.UpdateProcessChanges) {
          ProcessMatchResult = ProjectItemProcessMatchResult.MatchedEqual
        };
      }

      if (newModelIndex < currentIndex) {
        newModelIndex++;
        newModelState.ProcessMatchResult = ProjectItemProcessMatchResult.OnlyInProcess;
        return newModelState;

      } else if (newModelIndex > currentIndex) {
        currentIndex++;
        currentState.ProcessMatchResult = ProjectItemProcessMatchResult.OnlyInProject;

        return currentState;
      } else {
        currentIndex++;
        newModelIndex++;

        currentState.ProcessMatchResult = ProjectItemProcessMatchResult.OnlyInProject;

        return currentState;
      }
    }


    private WhatIfResult GetNewModelResult() {
      return ModelingServices.WhatIfCreatedFromEvent(this.ProcessDefinitionRootItem,
                                                     this.Project, this.ProjectItem.Deadline);
    }


    private WhatIfResult GetWhatIfResultWithCurrentUnmatchedActivities() {
      WhatIfResult result = new WhatIfResult(this.ProjectItem, ProjectItemOperation.UpdateProcessChanges);

      // FixedList<ProjectItem> currentActivities = this.Project.GetInProcessList(this.ProjectItem);

      FixedList<ProjectItem> currentActivities = this.ProjectItem.GetBranch();

      foreach (var projectItem in currentActivities) {
        var stateChange = new ProjectItemStateChange(projectItem, ProjectItemOperation.UpdateProcessChanges);

        result.AddStateChange(stateChange);
      }

      return result;
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


    #endregion Private methods

  }  // class ProcessUpdater

}  // namespace Empiria.ProjectManagement.Services
