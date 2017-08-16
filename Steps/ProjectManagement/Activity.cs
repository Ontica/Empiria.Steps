/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Activity                                         License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project activity.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Json;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Describes a project activity.</summary>
  public class Activity : ProjectObject {

    #region Fields

    private Lazy<List<Activity>> subactivitiesList = null;
    private Lazy<List<Task>> tasksList = null;

    #endregion Fields

    #region Constructors and parsers

    protected Activity() : this(ProjectObjectType.ActivityType) {

    }


    protected Activity(ProjectObjectType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected internal Activity(ProjectObject parent, JsonObject data) :
                                base(ProjectObjectType.ActivityType, parent, data) {
      this.AssertIsValid(data);

      this.Load(data);
    }

    static public new Activity Parse(int id) {
      return BaseObject.ParseId<Activity>(id);
    }

    static public new Activity Empty {
      get {
        return BaseObject.ParseEmpty<Activity>();
      }
    }

    protected override void OnInitialize() {
      subactivitiesList = new Lazy<List<Activity>>(() => ProjectData.GetChildrenActivities(this));
      tasksList = new Lazy<List<Task>>(() => ProjectData.GetProjectActivityTasks(this));
    }

    #endregion Constructors and parsers

    #region Public properties

    public ProjectType ProjectType {
      get {
        return (ProjectType) base.GetEmpiriaType();
      }
    }

    [DataField("ResourceId")]
    public Resource Resource {
      get;
      private set;
    } = Resource.Empty;


    [DataField("ResponsibleId")]
    public Contact Responsible {
      get;
      private set;
    }


    [DataField("RequestedTime")]
    public DateTime RequestedTime {
      get;
      private set;
    } = ExecutionServer.DateMinValue;



    public Contact RequestedBy {
      get;
      private set;
    } = Contact.Empty;


    #endregion Public properties

    #region Properties related to the activity structure


    [DataField("BaseProjectId")]
    public Project Project {
      get;
      private set;
    }


    public FixedList<Activity> Subactivities {
      get {
        return subactivitiesList.Value.ToFixedList();
      }
    }


    public new ProjectObject Parent {
      get {
        return base.Parent;
      }
    }


    public FixedList<Task> Tasks {
      get {
        return tasksList.Value.ToFixedList();
      }
    }


    #endregion Properties related to the activity structure

    #region Public methods

    protected override void AssertIsValid(JsonObject data) {
      base.AssertIsValid(data);
    }

    protected override void Load(JsonObject data) {
      base.Load(data);

      this.Resource = Resource.Parse(data.Get<string>("resourceUID"));
      this.Responsible = Contact.Parse(data.Get<string>("responsibleUID"));
      this.RequestedTime = data.Get<DateTime>("requestedTime", this.RequestedTime);
      this.RequestedBy = Contact.Parse(data.Get<string>("requestedByUID"));
    }

    public Activity AddActivity(JsonObject data) {
      var activity = new Activity(this, data);

      activity.Save();

      subactivitiesList.Value.Add(activity);

      return activity;
    }

    protected override void OnSave() {
      ProjectData.WriteActivity(this);
    }

    #endregion Public methods

  } // class Activity

} // namespace Empiria.Steps.ProjectManagement
