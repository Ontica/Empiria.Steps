/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : ProjectItem                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project as a set of well defined activities.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
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

    protected override void OnInitialize() {
     // this.Contract = new Lazy<Contract>(() => Contract.Parse());
    }

    protected override void OnLoadObjectData(System.Data.DataRow row) {
      this.Project = Project.Parse((int) row["ProjectId"]);
      this.Resource = Resource.Parse((int) row["AssociatedResourceId"]);

      if (this.IsEmptyInstance) {
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
    }

    public Resource Resource {
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
    }

    [DataField("EstimatedEnd")]
    public DateTime EstimatedEnd {
      get;
      private set;
    }

    [DataField("EstimatedDuration")]
    public string EstimatedDuration {
      get;
      private set;
    }

    [DataField("ActualStart")]
    public DateTime ActualStart {
      get;
      private set;
    }

    [DataField("ActualEnd")]
    public DateTime ActualEnd {
      get;
      private set;
    }

    [DataField("CompletionProgress")]
    public decimal CompletionProgress {
      get;
      private set;
    }

    [DataField("ResponsibleId")]
    public Contact Responsible {
      get;
      private set;
    }

    [DataField("RequestedTime")]
    public DateTime RequestedTime {
      get;
      private set;
    }

    [DataField("RequestedById")]
    public Contact RequestedById {
      get;
      private set;
    }

    public ProjectItem Parent {
      get;
      private set;
    }

    [DataField("Status")]
    public string Status {
      get;
      private set;
    }

    #endregion Public properties

  } // class ProjectItem

} // namespace Empiria.Steps.ProjectManagement
