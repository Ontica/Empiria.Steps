/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : ProjectItem                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Abstract class that describes a project item.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Collections;
using Empiria.DataTypes;
using Empiria.Json;
using Empiria.StateEnums;

using Empiria.Ontology;

using Empiria.Workflow.Definition;

namespace Empiria.ProjectManagement {

  /// <summary>Abstract class that describes a project item.</summary>
  [PartitionedType(typeof(ProjectItemType))]
  public abstract class ProjectItem : BaseObject {

    #region Constructors and parsers

    protected ProjectItem(ProjectItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected internal ProjectItem(ProjectItemType type,
                                   Project project, ProjectItem parent,
                                   JsonObject data) : base(type) {
      Assertion.AssertObject(type, "type");
      Assertion.AssertObject(project, "project");
      Assertion.AssertObject(parent, "parent");
      Assertion.AssertObject(data, "data");

      Assertion.Assert(!project.IsEmptyInstance,
                       "Project can't be the empty instance.");


      this.Project = project;
      this.Parent = parent;
      this.Owner = parent.Owner;

      this.AssertIsValid(data);

      this.Load(data);
    }


    static public ProjectItem Parse(int id) {
      return BaseObject.ParseId<ProjectItem>(id);
    }


    static public ProjectItem Empty {
      get {
        return BaseObject.ParseEmpty<ProjectItem>();
      }
    }


    protected override void OnLoadObjectData(System.Data.DataRow row) {
      if ((int) row["ProjectObjectId"] != -1) {
        this.Parent = ProjectItem.Parse((int) row["ParentId"]);
        this.CreatedFrom = ProjectItem.Parse((int) row["CreatedFromId"]);
      } else {
        this.Parent = this;
        this.CreatedFrom = this;
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public ProjectItemType ProjectObjectType {
      get {
        return (ProjectItemType) base.GetEmpiriaType();
      }
    }


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


    [DataField("OwnerId")]
    public Contact Owner {
      get;
      private set;
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


    protected internal ProjectItem CreatedFrom {
      get;
      private set;
    }


    [DataField("ExtData")]
    protected internal JsonObject ExtensionData {
      get;
      private set;
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


    [DataField("WorkflowObjectId")]
    public WorkflowObject WorkflowObject {
      get;
      private set;
    } = WorkflowObject.Empty;


    #endregion Public properties

    #region Private methods

    protected virtual void AssertIsValid(JsonObject data) {
      Assertion.AssertObject(data, "data");
    }


    public virtual void Close(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.AssertIsValid(data);
      this.Load(data);

      this.EndDate = data.Get<DateTime>("endDate", this.EndDate);
      this.StartDate = this.StartDate == ExecutionServer.DateMaxValue
                            ? this.EndDate : this.StartDate;

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


    protected virtual void Load(JsonObject data) {
      this.Name = data.GetClean("name", this.Name);
      this.Notes = data.GetClean("notes", this.Notes);
      this.RagStatus = data.Get<RAGStatus>("ragStatus", this.RagStatus);

      var tags = data.GetList<string>("tags", false);
      if (tags != null && tags.Count != 0) {
        this.Tags = TagsCollection.Parse(tags);
      }

      this.StartDate = data.Get("startDate", this.StartDate.Date);
      this.TargetDate = data.Get("targetDate", this.TargetDate.Date);
      this.DueDate = data.Get("dueDate", this.DueDate.Date);

      this.EstimatedDuration = Duration.Parse(data.GetClean("estimatedDuration",
                                                            this.EstimatedDuration.ToString()));

      this.WorkflowObject = WorkflowObject.Parse(data.Get<int>("workflowObjectId", -1));

      if (this.Name.Length == 0) {
        this.Name = this.WorkflowObject.Name;
      }

      if (!this.IsEmptyInstance) {
        int parentId = data.Get<int>("parentId", -1);
        this.Parent = parentId != -1 ? ProjectItem.Parse(parentId) : this.Parent;

        this.CreatedFrom = ProjectItem.Empty;
      }
    }


    protected override void OnBeforeSave() {
      if (this.IsNew) {
        this.UID = EmpiriaString.BuildRandomString(6, 24);
      }
    }


    protected override void OnSave() {
      throw Assertion.AssertNoReachThisCode();
    }


    internal void SetPosition(int position) {
      this.Position = position;
    }


    internal void SetDates(DateTime startDate, DateTime dueDate) {
      this.StartDate = startDate;
      this.DueDate = dueDate;

      this.Save();
    }


    public virtual void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.AssertIsValid(data);
      this.Load(data);
    }

    #endregion Private methods

  } // class ProjectItem

} // namespace Empiria.ProjectManagement
