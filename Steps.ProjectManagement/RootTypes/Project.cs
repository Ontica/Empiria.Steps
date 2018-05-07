/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Domain services                       *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Aggregate root                        *
*  Type     : ActivityController                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes a project as a tree of activities.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Collections;
using Empiria.Contacts;
using Empiria.DataTypes;
using Empiria.Json;
using Empiria.StateEnums;

using Empiria.Workflow.Definition;

using Empiria.ProjectManagement.Resources;

  namespace Empiria.ProjectManagement {

  /// <summary>Describes a project as a tree of activities.</summary>
  public class Project : BaseObject {

    #region Fields

    private Lazy<List<ProjectItem>> itemsList = null;

    #endregion Fields

    #region Constructors and parsers

    protected Project() {
      // Required by Empiria Framework.
    }


    static internal Project Parse(int id) {
      return BaseObject.ParseId<Project>(id);
    }


    static public Project Parse(string uid) {
      return BaseObject.ParseKey<Project>(uid);
    }


    static public Project Empty {
      get {
        return BaseObject.ParseEmpty<Project>();
      }
    }


    static public FixedList<Project> GetList(string filter = "") {
      var ownerOrManager = Contact.Parse(51);

      var list = ProjectData.GetProjects(ownerOrManager);

      return list.ToFixedList();
    }


    protected override void OnInitialize() {
      itemsList = new Lazy<List<ProjectItem>>(() => ProjectData.GetProjectActivities(this));
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("Name")]
    public string Name {
      get;
      private set;
    }


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }


    [DataField("Tags")]
    public TagsCollection Tags {
      get;
      private set;
    } = TagsCollection.Empty;


    [DataField("RagStatus", Default = RAGStatus.NoColor)]
    public RAGStatus RagStatus {
      get;
      private set;
    }


    [DataField("ExtData")]
    protected internal JsonObject ExtensionData {
      get;
      private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Name, this.Tags.ToString());
      }
    }


    [DataField("ItemPosition")]
    public int Position {
      get;
      private set;
    }


    public int Level {
      get {
        if (!this.Parent.IsEmptyInstance) {
          return this.Parent.Level + 1;
        } else {
          return 1;
        }
      }
    }


    internal protected Project Parent {
      get;
      private set;
    }


    [DataField("EstimatedDuration")]
    public Duration EstimatedDuration {
      get;
      private set;
    } = Duration.Empty;


    [DataField("StartDate")]
    public DateTime StartDate {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("TargetDate")]
    public DateTime TargetDate {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("EndDate")]
    public DateTime EndDate {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("DueDate")]
    public DateTime DueDate {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("OwnerId")]
    public Contact Owner {
      get;
      private set;
    }


    [DataField("Status", Default = ActivityStatus.Pending)]
    public ActivityStatus Status {
      get;
      private set;
    }


    [DataField("WorkflowObjectId")]
    public WorkflowObject WorkflowObject {
      get;
      private set;
    } = WorkflowObject.Empty;


    [DataField("ResponsibleId")]
    public Contact Responsible {
      get;
      private set;
    }


    [DataField("ResourceId")]
    public Resource Resource {
      get;
      private set;
    } = Resource.Empty;


    #endregion Public properties

    #region Project structure

    private List<ProjectItem> ItemsList {
      get {
        return this.itemsList.Value;
      }
    }


    public FixedList<ProjectItem> GetItems(ActivityFilter filter = null,
                                           ActivityOrder orderBy = ActivityOrder.Default) {
      if (filter == null) {
        filter = new ActivityFilter();
      }

      return ProjectData.GetProjectActivities(this, filter, orderBy)
                        .ToFixedList();
    }

    #endregion Project structure

    #region Project participants

    public FixedList<Contact> Responsibles {
      get {
        return ProjectData.GetProjectResponsibles(this);
      }
    }


    public FixedList<Contact> Requesters {
      get {
        return ProjectData.GetProjectRequesters(this);
      }
    }


    public FixedList<Contact> TaskManagers {
      get {
        return ProjectData.GetProjectTaskManagers(this);
      }
    }


    internal FixedList<Contact> GetInvolvedContacts() {
      return ProjectData.GetProjectInvolvedContacts(this);
    }


    #endregion Project participants

    #region Public methods

    public Activity AddActivity(JsonObject data) {
      Assertion.AssertObject(data, "data");

      var activity = new Activity(this, data);

      activity.SetPosition(this.ItemsList.Count + 1);
      activity.Save();

      this.ItemsList.Add(activity);

      UpdateParentAndPositionFromJson(activity, data);

      return activity;
    }


    public Activity GetActivity(string activityUID) {
      Assertion.AssertObject(activityUID, "activityUID");

      Activity activity = ItemsList.Find((x) => x.UID == activityUID && x is Activity) as Activity;

      Assertion.AssertObject(activity,
                             $"Activity with uid '{activityUID}' is not part of project '{this.Name}'.");

      return activity;
    }


    protected override void OnSave() {
      ProjectData.WriteProject(this);
    }


    public void RemoveActivity(Activity activity) {
      Assertion.AssertObject(activity, "activity");
      Assertion.Assert(ItemsList.Contains(activity),
                      $"Activity '{activity.Name}' doesn't belong to this project.");
      Assertion.Assert(this.IsLeaf(activity),
                      $"Activity '{activity.Name}' can't be deleted because has children.");

      activity.Delete();

      ItemsList.Remove(activity);

      this.RefreshPositions();
    }


    internal void UpdateParentAndPositionFromJson(ProjectItem item, JsonObject data) {
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

      var lastChild = this.TryGetLastChildOf(newParent);

      int insertionIndex = 0;
      if (lastChild != null) {
        var lastChildBranch = this.GetBranch(lastChild);

        insertionIndex = this.ItemsList.IndexOf(lastChildBranch[lastChildBranch.Count - 1]) + 1;
      } else {
        insertionIndex = this.ItemsList.IndexOf(newParent) + 1;
      }

      foreach (var branchItem in branchToMove) {
        ItemsList.Insert(insertionIndex, branchItem);

        insertionIndex++;
      }

      this.RefreshPositions();

      item.SetParent(newParent);

      item.Save();
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

      item.Save();
    }


    private FixedList<ProjectItem> GetBranch(ProjectItem root) {
      var branch = new List<ProjectItem>();

      branch.Add(root);

      var rootIndex = this.ItemsList.IndexOf(root);

      if (rootIndex == -1) {
        return branch.ToFixedList();
      }

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


    private void RefreshPositions() {
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

      Assertion.Assert(ItemsList.Contains(parent),
                       new ValidationException("UnrecognizedActivityParent",
                                               $"Invalid activity parent '{parent.Name}' for project '{this.Name}'."));
      return parent;
    }


    private int? TryGetPositionFromJson(JsonObject data) {
      if (!data.Contains("position")) {
        return null;
      }

      var position = data.Get<int>("position");

      Assertion.Assert(1 <= position && position <= ItemsList.Count + 1,
                       new ValidationException("PositionOutOfIndex",
                                               $"Invalid activity position ({position}) for project ({this.UID})."));

      return position;
    }

    #endregion Private methods

  } // class Project

} // namespace Empiria.ProjectManagement
