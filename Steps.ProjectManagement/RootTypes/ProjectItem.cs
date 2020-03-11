/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : ProjectItem                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Abstract class that describes a project item, like an activity, a task or summary.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Collections;
using Empiria.DataTypes;
using Empiria.Json;
using Empiria.Ontology;
using Empiria.StateEnums;
using Empiria.ProjectManagement.Services;

namespace Empiria.ProjectManagement {

  /// <summary>Abstract class that describes a project item, like an activity, a task or summary.</summary>
  [PartitionedType(typeof(ProjectItemType))]
  public abstract class ProjectItem : BaseObject {

    #region Constructors and parsers

    protected ProjectItem(ProjectItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    protected internal ProjectItem(ProjectItemType type,
                                   Project project) : base(type) {
      Assertion.AssertObject(type, "type");
      Assertion.AssertObject(project, "project");

      Assertion.Assert(!project.IsEmptyInstance,
                       "Project can't be the empty instance.");


      this.Project = project;
      this.Parent = ProjectItem.Empty;
    }


    protected internal ProjectItem(ProjectItemType type,
                                   Project project,
                                   JsonObject data) : base(type) {
      Assertion.AssertObject(type, "type");
      Assertion.AssertObject(project, "project");
      Assertion.AssertObject(data, "data");

      Assertion.Assert(!project.IsEmptyInstance,
                       "Project can't be the empty instance.");


      this.Project = project;
      this.Parent = ProjectItem.Empty;

      this.AssertIsValid(data);

      this.Load(data);
    }


    static public ProjectItem Parse(int id) {
      return BaseObject.ParseId<ProjectItem>(id);
    }


    static public ProjectItem Parse(string uid) {
      return BaseObject.ParseKey<ProjectItem>(uid);
    }


    static public ProjectItem Empty {
      get {
        return BaseObject.ParseEmpty<ProjectItem>();
      }
    }


    static public FixedList<ProjectItem> GetList() {
      return ProjectData.GetAllActivities();
    }


    protected override void OnLoadObjectData(System.Data.DataRow row) {
      if (!this.IsEmptyInstance) {
        this.Parent = ProjectItem.Parse((int) row["ParentId"]);
      } else {
        this.Parent = this;
      }

      this.ExtensionData = JsonObject.Parse((string) row["ExtData"]);
    }


    #endregion Constructors and parsers

    #region Public properties


    public ProjectItemType ProjectObjectType {
      get {
        return (ProjectItemType) base.GetEmpiriaType();
      }
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


    [DataField("Theme")]
    internal string _theme = String.Empty;

    public string Theme {
      get {
        if (_theme.Length == 0 && this.HasTemplate) {
          return this.GetTemplate().Theme;
        }
        return _theme;
      }
      private set {
        _theme = value;
      }
    }


    [DataField("Tags")]
    public string Tag {
      get;
      private set;
    }


    [DataField("Resource")]
    public string Resource {
      get;
      private set;
    } = "Contrato";


    protected internal JsonObject ExtensionData {
      get;
      private set;
    } = new JsonObject();


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Name, this.Theme, this.Tag, this.Project.Name);
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


    [DataField("BaseProjectId")]
    public Project Project {
      get;
      private set;
    }


    public ProjectItem Parent {
      get;
      private set;
    }


    [DataField("EstimatedDuration")]
    public Duration EstimatedDuration {
      get;
      protected set;
    } = Duration.Empty;


    public int WarnDays {
      get {
        return this.ExtensionData.Get("warnDays", 0);
      }
    }


    public string WarnType {
      get {
        return this.ExtensionData.Get("warnType", "Default");
      }
    }


    [DataField("ActualStartDate")]
    public DateTime ActualStartDate {
      get;
      protected set;
    } = ExecutionServer.DateMaxValue;


    [DataField("ActualEndDate")]
    public DateTime ActualEndDate {
      get;
      protected set;
    } = ExecutionServer.DateMaxValue;


    [DataField("PlannedEndDate")]
    public DateTime PlannedEndDate {
      get;
      protected set;
    } = ExecutionServer.DateMaxValue;


    [DataField("Deadline")]
    public DateTime Deadline {
      get;
      protected set;
    } = ExecutionServer.DateMaxValue;


    public bool IsUpcoming {
      get {
        return this.Status != ActivityStatus.Deleted &&
               this.Status != ActivityStatus.Completed &&
               this.Status != ActivityStatus.Canceled &&
               this.Deadline <= DateTime.Today.AddDays(28);
      }
    }


    public FixedList<Contact> SendAlertsTo {
      get {
        var list = this.ExtensionData.GetList<Contact>("sendAlertsTo", false);

        return list.ToFixedList();
      }
      set {
        Assertion.AssertObject(value, "value");

        var idsArray = value.ConvertAll<int>((x) => x.Id);

        if (idsArray == null || idsArray.Count == 0) {
          this.ExtensionData.Remove("sendAlertsTo");
        } else {
          this.ExtensionData.Set("sendAlertsTo", idsArray);
        }
      }
    }


    [DataField("Status", Default = ActivityStatus.Pending)]
    public ActivityStatus Status {
      get;
      private set;
    }


    [DataField("Stage", Default = ItemStage.Backlog)]
    public ItemStage Stage {
      get;
      private set;
    }


    [DataField("TemplateId")]
    public int TemplateId {
      get;
      private set;
    } = -1;


    public bool HasTemplate {
      get {
        return this.TemplateId > 0;
      }
    }


    public FixedList<Task> Tasks {
      get {
        return ProjectItemData.GetTasks(this)
                              .ToFixedList();
      }
    }


    [DataField("ProcessUID")]
    public string ProcessID {
      get;
      private set;
    }


    [DataField("SubprocessUID")]
    public string SubprocessID {
      get;
      private set;
    }


    public bool HasProcess {
      get {
        return (this.ProcessID.Length != 0);
      }
    }


    #endregion Public properties

    #region Methods

    public Task AddTask(JsonObject data) {
      Assertion.AssertObject(data, "data");

      return new Task(this, data);
    }


    protected virtual void AssertIsValid(JsonObject data) {
      Assertion.AssertObject(data, "data");
    }


    public virtual void Complete(DateTime actualEndDate) {
      Assertion.Assert(this.Status != ActivityStatus.Completed,
                       "This activity is already marked as completed.");

      this.ActualEndDate = actualEndDate;

      this.Stage = ItemStage.Done;
      this.Status = ActivityStatus.Completed;

      this.Save();
    }


    internal void Delete() {
      Assertion.Assert(this.Status != ActivityStatus.Completed,
                       "Deletion is only possible for project items with statuses distinct than completed.");

      this.Status = ActivityStatus.Deleted;

      this.Save();
    }


    public FixedList<ProjectItem> GetBranch() {
      return this.Project.GetBranch(this);
    }


    public Activity GetTemplate() {
      if (this.HasTemplate) {
        return Activity.Parse(this.TemplateId);
      } else {
        return Activity.Empty;
      }
    }


    protected virtual void Load(ProjectItem data) {
      this.Name = data.Name;
      this.Notes = data.Notes;
      this.Theme = data.Theme;

      this.Tag = data.Tag;
      this.ExtensionData = JsonObject.Parse(data.ExtensionData.ToString());

      this.Resource = data.Resource;
      this.TemplateId = data.TemplateId;
    }


    protected virtual void Load(JsonObject data) {
      this.Name = data.GetClean("name", this.Name);
      this.Notes = data.GetClean("notes", this.Notes);
      this.Theme = data.GetClean("theme", this.Theme);

      this.Tag = data.GetClean("tags", this.Tag);

      if (data.Contains("config")) {
        this.ExtensionData.Set("config", data.Slice("config"));
      }

      this.Resource = data.Get<string>("resource", this.Resource);

      this.TemplateId = data.Get<int>("templateId", this.TemplateId);

      this.ProcessID = data.Get<string>("processID", this.ProcessID);
      this.SubprocessID = data.Get<string>("subProcessID", this.SubprocessID);
    }


    protected void LoadDateFields(JsonObject data) {
      if (data.HasValue("estimatedDuration/type")) {
        this.EstimatedDuration = new Duration(data.Get<int>("estimatedDuration/value", 0),
                                              data.Get<DurationType>("estimatedDuration/type", DurationType.Unknown));
      }

      if (data.Contains("plannedEndDate")) {
        this.PlannedEndDate = data.Get("plannedEndDate", ExecutionServer.DateMaxValue);
      }

      if (data.Contains("deadline")) {
        this.Deadline = data.Get("deadline", ExecutionServer.DateMaxValue);
      }

      if (data.Contains("actualStartDate")) {
        this.ActualStartDate = data.Get("actualStartDate", ExecutionServer.DateMaxValue);
      }

      if (data.HasValue("warnDays")) {
        this.ExtensionData.Set("warnDays", data.Get<int>("warnDays"));
      }

      if (data.HasValue("warnType")) {
        this.ExtensionData.Set("warnType", data.Get<string>("warnType"));
      }
    }


    protected override void OnSave() {
      throw Assertion.AssertNoReachThisCode();
    }


    public virtual void Reactivate() {
      Assertion.Assert(this.Status != ActivityStatus.Active,
                      "Reactivation is only possible when status is distinct than active.");

      this.ActualEndDate = ExecutionServer.DateMaxValue;

      this.Stage = ItemStage.InProcess;
      this.Status = ActivityStatus.Active;

      this.Save();
    }


    internal void SetData(ProjectItemStateChange stateChange) {
      if (stateChange.Name != null) {
        this.Name = stateChange.Name;
      }
      if (stateChange.Notes != null) {
        this.Notes = stateChange.Notes;
      }
      if (stateChange.Theme != null) {
        this.Theme = stateChange.Theme;
      }
    }


    internal void SetDeadline(DateTime deadline) {
      this.Deadline = deadline;
    }


    internal void SetParent(ProjectItem parent) {
      Assertion.AssertObject(parent, "parent");
      Assertion.Assert(!parent.Equals(this),
                "A project item can't be parent of itself.");

      if (this.ProjectObjectType.Id != ProjectItemType.TaskType.Id) {
        Assertion.Assert(parent.Position < this.Position,
               $"Wrong operation 0: Parent position {parent.Position} {parent.Name} can not be below of this project item  position {this.Position} {this.Name}.");
      }
      this.Parent = parent;
    }


    internal void SetParentAndPosition(ProjectItem parent, int position) {
      Assertion.AssertObject(parent, "parent");
      Assertion.Assert(!parent.Equals(this),
                       "A project item can't be parent of itself.");
      Assertion.Assert(position >= 0, "position can not be negative.");

      this.Parent = parent;
      this.Position = position;

      this.Save();
    }


    public void SetAndSaveParent(ProjectItem parent) {
      Assertion.AssertObject(parent, "parent");
      Assertion.Assert(!parent.Equals(this),
              "A project item can't be parent of itself.");
      Assertion.Assert(parent.Position < this.Position,
              $"Wrong operation 1: Parent's position can not be below of this project item's position.");

      this.Parent = parent;

      this.Save();
    }


    internal void SetPosition(int position, bool assert = false) {
      Assertion.Assert(position > 0, "position must be greater than zero.");
      if (assert) {
        Assertion.Assert(this.Parent.Position < position,
                         $"Wrong operation 2: Parent position {this.Parent.Name} can not be below of this project item position {this.Name}.");
      }
      this.Position = position;
    }


    internal void SetProcess(string processID, string subprocessID) {
      this.ProcessID = processID;
      this.SubprocessID = subprocessID;
    }


    internal void SetProject(Project project) {
      Assertion.AssertObject(project, "project");

      this.Project = project;
    }


    internal void SetDates(DateTime actualStartDate, DateTime deadline) {
      this.ActualStartDate = actualStartDate;
      this.Deadline = deadline;

      this.Save();
    }


    public virtual void Update(JsonObject data) {
      this.AssertIsValid(data);

      this.Load(data);

      this.Save();
    }

    #endregion Methods

  } // class ProjectItem

} // namespace Empiria.ProjectManagement
