/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : Activity                                         License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes a project activity.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Security;

namespace Empiria.ProjectManagement {

  /// <summary>Describes a project activity.</summary>
  public class Activity : ProjectItem {

    #region Constructors and parsers

    protected Activity() : this(ProjectItemType.ActivityType) {

    }


    protected Activity(ProjectItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    protected internal Activity(Project project, JsonObject data) :
                                  base(ProjectItemType.ActivityType, project, data) {

      this.AssertIsValid(data);
      this.Load(data);
    }


    protected internal Activity(Project project, ProjectItem data) :
                                              base(ProjectItemType.ActivityType, project) {
      this.Load(data);
    }


    static public new Activity Parse(string uid) {
      return BaseObject.ParseKey<Activity>(uid);
    }


    static public new Activity Parse(int id) {
      return BaseObject.ParseId<Activity>(id);
    }


    static public new Activity Empty {
      get {
        return BaseObject.ParseEmpty<Activity>();
      }
    }


    #endregion Constructors and parsers

    #region Properties

    [DataField("ResponsibleId")]
    public Contact Responsible {
      get;
      private set;
    }


    [DataField("AssignedDate")]
    public DateTime AssignedDate {
      get;
      private set;
    } = ExecutionServer.DateMaxValue;


    [DataField("AssignedById")]
    public Contact AssignedBy {
      get;
      private set;
    }


    public bool IsAssigned {
      get {
        return !this.Responsible.IsEmptyInstance;
      }
    }


    public JsonObject ConfigurationJson {
      get {
        return this.ExtensionData.Slice("config", false);
      }
    }


    [DataField("PeriodicRuleData")]
    private string _periodicRuleData {
      get;
      set;
    } = String.Empty;


    public ActivityModel Template {
      get {
        var json = this.ConfigurationJson;

        if (json.IsEmptyInstance) {
          return ActivityModel.Empty;
        } else {
          return ActivityModel.Parse(json, _periodicRuleData);
        }
      }
    }

    #endregion Properties


    #region Public methods

    public void Assign(JsonObject data) {
      if (!data.Contains("responsibleUID")) {
        return;
      }

      this.Responsible = data.Get<Contact>("responsibleUID", this.Responsible);

      if (!this.Responsible.IsEmptyInstance) {
        this.AssignedDate = DateTime.Now;
        this.AssignedBy = EmpiriaUser.Current.AsContact();
      } else {
        this.AssignedDate = ExecutionServer.DateMaxValue;
        this.AssignedBy = Contact.Empty;
      }
    }


    public ActivityEditionRules GetRules() {
      return new ActivityEditionRules(this);
    }


    protected override void Load(JsonObject data) {
      base.Load(data);
      base.LoadDateFields(data);
      this.Assign(data);
    }


    protected override void OnSave() {
      ProjectItemData.WriteActivity(this);
    }


    public override void Update(JsonObject data) {
      this.Load(data);

      this.Save();
    }

    #endregion Public methods

    } // class Activity

} // namespace Empiria.ProjectManagement
