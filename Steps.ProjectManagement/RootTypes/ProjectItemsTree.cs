/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Domain services                       *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Tree structure                        *
*  Type     : ProjectItemsTree                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Tree structure of project items.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Json;

namespace Empiria.ProjectManagement {

  /// <summary>Tree structure of project items.</summary>
  internal class ProjectItemsTree {

    #region Fields

    private List<ProjectItem> itemsList = null;

    #endregion Fields

    #region Constructors and parsers

    private ProjectItemsTree(Project project) {
      this.Project = project;

      itemsList = ProjectData.GetProjectActivities(project);
    }


    static internal ProjectItemsTree Load(Project project) {
      Assertion.AssertObject(project, "project");

      return new ProjectItemsTree(project);
    }

    #endregion Constructors and parsers

    #region Properties

    public int Count {
      get {
        return this.ItemsList.Count;
      }
    }


    private List<ProjectItem> ItemsList {
      get {
        return this.itemsList;
      }
    }


    public Project Project {
      get;
    }

    #endregion Properties

    #region Public methods

    internal Activity AddActivity(JsonObject data) {
      Assertion.AssertObject(data, "data");

      var activity = new Activity(this.Project, data);

      activity.SetPosition(this.ItemsList.Count + 1);
      activity.Save();

      this.ItemsList.Add(activity);

      UpdateItemParentAndPosition(activity, data);
      activity.Save();

      return activity;
    }


    internal Activity CopyActivity(Activity activity) {
      Assertion.AssertObject(activity, "activity");

      FixedList<ProjectItem> branchToCopy = activity.GetBranch();

      var copiedItems = new List<Tuple<Activity, string>>(branchToCopy.Count);

      foreach (var item in branchToCopy) {
        var copy = new Activity(this.Project, item);

        copy.SetPosition(this.ItemsList.Count + 1);

        var parentInBranch = branchToCopy.Find(x => x.Id == item.Parent.Id);

        if (parentInBranch != null) {
          var copyParent = copiedItems.Find(x => x.Item2 == parentInBranch.UID);

          copy.SetParent(copyParent.Item1);
        }

        copy.Save();

        this.ItemsList.Add(copy);

        copiedItems.Add(Tuple.Create(copy, item.UID));
      }

      return copiedItems[0].Item1;
    }


    internal void DeleteActivity(Activity activity) {
      Assertion.AssertObject(activity, "activity");

      Assertion.Assert(this.ItemsList.Contains(activity),
                      $"Activity '{activity.Name}' doesn't belong to this project.");

      Assertion.Assert(this.IsLeaf(activity),
                      $"Activity '{activity.Name}' can't be deleted because has children.");

      activity.Delete();

      this.ItemsList.Remove(activity);

      this.RefreshPositions();
    }


    public Activity GetActivity(string activityUID) {
      Assertion.AssertObject(activityUID, "activityUID");

      Activity activity = this.ItemsList.Find((x) => x.UID == activityUID && x is Activity) as Activity;

      Assertion.AssertObject(activity,
                             $"Activity with uid '{activityUID}' is not part of project '{this.Project.Name}'.");

      return activity;
    }


    internal Activity MoveActivity(Activity activity) {
      Assertion.AssertObject(activity, "activity");
      Assertion.Assert(!activity.Project.Equals(this.Project),
            $"Can't move activity '{activity.Name}' because its project is the same than the target project.");

      FixedList<ProjectItem> branch = activity.GetBranch();

      var sourceProject = activity.Project;

      activity.SetParent(Activity.Empty);
      foreach (var item in branch) {
        item.SetPosition(this.ItemsList.Count + 1);
        item.SetProject(this.Project);

        item.Save();

        this.ItemsList.Add(item);
      }

      sourceProject.RemoveBranch(activity);

      return activity;
    }


    internal void RemoveBranch(Activity root) {
      Assertion.AssertObject(root, "root");

      FixedList<ProjectItem> branch = root.GetBranch();

      foreach (var item in branch) {
        this.ItemsList.Remove(item);
      }

      this.RefreshPositions();
    }


    internal FixedList<ProjectItem> ToFixedList() {
      return this.ItemsList.ToFixedList();
    }


    internal void UpdateItemParentAndPosition(ProjectItem item, JsonObject data) {
      ProjectItem parentFromJson = TryGetParentFromJson(data);
      int? positionFromJson = TryGetPositionFromJson(data);

      if (parentFromJson != null && item.Parent.Equals(parentFromJson)) {
        parentFromJson = null;          // Nothing to change
      }
      if (positionFromJson != null && item.Position == positionFromJson.Value) {
        positionFromJson = null;        // Nothing to change
      }

      if (parentFromJson != null && positionFromJson != null) {
        Assertion.AssertFail("It is not possible to change position and parent at the same time.");

        return;

      } else if (parentFromJson != null && positionFromJson == null) {
        this.ChangeParent(item, parentFromJson);

        return;

      } else if (parentFromJson == null && positionFromJson != null) {
        this.ChangePosition(item, positionFromJson.Value);
        return;

      } else if (parentFromJson == null && positionFromJson == null) {
        // No-op. Nothing to change.
        return;

      } else {
        throw Assertion.AssertNoReachThisCode();
      }

    }

