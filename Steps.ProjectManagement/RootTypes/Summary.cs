/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : Summary                                          License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project activity.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Json;

using Empiria.ProjectManagement.Resources;

namespace Empiria.ProjectManagement {

  /// <summary>Describes a project activity.</summary>
  public class Summary : ProjectObject {

    #region Fields

    private Lazy<List<ProjectObject>> itemsList = null;

    #endregion Fields

    #region Constructors and parsers

    protected Summary() : this(ProjectObjectType.SummaryType) {

    }


    protected Summary(ProjectObjectType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected internal Summary(ProjectObject parent, JsonObject data) :
                               base(ProjectObjectType.SummaryType, parent, data) {
      if (parent is Activity) {
        this.Project = ((Activity) parent).Project;
      } else if (parent is Summary) {
        this.Project = ((Summary) parent).Project;
      } else {
        this.Project = (Project) parent;
      }
      this.AssertIsValid(data);
      this.Load(data);
    }

    static public Activity Parse(string uid) {
      return BaseObject.ParseKey<Activity>(uid);
    }

    static internal new Activity Parse(int id) {
      return BaseObject.ParseId<Activity>(id);
    }

    static public new Activity Empty {
      get {
        return BaseObject.ParseEmpty<Activity>();
      }
    }

    protected override void OnInitialize() {
      itemsList = new Lazy<List<ProjectObject>>(() => ProjectData.GetAllActivities(this));
    }

    #endregion Constructors and parsers

    #region Public properties


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


    public FixedList<ProjectObject> Subactivities {
      get {
        return itemsList.Value.ToFixedList();
      }
    }


    public new ProjectObject Parent {
      get {
        return base.Parent;
      }
    }

    public int Level {
      get {
        if (this.Parent.ProjectObjectType == ProjectObjectType.SummaryType) {
          return ((Summary) this.Parent).Level + 1;
        } else {
          return 1;
        }
      }
    }


    #endregion Properties related to the activity structure

    #region Public methods

    protected override void AssertIsValid(JsonObject data) {
      base.AssertIsValid(data);
    }

    protected override void Load(JsonObject data) {
      base.Load(data);

      this.Resource = Resource.Parse(data.Get("resourceUID", "Undefined"));
      this.Responsible = Contact.Parse(data.Get("responsibleUID", "Undefined"));
      this.RequestedTime = data.Get<DateTime>("requestedTime", this.RequestedTime);
      this.RequestedBy = Contact.Parse(data.Get("requestedByUID", "Undefined"));
    }


    public Activity AddActivity(JsonObject data) {
      var activity = new Activity(this, data);

      activity.Save();

      itemsList.Value.Add(activity);

      return activity;
    }


    protected override void OnSave() {
      ProjectData.WriteSummary(this);
    }

    #endregion Public methods

  } // class Summary

} // namespace Empiria.ProjectManagement
