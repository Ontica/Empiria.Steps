/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Project                                          License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project as a set of well defined activities.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Ontology;

using Empiria.Steps.Legal;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Describes a project as a set of well defined activities.</summary>
  [PartitionedType(typeof(ProjectType))]
  public class Project : BaseObject {

    #region Fields

    private Lazy<List<ProjectItem>> activitiesList = null;

    #endregion Fields

    #region Constructors and parsers

    protected Project(ProjectType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
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
      filter = "OwnerId = 51";

      var list = BaseObject.GetList<Project>(filter);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list.ToFixedList();
    }

    protected override void OnInitialize() {
      activitiesList = new Lazy<List<ProjectItem>>(() => ProjectData.GetProjectActivities(this));
    }

    protected override void OnLoadObjectData(System.Data.DataRow row) {
      this.Contract = Contract.Parse((int) row["ContractId"]);
    }

    #endregion Constructors and parsers

    #region Public properties

    public ProjectType ProjectType {
      get {
        return (ProjectType) base.GetEmpiriaType();
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


    [DataField("OwnerId")]
    public Contact Owner {
      get;
      private set;
    }


    [DataField("ManagerId")]
    public Contact Manager {
      get;
      private set;
    }

    public Contract Contract {
      get;
      private set;
    } = Contract.Empty;


    //[DataField("AppliedToId")]
    //public Resource AppliedTo {
    //  get;
    //  private set;
    //} = Resource.Empty;


    [DataField("Deadline")]
    public DateTime Deadline {
      get;
      private set;
    }


    [DataField("ProjectStatus")]
    public string Status {
      get;
      private set;
    }

    #endregion Public properties

    #region Public properties related to the project's structure

    public FixedList<ProjectItem> Activities {
      get {
        return activitiesList.Value.ToFixedList();
      }
    }

    public Project Parent {
      get;
      private set;
    }


    public Project[] Subprojects {
      get;
      private set;
    } = new Project[0];


    #endregion Public properties related to the project's structure


  } // class Project

} // namespace Empiria.Steps.ProjectManagement
