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
      WhatIfResult newModelResult = GetNewModelResult();

      WhatIfResult current = GetWhatIfResultWithRootProjectItemBranch();

      WhatIfResult merged = GetMergeWhatIfResult(current.StateChanges, newModelResult.StateChanges);

      UpdatedDeadlinesForCurrentCompletedActivities(merged);

      return merged;
    }


    public FixedList<ProjectItem> UpdatedWithLastProcessChanges() {
      WhatIfResult result = this.OnUpdateProcess();

      if (result.HasErrors) {
        throw result.GetException();
      }

      StoreChanges(result);

      return result.StateChanges.ConvertAll(x => x.ProjectItem)
                   .ToFixedList();
    }


    #endregion Public Methods


    #region Private methods


    static private ProjectItemProcessMatchResult AddDeadlineChangeToMatchResult(ProjectItemProcessMatchResult processMatchResult) {
      if (processMatchResult == ProjectItemProcessMatchResult.MatchedEqual) {
        return ProjectItemProcessMatchResult.MatchedWithDeadlineChanges;
      } else if (processMatchResult == ProjectItemProcessMatchResult.MatchedWithDataChanges) {
        return ProjectItemProcessMatchResult.MatchedWithDeadlineAndDataChanges;
      } else {
        return processMatchResult;
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


    static private void UpdatedDeadlinesForCurrentCompletedActivities(WhatIfResult current) {
      var currentCompletedActivities =
                current.StateChanges.FindAll(x => x.ProjectItem.ActualEndDate != ExecutionServer.DateMaxValue &&
                                                  x.ProjectItem.Status == StateEnums.ActivityStatus.Completed);

      foreach (var completedActivity in currentCompletedActivities) {
        var onCompleted = ModelingServices.WhatIfCompleted(completedActivity.ProjectItem,
                                                           completedActivity.ProjectItem.ActualEndDate, false);

        foreach (var onCompletedStateChange in onCompleted.StateChanges) {
          if (onCompletedStateChange.Operation != ProjectItemOperation.UpdateDeadline) {
            continue;
          }

          var stateToChange = current.StateChanges.Find(x => x.ProjectItem.UID == onCompletedStateChange.ProjectItem.UID);

          if (stateToChange == null) {
            continue;
          }

          if (onCompletedStateChange.Deadline != stateToChange.ProjectItem.Deadline) {
            stateToChange.Deadline = onCompletedStateChange.Deadline;
            stateToChange.ProcessMatchResult = AddDeadlineChangeToMatchResult(stateToChange.ProcessMatchResult);
          } else {
            stateToChange.Deadline = ExecutionServer.DateMaxValue;
            stateToChange.ProcessMatchResult = RemoveDeadlineChangeFromMatchResult(stateToChange.ProcessMatchResult);
          }

        }  // foreach

      }  // foreach

    }


    #endregion Private methods

  }  // class ProcessUpdater

}  // namespace Empiria.ProjectManagement.Services
