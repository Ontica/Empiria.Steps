/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : ProjectObject                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project as a set of well defined activities.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Ontology;

using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Steps.ProjectManagement {

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
      } else {
        this.Parent = this;
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


    [DataField("EstimatedStart")]
    public DateTime EstimatedStart {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("EstimatedEnd")]
    public DateTime EstimatedEnd {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("EstimatedDuration")]
    public Duration EstimatedDuration {
      get;
      private set;
    } = Duration.Empty;


    [DataField("ActualStart")]
    public DateTime ActualStart {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("ActualEnd")]
    public DateTime ActualEnd {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("CompletionProgress")]
    public decimal CompletionProgress {
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


    [DataField("ExtData")]
    protected internal JsonObject ExtensionData {
      get;
      private set;
    }


    [DataField("Status", Default = ProjectObjectStatus.Inactive)]
    public ProjectObjectStatus Status {
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

    protected virtual void Load(JsonObject data) {
      this.Name = data.GetClean("name", this.Name);
      this.Notes = data.GetClean("notes", this.Notes);
      this.EstimatedStart = data.Get<DateTime>("estimatedStart", this.EstimatedStart);
      this.EstimatedEnd = data.Get<DateTime>("estimatedEnd", this.EstimatedEnd);
      this.EstimatedDuration = Duration.Parse(data.GetClean("estimatedDuration", this.EstimatedDuration.ToString()));
      this.CompletionProgress = data.Get<decimal>("completionProgress", this.CompletionProgress);
      this.WorkflowObject = WorkflowObject.Parse(data.Get<int>("workflowObjectId", -1));
      if (!this.IsEmptyInstance) {
        int parentId = data.Get<int>("parentId", -1);
        this.Parent = parentId != -1 ? ProjectObject.Parse(parentId) : this.Parent;
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

    internal void SetEstimatedDates(DateTime startDate, DateTime endDate) {
      this.EstimatedStart = startDate;
      this.EstimatedEnd = endDate;

      this.Save();
    }

    public virtual void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.AssertIsValid(data);
      this.Load(data);
    }

    #endregion Private methods

  } // class ProjectObject

} // namespace Empiria.Steps.ProjectManagement
