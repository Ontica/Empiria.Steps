/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : Task                                             License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes a task.                                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Security;

namespace Empiria.ProjectManagement {

  /// <summary>Describes a task.</summary>
  public class Task : ProjectItem {

    #region Constructors and parsers

    protected Task(ProjectItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected Task() : this(ProjectItemType.TaskType) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected internal Task(Activity activity, JsonObject data) :
                            base(ProjectItemType.TaskType, activity.Project, data) {
      this.AssertIsValid(data);

      base.SetParent(activity);

      this.Load(data);
    }

    static internal new Task Parse(int id) {
      return BaseObject.ParseId<Task>(id);
    }


    static public new Task Parse(string uid) {
      return BaseObject.ParseKey<Task>(uid);
    }


    protected override void OnLoadObjectData(System.Data.DataRow row) {
      base.OnLoadObjectData(row);
    }

    #endregion Constructors and parsers

    #region Properties

    public Activity Activity {
      get {
        return base.Parent.IsEmptyInstance ?
                          Activity.Empty : (Activity) base.Parent;
      }
    }


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

    #endregion Properties


    #region Private methods

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


    protected override void AssertIsValid(JsonObject data) {
      base.AssertIsValid(data);
      this.Assign(data);
    }


    protected override void Load(JsonObject data) {
      base.Load(data);
      base.LoadDateFields(data);
      this.Assign(data);
    }


    protected override void OnSave() {
      ProjectItemData.WriteTask(this);
    }

    #endregion Private methods

  } // class Task

} // namespace Empiria.ProjectManagement
