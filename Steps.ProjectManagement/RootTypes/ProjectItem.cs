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
using Empiria.Collections;
using Empiria.DataTypes;
using Empiria.Json;
using Empiria.Ontology;
using Empiria.ProjectManagement.Resources;
using Empiria.StateEnums;

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


    [DataField("Tags")]
    public TagsCollection Tags {
      get;
      private set;
    } = TagsCollection.Empty;


    [DataField("ResourceId")]
    public Resource Resource {
      get;
      private set;
    } = Resource.Empty;


    [DataField("RagStatus", Default = RAGStatus.NoColor)]
    public RAGStatus RagStatus {
      get;
      private set;
    }


    protected internal JsonObject ExtensionData {
      get;
      private set;
    } = new JsonObject();


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
        return this.ExtensionData.Get("warnType", "DefaultConfig");
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


    #endregion Public properties

    #region Private methods

    protected virtual void AssertIsValid(JsonObject data) {
      Assertion.AssertObject(data, "data");
    }


    public virtual void Close(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.AssertIsValid(data);
      this.Load(data);

      this.ActualEndDate = data.Get("endDate", this.ActualEndDate);
      this.ActualStartDate = this.ActualStartDate == ExecutionServer.DateMaxValue
                            ? this.ActualEndDate : this.ActualStartDate;

      this.Stage = ItemStage.Done;
      this.Status = ActivityStatus.Completed;

      this.Save();
    }


    internal void Delete() {
      Assertion.Assert(this.Status == ActivityStatus.Pending,
                       "Deletion is only possible to project items in pending status.");

      this.Status = ActivityStatus.Deleted;

      this.Save();
    }


    public FixedList<ProjectItem> GetBranch() {
      return this.Project.GetBranch(this);
    }


    protected virtual void Load(ProjectItem data) {
      this.Name = data.Name;
      this.Notes = data.Notes;

      this.Tags = data.Tags;
      this.ExtensionData = JsonObject.Parse(data.ExtensionData.ToString());
      this.Resource = data.Resource;
      this.TemplateId = data.TemplateId;
    }


    protected virtual void Load(JsonObject data) {
      this.Name = data.GetClean("name", this.Name);
      this.Notes = data.GetClean("notes", this.Notes);
      this.RagStatus = data.Get("ragStatus", this.RagStatus);

      var tags = data.GetList<string>("tags", false);
      if (tags != null && tags.Count != 0) {
        this.Tags = TagsCollection.Parse(tags);
      }

      if (data.Contains("config")) {
        this.ExtensionData.Set("config", data.Slice("config"));
      }

      this.Resource = data.Get<Resource>("resourceUID", this.Resource);

      this.TemplateId = data.Get<int>("templateId", this.TemplateId);
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

      if (data.Contains("actualEndDate")) {
        this.ActualEndDate = data.Get("actualEndDate", ExecutionServer.DateMaxValue);
      }

    //  if (data.HasValue("warnDays")) {
        this.ExtensionData.Set("warnDays", data.Get<int>("warnDays", 0));
  //    }
 //     if (data.HasValue("warnType")) {
        this.ExtensionData.Set("warnType", data.Get<string>("warnType", "DefaultConfig"));
  //    }
    }


    protected override void OnSave() {
      throw Assertion.AssertNoReachThisCode();
    }


    internal void SetParent(ProjectItem parent) {
      Assertion.AssertObject(parent, "parent");
      Assertion.Assert(!parent.Equals(this),
                      "A project item can't be parent of itself.");

      this.Parent = parent;
    }

    public void SetAndSaveParent(ProjectItem parent) {
      Assertion.AssertObject(parent, "parent");
      Assertion.Assert(!parent.Equals(this),
                      "A project item can't be parent of itself.");

      this.Parent = parent;

      this.Save();
    }


    internal void SetPosition(int position) {
      Assertion.Assert(position > 0, "position must be greater than zero.");

      this.Position = position;
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

    #endregion Private methods

    } // class ProjectItem

} // namespace Empiria.ProjectManagement
