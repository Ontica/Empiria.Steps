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
using Empiria.Json;

using Empiria.Steps.Legal;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Describes a project as a set of well defined activities.</summary>
  public class Project : ProjectObject {

    #region Fields

    private Lazy<List<Activity>> activitiesList = null;

    #endregion Fields

    #region Constructors and parsers

    protected Project() : this(ProjectObjectType.ProjectType) {

    }

    protected Project(ProjectObjectType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static internal new Project Parse(int id) {
      return BaseObject.ParseId<Project>(id);
    }

    static public Project Parse(string uid) {
      return BaseObject.ParseKey<Project>(uid);
    }

    static public new Project Empty {
      get {
        return BaseObject.ParseEmpty<Project>();
      }
    }

    static public FixedList<Project> GetList(string filter = "") {
      var ownerOrManager = Contact.Parse(51);

      var list = ProjectData.GetProjects(ownerOrManager);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list.ToFixedList();
    }

    protected override void OnInitialize() {
      activitiesList = new Lazy<List<Activity>>(() => ProjectData.GetChildrenActivities(this));
    }

    protected override void OnLoadObjectData(System.Data.DataRow row) {
      //this.Contract = Contract.Parse((int) row["ContractId"]);
      //this.Resource = Resource.Parse((int) row["ResourceId"]);
    }

    #endregion Constructors and parsers

    #region Public properties


    [DataField("ResponsibleId")]
    public Contact Manager {
      get;
      private set;
    }


    [DataField("ExtData.ContractId")]
    public Contract Contract {
      get;
      private set;
    } = Contract.Empty;


    //[DataField("Deadline")]
    public DateTime Deadline {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("ResourceId")]
    public Resource Resource {
      get;
      private set;
    } = Resource.Empty;


    #endregion Public properties

    #region Properties related to the project's structure

    public FixedList<Activity> Activities {
      get {
        return activitiesList.Value.ToFixedList();
      }
    }

    public FixedList<Activity> GetAllActivities() {
      return ProjectData.GetAllProjectActivities(this);
    }

    #endregion Properties related to the project's structure

    #region Properties related to project's participants

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

    #endregion Properties related to project's participants

    #region Public methods

    public ProjectObject AddActivity(JsonObject data) {
      var activity = new Activity(this, data);

      activity.Save();

      activitiesList.Value.Add(activity);

      return activity;
    }

    protected override void OnSave() {
      ProjectData.WriteProject(this);
    }

    #endregion Public methods

  } // class Project

} // namespace Empiria.Steps.ProjectManagement
