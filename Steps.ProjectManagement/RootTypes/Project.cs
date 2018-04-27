/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : Project                                          License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project as a set of well defined activities.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Json;

using Empiria.ProjectManagement.Resources;

namespace Empiria.ProjectManagement {

  /// <summary>Describes a project as a set of well defined activities.</summary>
  public class Project : ProjectObject {

    #region Fields

    private Lazy<List<ProjectObject>> itemsList = null;

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

      return list.ToFixedList();
    }

    protected override void OnInitialize() {
      itemsList = new Lazy<List<ProjectObject>>(() => ProjectData.GetProjectActivities(this));
    }

    #endregion Constructors and parsers

    #region Public properties


    [DataField("ResponsibleId")]
    public Contact Manager {
      get;
      private set;
    }


    [DataField("ResourceId")]
    public Resource Resource {
      get;
      private set;
    } = Resource.Empty;


    #endregion Public properties

    #region Project structure

    public FixedList<ProjectObject> Items {
      get {
        return itemsList.Value.ToFixedList();
      }
    }

    public FixedList<ProjectObject> GetActivities(ActivityFilter filter = null,
                                                  ActivityOrder orderBy = ActivityOrder.Default) {
      if (filter == null) {
        filter = new ActivityFilter();
      }

      return ProjectData.GetProjectActivities(this, filter, orderBy)
                        .ToFixedList();
    }

    #endregion Project structure

    #region Project participants

    public FixedList<Contact> Responsibles {
      get {
        return ProjectContactsData.GetProjectResponsibles(this);
      }
    }

    public FixedList<Contact> Requesters {
      get {
        return ProjectContactsData.GetProjectRequesters(this);
      }
    }

    public FixedList<Contact> TaskManagers {
      get {
        return ProjectContactsData.GetProjectTaskManagers(this);
      }
    }

    internal FixedList<Contact> GetInvolvedContacts() {
      var list = new Contact[6];

      list[0] = Contact.Parse(2);
      list[1] = Contact.Parse(4);
      list[2] = Contact.Parse(8);
      list[3] = Contact.Parse(9);
      list[4] = Contact.Parse(10);
      list[5] = Contact.Parse(11);

      return new FixedList<Contact>(list);
    }

    #endregion Project participants

    #region Public methods

    public Activity AddActivity(JsonObject data) {
      Assertion.AssertObject(data, "data");
      ProjectObject parent = GetParentFromJson(data);
      int position = GetPositionFromJson(data);

      var activity = new Activity(parent, data);

      activity.SetPosition(position);

      activity.Save();

      if (activity.Position <= itemsList.Value.Count) {
        ProjectData.UpdatePositionsStartingFrom(activity);

        itemsList = new Lazy<List<ProjectObject>>(() => ProjectData.GetProjectActivities(this));

      } else {
        itemsList.Value.Add(activity);
      }

      return activity;
    }


    public Activity GetActivity(string activityUID) {
      Assertion.AssertObject(activityUID, "activityUID");

      Activity activity = this.Items.Find((x) => x.UID == activityUID && x is Activity) as Activity;

      Assertion.AssertObject(activity,
                             $"Activity with uid ({activityUID}) is not part of project {this.Name}.");

      return activity;
    }

    public void RemoveActivity(Activity activity) {
      Assertion.AssertObject(activity, "activity");
      Assertion.Assert(this.Items.Contains(activity),
                      $"Activity {activity.Name} doesn't belong to this project.");

      activity.Delete();

      ProjectData.UpdatePositionsStartingFrom(activity);

      itemsList = new Lazy<List<ProjectObject>>(() => ProjectData.GetProjectActivities(this));
    }


    public Summary AddSummary(JsonObject data) {
      Assertion.AssertObject(data, "data");

      ProjectObject parent = GetParentFromJson(data);

      Summary summary = new Summary(this, data);

      summary.Save();

      itemsList.Value.Add(summary);

      return summary;
    }


    protected override void OnSave() {
      ProjectData.WriteProject(this);
    }

    #endregion Public methods

    #region Private methods

    private ProjectObject GetParentFromJson(JsonObject data) {
      ProjectObject parent = data.Get<ProjectObject>("parentUID", ProjectObject.Empty);

      if (parent.IsEmptyInstance) {
        return this;
      }

      Assertion.Assert(this.Items.Contains(parent),
                       new ValidationException("UnrecognizedActivityParent",
                                               $"Invalid activity parent ({parent.Name}) for project ({this.UID})."));
      return parent;
    }


    private int GetPositionFromJson(JsonObject data) {
      int position = data.Get("position", -1);

      if (position == -1) {
        return this.Items.Count + 1;
      }


      Assertion.Assert(1 <= position && position <= this.Items.Count + 1,
                       new ValidationException("PositionOutOfIndex",
                                               $"Invalid activity position ({position}) for project ({this.UID})."));

      return position;
    }

    #endregion Private methods

  } // class Project

} // namespace Empiria.ProjectManagement
