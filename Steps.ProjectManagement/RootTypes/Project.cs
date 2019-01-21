/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Domain services                       *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Aggregate root                        *
*  Type     : Project                                      License   : Please read LICENSE.txt file          *
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

    static public FixedList<Project> GetTemplatesList(string filter = "") {
      var ownerOrManager = Contact.Parse(51);

      var list = ProjectData.GetTemplates(ownerOrManager);

      return list.ToFixedList();
    }


    static public FixedList<ProjectItem> GetEventsList(string filter = "") {
      var ownerOrManager = Contact.Parse(51);

      return ProjectData.GetEvents(ownerOrManager);
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


    public bool IsTemplate {
      get {
        return this.ExtensionData.Contains("template");
      }
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


    [DataField("ActualStartDate")]
    public DateTime ActualStartDate {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("ActualEndDate")]
    public DateTime ActualEndDate {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("PlannedEndDate")]
    public DateTime PlannedEndDate {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    [DataField("Deadline")]
    public DateTime Deadline {
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


    public Activity CopyActivity(Activity activity, JsonObject bodyAsJson) {
      Assertion.AssertObject(activity, "activity");

      lock (__treeLock) {
        return this.Items.CopyActivity(activity);
      }
    }


    public void DeleteActivity(Activity activity) {
      Assertion.AssertObject(activity, "activity");

      lock (__treeLock) {
        this.Items.DeleteActivity(activity);
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


    public FixedList<ProjectItem> GetBranch(ProjectItem root) {
      Assertion.AssertObject(root, "root");

      return this.Items.GetBranch(root);
    }


    public FixedList<ProjectItem> GetItems() {
      return this.Items.ToFixedList();
    }


    public Activity MoveActivity(Activity activity, JsonObject options = null) {
      Assertion.AssertObject(activity, "activity");

      lock (__treeLock) {
        return this.Items.MoveActivity(activity);
      }
    }


    internal void RemoveBranch(Activity root) {
      Assertion.AssertObject(root, "root");

      lock (__treeLock) {
        this.Items.RemoveBranch(root);
      }
    }


    internal void UpdateItemParentAndPosition(ProjectItem projectItem, JsonObject data) {
      Assertion.AssertObject(projectItem, "projectItem");
      Assertion.AssertObject(data, "data");

      lock (__treeLock) {
        this.Items.UpdateItemParentAndPosition(projectItem, data);
      }
    }

    #endregion Project items

    #region Public methods

    protected override void OnSave() {
      ProjectData.WriteProject(this);
    }


    internal void Refresh() {
      itemsTree.Value.RefreshPositions();

      itemsTree = new Lazy<ProjectItemsTree>(() => ProjectItemsTree.Load(this));
    }

    #endregion Public methods

  } // class Project

} // namespace Empiria.ProjectManagement
