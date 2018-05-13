/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Domain services                       *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Aggregate root                        *
*  Type     : ActivityController                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes a project as a tree of activities.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Collections;
using Empiria.Contacts;
using Empiria.DataTypes;
using Empiria.Json;
using Empiria.StateEnums;

using Empiria.Workflow.Definition;

using Empiria.ProjectManagement.Resources;

  namespace Empiria.ProjectManagement {

  /// <summary>Describes a project as a tree of activities.</summary>
  public class Project : BaseObject {

    #region Fields

    private Lazy<ProjectItemsTree> itemsTree = null;
    private readonly object __treeLock = new object();

    #endregion Fields

    #region Constructors and parsers

    protected Project() {
      // Required by Empiria Framework.
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
      var ownerOrManager = Contact.Parse(51);

      var list = ProjectData.GetProjects(ownerOrManager);

      return list.ToFixedList();
    }


    protected override void OnInitialize() {
      itemsTree = new Lazy<ProjectItemsTree>(() => ProjectItemsTree.Load(this));
    }

    #endregion Constructors and parsers

    #region Public properties

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


    [DataField("ExtData")]
    protected internal JsonObject ExtensionData {
      get;
      private set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Name, this.Tags.ToString());
      }
    }


    [DataField("ItemPosition")]
    public int Position {
      get;
      private set;
    }


    public int Level {
      get {
        if (!this.Parent.IsEmptyInstance) {
          return this.Parent.Level + 1;
        } else {
          return 1;
        }
      }
    }


    internal protected Project Parent {
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


    [DataField("OwnerId")]
    public Contact Owner {
      get;
      private set;
    }


    [DataField("Status", Default = ActivityStatus.Pending)]
    public ActivityStatus Status {
      get;
      private set;
    }


    [DataField("WorkflowObjectId")]
    public WorkflowObject WorkflowObject {
      get;
      private set;
    } = WorkflowObject.Empty;


    [DataField("ResponsibleId")]
    public Contact Responsible {
      get;
      private set;
    }


    [DataField("ResourceId")]
    public Resource Resource {
      get;
      private set;
    } = Resource.Empty;


    #endregion Public properties


    #region Project participants

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


    internal FixedList<Contact> GetInvolvedContacts() {
      return ProjectData.GetProjectInvolvedContacts(this);
    }


    #endregion Project participants

    #region Project items

    public Activity AddActivity(JsonObject data) {
      Assertion.AssertObject(data, "data");

      lock (__treeLock) {
        return this.Items.AddActivity(data);
      }
    }


    private ProjectItemsTree Items {
      get {
        return this.itemsTree.Value;
      }
    }


    public Activity GetActivity(string activityUID) {
      Assertion.AssertObject(activityUID, "activityUID");

      return this.Items.GetActivity(activityUID);
    }


    public FixedList<ProjectItem> GetItems() {
      return this.Items.ToFixedList();
    }

    public FixedList<ProjectItem> GetItems(ActivityFilter filter = null,
                                           ActivityOrder orderBy = ActivityOrder.Default) {
      if (filter == null) {
        filter = new ActivityFilter();
      }

      return ProjectData.GetProjectActivities(this, filter, orderBy)
                        .ToFixedList();
    }


    public void RemoveActivity(Activity activity) {
      Assertion.AssertObject(activity, "activity");

      lock (__treeLock) {
        this.Items.RemoveActivity(activity);
      }
    }


    internal void UpdateItemParentAndPosition(Activity activity, JsonObject data) {
      Assertion.AssertObject(activity, "activity");
      Assertion.AssertObject(data, "data");

      lock (__treeLock) {
        this.Items.UpdateItemParentAndPosition(activity, data);
      }
    }

    #endregion Project items

    #region Public methods

    protected override void OnSave() {
      ProjectData.WriteProject(this);
    }

    #endregion Public methods

  } // class Project

} // namespace Empiria.ProjectManagement
