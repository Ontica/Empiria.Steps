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
      var list = ProjectData.GetProjects();

      return list.ToFixedList();
    }


    static public FixedList<Project> GetTemplatesList(string filter = "") {
      var list = ProjectData.GetTemplates();

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list.ToFixedList();
    }


    static public FixedList<string> ThemesList {
      get {
        var list = GeneralList.Parse("ProjectManagement.Themes.List");

        FixedList<string> items = list.GetItems<string>();

        items.Sort((x, y) => x.CompareTo(y));

        return items;
      }
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



    [DataField("ExtData")]
    protected internal JsonObject ExtensionData {
      get;
      private set;
    }


    internal string TemplatesList {
      get {
        return this.ExtensionData.Get("templatesList", String.Empty);
      }
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


    [DataField("Resource")]
    public string Resource {
      get;
      private set;
    } = "";


    #endregion Public properties


    #region Project participants

    public FixedList<Contact> Responsibles {
      get {
        return ProjectData.GetProjectAssignees(this);
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


    public Activity CopyActivity(Activity activity) {
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


    public Activity InsertActivity(JsonObject data, int position) {
      Assertion.AssertObject(data, "data");

      lock (__treeLock) {
        return this.Items.InsertActivity(data, position);
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


    public FixedList<ProjectItem> GetInProcessList(ProjectItem projectItem) {
      return this.Items.GetInProcessList(projectItem);
    }


    public FixedList<ProjectItem> GetItems() {
      return this.Items.ToFixedList();
    }


    public Activity MoveActivity(Activity activity) {
      Assertion.AssertObject(activity, "activity");

      lock (__treeLock) {
        return this.Items.MoveActivity(activity);
      }
    }


    public ProjectItem MoveTo(ProjectItem item, TreeItemInsertionRule insertionRule,
                              ProjectItem insertionPoint = null, int relativePosition = -1) {
      Assertion.AssertObject(item, "item");

      lock (__treeLock) {
        return this.Items.MoveToInsertionPoint(item, insertionRule, insertionPoint, relativePosition);
      }
    }


    public ProjectItem ChangeParentKeepingPosition(ProjectItem activity, ProjectItem newParent) {
      Assertion.AssertObject(activity, "activity");
      Assertion.AssertObject(newParent, "newParent");

      lock (__treeLock) {
        return this.Items.ChangeParentKeepingPosition(activity, newParent);
      }
    }


    public ProjectItem ChangePosition(ProjectItem activity, int newPosition) {
      Assertion.AssertObject(activity, "activity");

      lock (__treeLock) {
        return this.Items.ChangePosition(activity, newPosition);
      }
    }


    internal void RemoveBranch(Activity root) {
      Assertion.AssertObject(root, "root");

      lock (__treeLock) {
        this.Items.RemoveBranch(root);
      }
    }


    #endregion Project items

    #region Public methods


    public FixedList<ProjectItem> GetEventsList(string filter = "") {
      return ProjectData.GetEvents(this, filter);
    }


    protected override void OnSave() {
      ProjectData.WriteProject(this);
    }


    public void Refresh() {
      //if (itemsTree.IsValueCreated) {

      //}
      itemsTree = new Lazy<ProjectItemsTree>(() => ProjectItemsTree.Load(this));
      itemsTree.Value.RefreshPositions();
    }


    #endregion Public methods

  } // class Project

} // namespace Empiria.ProjectManagement
