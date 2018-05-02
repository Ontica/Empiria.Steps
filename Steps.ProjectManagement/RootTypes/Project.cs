/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : Project                                          License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project as well defined set of activities.                                         *
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

  /// <summary>Describes a project as well defined set of activities.</summary>
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

    [DataField("UID")]
    public string UID {
      get;
      private set;
    }


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

    public List<ProjectItem> ItemsList {
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

      SetParentAndPositionFromJson(activity, data);

      activity.Save();

      this.ItemsList.Insert(activity.Position - 1, activity);

      this.UpdateItemsPositionsBelow(activity);

      return activity;
    }


    public Activity GetActivity(string activityUID) {
      Assertion.AssertObject(activityUID, "activityUID");

      Activity activity = ItemsList.Find((x) => x.UID == activityUID && x is Activity) as Activity;

      Assertion.AssertObject(activity,
                             $"Activity with uid ({activityUID}) is not part of project {this.Name}.");

      return activity;
    }


    public void RemoveActivity(Activity activity) {
      Assertion.AssertObject(activity, "activity");
      Assertion.Assert(ItemsList.Contains(activity),
                      $"Activity {activity.Name} doesn't belong to this project.");

      activity.Delete();

      ItemsList.Remove(activity);

      this.UpdateItemsPositionsBelow(activity);

    }


    public Summary AddSummary(JsonObject data) {
      Assertion.AssertObject(data, "data");

      Summary summary = new Summary(this, data);

      SetParentAndPositionFromJson(summary, data);

      summary.Save();

      ItemsList.Add(summary);

      return summary;
    }


    protected override void OnBeforeSave() {
      if (this.IsNew) {
        this.UID = EmpiriaString.BuildRandomString(6, 36);
      }
    }


    protected override void OnSave() {
      ProjectData.WriteProject(this);
    }

    #endregion Public methods

    #region Private methods

    private ProjectItem GetParentFromJson(JsonObject data) {
      ProjectItem parent = data.Get<ProjectItem>("parentUID", ProjectItem.Empty);

      if (parent.IsEmptyInstance) {
        return ProjectItem.Empty;
      }

      Assertion.Assert(ItemsList.Contains(parent),
                       new ValidationException("UnrecognizedActivityParent",
                                               $"Invalid activity parent ({parent.Name}) for project ({this.UID})."));
      return parent;
    }


    private int GetPositionFromJson(JsonObject data) {
      int position = data.Get("position", -1);

      if (position == -1) {
        return this.ItemsList.Count + 1;
      }

      Assertion.Assert(1 <= position && position <= ItemsList.Count + 1,
                       new ValidationException("PositionOutOfIndex",
                                               $"Invalid activity position ({position}) for project ({this.UID})."));

      return position;
    }


    private void SetParentAndPositionFromJson(ProjectItem item, JsonObject data) {
      ProjectItem parent = GetParentFromJson(data);

      item.SetParent(parent);

      int position = GetPositionFromJson(data);

      item.SetPosition(position);
    }


    private void UpdateItemsPositionsBelow(Activity activity) {
      var items = this.ItemsList.FindAll((x) => x.Position >= activity.Position &&
                                                !x.Equals(activity));

      foreach (var item in items) {
        if (activity.Status != ActivityStatus.Deleted) {
          item.SetPosition(item.Position + 1);
        } else {
          item.SetPosition(item.Position - 1);
        }
        ProjectItemData.UpdatePosition(item);
      }
    }

    #endregion Private methods

  } // class Project

} // namespace Empiria.ProjectManagement
