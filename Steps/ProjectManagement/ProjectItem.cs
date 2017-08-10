/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : ProjectItem                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project as a set of well defined activities.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Ontology;

using Empiria.Steps.Legal;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Describes a project as a set of well defined activities.</summary>
  [PartitionedType(typeof(ProjectItemType))]
  public class ProjectItem : BaseObject {

    #region Constructors and parsers

    protected ProjectItem(ProjectItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    internal ProjectItem(Project project, ProjectItemType type, JsonObject data) : base(type) {
      Assertion.AssertObject(project, "project");
      Assertion.AssertObject(data, "data");

      this.Project = project;
      this.AssertIsValid(data);

      this.Load(data);
    }

    static internal ProjectItem Parse(int id) {
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
      this.Project = Project.Parse((int) row["ProjectId"]);
      this.Resource = Resource.Parse((int) row["AssociatedResourceId"]);

      if ((int) row["ProjectItemId"] != -1) {
        this.Parent = ProjectItem.Parse((int) row["ParentId"]);
      } else {
        this.Parent = this;
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public ProjectItemType ProjectItemType {
      get {
        return (ProjectItemType) base.GetEmpiriaType();
      }
    }


    public Project Project {
      get;
      private set;
    }


    [DataField("RelatedProcedureId")]
    public RelatedProcedure RelatedProcedure {
      get;
      private set;
    } = RelatedProcedure.Empty;


    public Resource Resource {
      get;
      private set;
    } = Resource.Empty;


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
    public string EstimatedDuration {
      get;
      private set;
    }


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


    [DataField("ResponsibleId")]
    public Contact Responsible {
      get;
      private set;
    } = Contact.Empty;


    [DataField("RequestedTime")]
    public DateTime RequestedTime {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("RequestedById")]
    public Contact RequestedBy {
      get;
      private set;
    } = Contact.Empty;


    public ProjectItem Parent {
      get;
      private set;
    }


    [DataField("Status", Default = ProjectItemStatus.Inactive)]
    public ProjectItemStatus Status {
      get;
      private set;
    }

    #endregion Public properties

    #region Private methods

    private void AssertIsValid(JsonObject data) {
      //Assertion.AssertObject(data, "data");

      //Validate.HasValue(data, "clauseNo", "Requiero conocer el número de cláusula o anexo.");
      //Validate.HasValue(data, "title", "Necesito el nombre que describe a la cláusula o anexo.");

      //var clauseNo = data.GetClean("clauseNo");

      //if (this.IsNew) {
      //  var clause = this.Contract.TryGetClause((x) => x.Number.Equals(clauseNo));
      //  Validate.AlreadyExists(clause, $"Este contrato ya contiene una cláusula con el número '{clauseNo}'.");
      //} else {
      //  var clause = this.Contract.TryGetClause((x) => x.Number.Equals(clauseNo) &&
      //                                                 x.Id != this.Id);
      //  Validate.AlreadyExists(clause, $"En este contrato ya existe otra cláusula con el número '{clauseNo}'.");
      //}
    }

    private void Load(JsonObject data) {
      this.Resource = Resource.Parse(data.Get<string>("resourceUID"));
      this.Name = data.GetClean("name", this.Name);
      this.Notes = data.GetClean("notes", this.Notes);
      this.EstimatedStart = data.Get<DateTime>("estimatedStart", this.EstimatedStart);
      this.EstimatedEnd = data.Get<DateTime>("estimatedEnd", this.EstimatedEnd);
      this.EstimatedDuration = data.GetClean("estimatedDuration", this.EstimatedDuration);
      this.CompletionProgress = data.Get<decimal>("completionProgress", this.CompletionProgress);
      this.Responsible = Contact.Parse(data.Get<string>("responsibleUID"));
      this.RequestedTime = data.Get<DateTime>("requestedTime", this.RequestedTime);
      this.RequestedBy = Contact.Parse(data.Get<string>("requestedByUID"));
      this.Parent = ProjectItem.Parse(data.Get<int>("parentId"));
    }

    protected override void OnSave() {
      ProjectData.WriteProjectItem(this);
    }

    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Load(data);
    }

    #endregion Private methods

  } // class ProjectItem

} // namespace Empiria.Steps.ProjectManagement
