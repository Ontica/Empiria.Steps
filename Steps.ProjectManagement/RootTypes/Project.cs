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

using Empiria.Workflow.Definition;

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

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

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

    #endregion Project participants

    #region Public methods

    public ProjectObject AddItem(JsonObject data) {
      Assertion.AssertObject(data, "data");

      ProjectObject activity = null;

      int parentId = data.Get<int>("parentId", -1);
      var workflowObject = WorkflowObject.Parse(data.Get<int>("workflowObjectId", -1));

      if (parentId == -1 || workflowObject.WorkflowObjectType == WorkflowObjectType.Process) {
        activity = new Summary(this, data);
      } else {
        var parent = this.Items.Find((x) => x.Id == parentId);

        if (parent != null && workflowObject.WorkflowObjectType == WorkflowObjectType.Process) {
          activity = new Summary(parent, data);
        } else if (parent != null && workflowObject.WorkflowObjectType == WorkflowObjectType.Activity) {
          activity = new Activity(parent, data);
        } else {
          throw new ValidationException("UnrecognizedActivityParent",
                                        $"Invalid activity parent ({parentId}) for project ({this.Id}).");
        }
      }
      activity.Save();

      itemsList.Value.Add(activity);

      return activity;
    }

    protected override void OnSave() {
      ProjectData.WriteProject(this);
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

    #endregion Public methods

  } // class Project

} // namespace Empiria.ProjectManagement