    #endregion Public methods

    #region Private methods

    private void ChangeParent(ProjectItem item, ProjectItem newParent) {
      var branchToMove = this.GetBranch(item);

      Assertion.Assert(!branchToMove.Contains(newParent),
                       $"Can't change the parent of '{item.Name}' because it is a branch " +
                       $"and '{newParent.Name}' is one of its children.");

      foreach (var branchItem in branchToMove) {
        ItemsList.Remove(branchItem);
      }

      int insertionIndex = this.ItemsList.IndexOf(newParent) + 1;

      foreach (var branchItem in branchToMove) {
        ItemsList.Insert(insertionIndex, branchItem);

        insertionIndex++;
      }

      this.RefreshPositions();

      item.SetParent(newParent);
    }


    private void ChangePosition(ProjectItem item, int newPosition) {
      var branchToMove = this.GetBranch(item);

      Assertion.Assert(newPosition < branchToMove[0].Position ||
                       newPosition > branchToMove[branchToMove.Count - 1].Position,
                       "Can't move item because it's a branch and the requested new position is inside it.");

      int insertionIndex = Math.Min(newPosition - 1, this.ItemsList.Count);

      // Get the insertion before item point
      ProjectItem insertBeforeItem = insertionIndex < this.ItemsList.Count ? this.ItemsList[insertionIndex] : null;

      // Then remove the whole branch an reinsert it in the new position
      foreach (var branchItem in branchToMove) {
        ItemsList.Remove(branchItem);
      }

      // Recalculate the new insertion index
      insertionIndex = insertBeforeItem != null ? this.ItemsList.IndexOf(insertBeforeItem) : this.ItemsList.Count;

      foreach (var branchItem in branchToMove) {
        ItemsList.Insert(insertionIndex, branchItem);

        insertionIndex++;
      }

      this.RefreshPositions();

      // Then change item's parent
      if (insertBeforeItem != null) {
        item.SetParent(insertBeforeItem.Parent);
      } else {
        item.SetParent(ProjectItem.Empty);
      }
    }


    internal FixedList<ProjectItem> GetBranch(ProjectItem root) {
      var branch = new List<ProjectItem>();

      branch.Add(root);

      var rootIndex = this.ItemsList.IndexOf(root);

      for (int i = rootIndex + 1; i < this.ItemsList.Count; i++) {
        var item = this.ItemsList[i];

        if (item.Level > root.Level) {
          branch.Add(item);
        } else {
          break;
        }
      }
      return branch.ToFixedList();
    }

    private FixedList<ProjectItem> GetChildren(ProjectItem item) {
      return this.ItemsList.FindAll((x) => x.Parent.Equals(item))
                           .ToFixedList();
    }


    private bool IsLeaf(ProjectItem item) {
      return (this.GetChildren(item).Count == 0);
    }


    internal void RefreshPositions() {
      for (int i = 0; i < this.ItemsList.Count; i++) {
        var item = this.ItemsList[i];

        if (item.Position != (i + 1)) {
          item.SetPosition(i + 1);

          ProjectItemData.UpdatePosition(item);
        }
      }  // for
    }


    private ProjectItem TryGetLastChildOf(ProjectItem parent) {
      return this.ItemsList.FindLast((x) => x.Parent.Equals(parent));
    }


    private ProjectItem TryGetParentFromJson(JsonObject data) {
      if (!data.Contains("parentUID")) {
        return null;
      }

      var parent = data.Get<ProjectItem>("parentUID");

      if (parent.IsEmptyInstance) {
        return ProjectItem.Empty;
      }

      Assertion.Assert(this.ItemsList.Contains(parent),
                       new ValidationException("UnrecognizedActivityParent",
                                               $"Invalid activity parent '{parent.Name}' for project '{this.Project.Name}'."));
      return parent;
    }


    private int? TryGetPositionFromJson(JsonObject data) {
      if (!data.Contains("position")) {
        return null;
      }

      var position = data.Get<int>("position");

      Assertion.Assert(1 <= position && position <= this.ItemsList.Count + 1,
                       new ValidationException("PositionOutOfIndex",
                                               $"Invalid activity position {position} for project '{this.Project.Name}'."));

      return position;
    }

    #endregion Public methods

  } // class ProjectItemsTree

} // namespace Empiria.ProjectManagement
