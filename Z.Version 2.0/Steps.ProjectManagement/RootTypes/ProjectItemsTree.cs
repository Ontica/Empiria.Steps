﻿/* Empiria Steps *********************************************************************************************
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

      itemsList = ProjectData.GetProjectActivities(this.Project);
    }


    static internal ProjectItemsTree Load(Project project) {
      Assertion.Require(project, "project");

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
      Assertion.Require(data, "data");

      var activity = new Activity(this.Project, data);

      activity.SetPosition(this.ItemsList.Count + 1);
      activity.Save();

      this.ItemsList.Add(activity);

      UpdateItemParentAndPosition(activity, data);
      activity.Save();

      return (Activity) this.ItemsList.Find(x => x.UID == activity.UID);
    }


    internal Activity InsertActivity(JsonObject data, int position) {
      Assertion.Require(data, "data");

      var activity = new Activity(this.Project, data);

      activity.SetPosition(position);
      activity.Save();

      this.ItemsList.Insert(position - 1, activity);
      UpdateItemParentAndPosition(activity, data);
      activity.Save();

      for (int i = position; i < this.ItemsList.Count; i++) {
        this.ItemsList[i].SetPosition(i + 1);
      }
      RefreshPositions();

      return (Activity) this.ItemsList.Find(x => x.UID == activity.UID);
    }

    internal Activity CopyActivity(Activity activity) {
      Assertion.Require(activity, "activity");

      FixedList<ProjectItem> branchToCopy = activity.GetBranch();

      var copiedItems = new List<Tuple<Activity, ProjectItem, int>>(branchToCopy.Count);

      foreach (var item in branchToCopy) {
        var copy = new Activity(this.Project, item);

        copy.SetPosition(this.ItemsList.Count + 1);

        var parentInBranch = branchToCopy.Find(x => x.Id == item.Parent.Id);

        if (parentInBranch != null) {
          var copyParent = copiedItems.Find(x => x.Item2.UID == parentInBranch.UID);

          copy.SetParent(copyParent.Item1);
        }


        copy.Save();

        var dependencyId = copy.Template.DueOnControllerId;

        this.ItemsList.Add(copy);

        copiedItems.Add(Tuple.Create(copy, item, dependencyId));
      }


      // Set copied dependencies or remote them if they are not in the same branch
      foreach (var tuple in copiedItems.FindAll(x => x.Item3 != -1)) {
        var dependency = copiedItems.Find(x => x.Item2.Id == tuple.Item3);

        if (dependency != null) {
          tuple.Item1.ExtensionData.Set("config/dueOnController", dependency.Item1.Id);
        } else {
          tuple.Item1.ExtensionData.Remove("config/dueOnController");
          tuple.Item1.ExtensionData.Remove("config/dueOnCondition");
          tuple.Item1.ExtensionData.Remove("config/dueOnTermUnit");
          tuple.Item1.ExtensionData.Remove("config/dueOnTerm");
          tuple.Item1.ExtensionData.Remove("config/dueOnRuleAppliesForAllContracts");
        }

        tuple.Item1.Save();
      }

      return copiedItems[0].Item1;
    }


    internal void DeleteActivity(Activity activity) {
      Assertion.Require(activity, "activity");

      Assertion.Require(this.ItemsList.Exists(x => x.UID == activity.UID),
                      $"Activity '{activity.Name}' doesn't belong to this project.");


      var branch = activity.GetBranch();

      // if (branch.Contains(x => x.Status == StateEnums.ActivityStatus.Completed)) {
      //  Assertion.AssertFail($"Activity {activity.Name} has one or more completed childrens " +
      //                       $"and we can not delete them, so it is not possible to delete the whole branch.");
      //}

      foreach (var item in branch) {
        item.Delete();
        this.ItemsList.Remove(item);
      }

      this.RefreshPositions();
    }


    public Activity GetActivity(string activityUID) {
      Assertion.Require(activityUID, "activityUID");

      Activity activity = this.ItemsList.Find((x) => x.UID == activityUID && x is Activity) as Activity;

      Assertion.Require(activity,
                             $"Activity with uid '{activityUID}' is not part of project '{this.Project.Name}'.");

      return activity;
    }


    internal Activity MoveActivity(Activity activity) {
      Assertion.Require(activity, "activity");
      Assertion.Require(!activity.Project.Equals(this.Project),
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

      return (Activity) this.ItemsList.Find(x => x.UID == activity.UID);
    }


    internal void RemoveBranch(Activity root) {
      Assertion.Require(root, "root");

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
        Assertion.RequireFail("It is not possible to change position and parent at the same time.");

        return;

      } else if (parentFromJson != null && positionFromJson == null) {
        this.ChangeParentKeepingPosition(item, parentFromJson);

        return;

      } else if (parentFromJson == null && positionFromJson != null) {
        this.ChangePosition(item, positionFromJson.Value);
        return;

      } else if (parentFromJson == null && positionFromJson == null) {
        // No-op. Nothing to change.
        return;

      } else {
        throw Assertion.EnsureNoReachThisCode();
      }

    }


    internal ProjectItem ChangeParentAndPosition(ProjectItem item, ProjectItem newParent, int newPosition) {
      EmpiriaLog.Debug($"ChangeParent of {item.Name} to new parent {newParent.Name}");

      if (item.Equals(newParent)) {
        EmpiriaLog.Info($"Trying to change the parent of a tree item to itself {item.Name} ({item.UID}).");

        return item;
      }

      var branchToMove = this.GetBranch(item);

      Assertion.Require(!branchToMove.Contains(newParent),
                       $"Can't change the parent of '{item.Name}' because it is a branch " +
                       $"and '{newParent.Name}' is one of its children.");

      // Then remove the whole branch an reinsert it in the new position
      foreach (var branchItem in branchToMove) {
        ItemsList.Remove(branchItem);
      }

      item.SetParentAndPosition(newParent, newPosition);

      int insertionIndex = newPosition - 1;      // insertionIndex is zero-based
      foreach (var branchItem in branchToMove) {
        ItemsList.Insert(insertionIndex, branchItem);

        insertionIndex++;
      }

      this.RefreshPositions();

      EmpiriaLog.Info($"ChangeParentAndPosition of {item.Name} to {newParent.Name} at position {newParent.Position + 1}.");

      return item;
    }


    internal ProjectItem ChangeParentKeepingPosition(ProjectItem item, ProjectItem newParent) {
      EmpiriaLog.Debug($"ChangeParent of {item.Name} to new parent {newParent.Name} keeping current position.");

      if (item.Equals(newParent)) {
        EmpiriaLog.Info($"Trying to change the parent of a tree item to itself {item.Name} ({item.UID}).");

        return item;
      }

      var branchToMove = this.GetBranch(item);

      Assertion.Require(!branchToMove.Contains(newParent),
                       $"Can't change the parent of '{item.Name}' because it is a branch " +
                       $"and '{newParent.Name}' is one of its children.");

      item.SetParent(newParent);
      item.Save();

      return item;
    }


    internal ProjectItem ChangePosition(ProjectItem item, int newPosition) {
      EmpiriaLog.Debug($"ChangePosition of {item.Name} in position {item.Position} to new position {newPosition}");

      var branchToMove = this.GetBranch(item);

      Assertion.Require(newPosition < branchToMove[0].Position ||
                       newPosition > branchToMove[branchToMove.Count - 1].Position,
                       "Can't move item because it's a branch and the requested new position is inside it.");

      int insertionIndex = Math.Min(newPosition - 1, this.ItemsList.Count);

      // Get the insertion point before item position
      ProjectItem insertBeforeItem = insertionIndex < this.ItemsList.Count ? this.ItemsList[insertionIndex] : null;

      // Then remove the whole branch an reinsert it in the new position
      foreach (var branchItem in branchToMove) {
        ItemsList.Remove(branchItem);
      }

      // Recalculate the new insertion index
      insertionIndex = insertBeforeItem != null ? this.ItemsList.IndexOf(insertBeforeItem) : this.ItemsList.Count;

      var newParent = insertBeforeItem != null ? insertBeforeItem.Parent : ProjectItem.Empty;

      item.SetParentAndPosition(newParent, insertionIndex);

      foreach (var branchItem in branchToMove) {
        ItemsList.Insert(insertionIndex, branchItem);

        insertionIndex++;
      }

      this.RefreshPositions();

      return item;
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


    internal ProjectItem GetBranchLastItem(ProjectItem root) {
      var branch = this.GetBranch(root);

      return branch[branch.Count - 1];
    }


    internal FixedList<ProjectItem> GetInProcessList(ProjectItem projectItem) {
      List<ProjectItem> list = this.ItemsList.FindAll(x => x.ProcessID == projectItem.ProcessID &&
                                                           x.SubprocessID == projectItem.SubprocessID);

      return list.ToFixedList();
    }


    internal ProjectItem MoveToInsertionPoint(ProjectItem itemToMove, TreeItemInsertionRule insertionRule,
                                              ProjectItem insertionPoint = null, int relativePosition = -1) {
      switch (insertionRule) {

        case TreeItemInsertionRule.AsSiblingBeforeInsertionPoint:
          this.ChangeParentAndPosition(itemToMove, newParent: insertionPoint.Parent,
                                       newPosition: insertionPoint.Position);
          return itemToMove;

        case TreeItemInsertionRule.AsSiblingAfterInsertionPoint:
          int branchLastItemPosition = GetBranchLastItem(insertionPoint).Position;

          this.ChangeParentAndPosition(itemToMove, newParent: insertionPoint.Parent,
                                       newPosition: branchLastItemPosition + 1);
          return itemToMove;

        case TreeItemInsertionRule.AsChildAsFirstNode:
          this.ChangeParentAndPosition(itemToMove, newParent: insertionPoint,
                                       newPosition: insertionPoint.Position + 1);

          return itemToMove;

        case TreeItemInsertionRule.AsChildAsLastNode:
          branchLastItemPosition = GetBranchLastItem(insertionPoint).Position;

          this.ChangeParentAndPosition(itemToMove, newParent: insertionPoint,
                                       newPosition: branchLastItemPosition + 1);

          return itemToMove;

        case TreeItemInsertionRule.AsChildAtPosition:
          Assertion.Require(relativePosition > 0, "Relative position must be greater than zero.");

          this.ChangeParentAndPosition(itemToMove, newParent: insertionPoint,
                                       newPosition: insertionPoint.Position + 1 + relativePosition);
          return itemToMove;

        case TreeItemInsertionRule.AsTreeRootAtStart:
          this.ChangePosition(itemToMove, 1);

          return itemToMove;

        case TreeItemInsertionRule.AsTreeRootAtEnd:
          return itemToMove;

        case TreeItemInsertionRule.AtRelativePosition:
          Assertion.Require(relativePosition > 0, "Relative position must be greater than zero.");

          int indexOfInsertionPoint = this.itemsList.IndexOf(insertionPoint);

          this.itemsList.Remove(itemToMove);

          itemToMove.SetPosition(indexOfInsertionPoint + relativePosition + 1);

          this.itemsList.Insert(indexOfInsertionPoint + relativePosition, itemToMove);

          EmpiriaLog.Trace($"Item {itemToMove.Name} inserted at position {indexOfInsertionPoint}/{relativePosition}: [{itemToMove.Position}] // {insertionPoint.Name}.");


          this.RefreshPositions();

          return itemToMove;

        default:
          throw Assertion.EnsureNoReachThisCode($"Unrecognized insertion rule '{insertionRule.ToString()}'.");
      }
    }


    internal void RefreshPositions() {
      for (int i = 0; i < this.ItemsList.Count; i++) {
        var item = this.ItemsList[i];

        if (item.Position != (i + 1)) {
          item.SetPosition(i + 1, false);

          ProjectItemData.UpdatePosition(item);
        }
      }  // for
    }


    #endregion Public methods


    #region Private methods


    private FixedList<ProjectItem> GetChildren(ProjectItem item) {
      return this.ItemsList.FindAll(x => x.Parent.Equals(item))
                           .ToFixedList();
    }


    private bool IsLeaf(ProjectItem item) {
      return (this.GetChildren(item).Count == 0);
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

      if (!this.ItemsList.Exists(x => x.UID == parent.UID)) {
        throw new ValidationException("UnrecognizedActivityParent",
                                     $"Invalid activity parent '{parent.Name}' for project '{this.Project.Name}'.");
      }

      return parent;
    }


    private int? TryGetPositionFromJson(JsonObject data) {
      if (!data.Contains("position")) {
        return null;
      }

      var position = data.Get<int>("position");

      if (!(1 <= position && position <= this.ItemsList.Count + 1)) {
        throw new ValidationException("PositionOutOfIndex",
                                     $"Invalid activity position {position} for project '{this.Project.Name}'.");

      }

      return position;
    }

    #endregion Private methods

  } // class ProjectItemsTree

} // namespace Empiria.ProjectManagement
