/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : ProjectObject                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project as a set of well defined activities.                                       *
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

  /// <summary>Describes a project as a set of well defined activities.</summary>
  [PartitionedType(typeof(ProjectObjectType))]
  public class ProjectObject : BaseObject {

    #region Constructors and parsers

    protected ProjectObject(ProjectObjectType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected internal ProjectObject(ProjectObjectType type,
                                     ProjectObject parent, JsonObject data) : base(type) {
      Assertion.AssertObject(type, "type");
      Assertion.AssertObject(parent, "parent");
      Assertion.AssertObject(data, "data");

      this.Parent = parent;
      this.Owner = parent.Owner;

      this.AssertIsValid(data);

      this.Load(data);
    }

    static public ProjectObject Parse(int id) {
      return BaseObject.ParseId<ProjectObject>(id);
    }

    static public ProjectObject Empty {
      get {
        return BaseObject.ParseEmpty<ProjectObject>();
      }
    }

    protected override void OnLoadObjectData(System.Data.DataRow row) {
      if ((int) row["ProjectObjectId"] != -1) {
        this.Parent = ProjectObject.Parse((int) row["ParentId"]);
        this.CreatedFrom = ProjectObject.Parse((int) row["CreatedFromId"]);
      } else {
        this.Parent = this;
        this.CreatedFrom = this;
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public ProjectObjectType ProjectObjectType {
      get {
        return (ProjectObjectType) base.GetEmpiriaType();
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

    [DataField("ItemOrdering")]
    public int Ordering {
      get;
      private set;
    }


    [DataField("OwnerId")]
    public Contact Owner {
      get;
      private set;
    }


    protected ProjectObject Parent {
      get;
      private set;
    }


    protected internal ProjectObject CreatedFrom {
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


      //FixedList<WorkflowObject> nextSteps = this.WorkflowObject.NextSteps();

      //foreach (var step in nextSteps) {

      //}

      //int daysCount = 0;

      //foreach (var step in nextSteps) {
      //  if (step is Process) {

      //  } else if (step is ProcessActivity) {
      //    this.Add
      //  }

      //}

      //    var stepAsJson = this.ConvertStepToJson(baseActivity, step, daysCount);

      //  baseActivity.AddActivity(stepAsJson);

      //  daysCount += step.EstimatedDuration.Value;
      //}

      //baseActivity.SetDates(startDate, startDate.AddDays(daysCount));
    }

    protected virtual void Load(JsonObject data) {
      this.Name = data.GetClean("name", this.Name);
      this.Notes = data.GetClean("notes", this.Notes);
      this.RagStatus = data.Get<RAGStatus>("ragStatus", this.RagStatus);

      var tags = data.GetList<string>("tags", false);
      if (tags == null || tags.Count == 0) {
        // no-op
      } else {
        this.Tags = TagsCollection.Parse(tags);
      }
      this.StartDate = data.Get<DateTime>("startDate", this.StartDate.Date);
      this.TargetDate = data.Get<DateTime>("targetDate", this.TargetDate.Date);
      this.DueDate = data.Get<DateTime>("dueDate", this.DueDate.Date);

      this.EstimatedDuration = Duration.Parse(data.GetClean("estimatedDuration",
                                                            this.EstimatedDuration.ToString()));

      this.WorkflowObject = WorkflowObject.Parse(data.Get<int>("workflowObjectId", -1));
      if (this.Name.Length == 0) {
        this.Name = this.WorkflowObject.Name;
      }
      if (!this.IsEmptyInstance) {
        int parentId = data.Get<int>("parentId", -1);
        this.Parent = parentId != -1 ? ProjectObject.Parse(parentId) : this.Parent;

        int createdFromId = data.Get<int>("createdFromId", -1);
        this.CreatedFrom = createdFromId != -1 ? ProjectObject.Parse(createdFromId) : this.Parent;
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

  } // class ProjectObject

} // namespace Empiria.ProjectManagement
