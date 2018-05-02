/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : Task                                             License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a task.                                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;

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

    protected internal Task(Project project, JsonObject data) :
                            base(ProjectItemType.TaskType, project, data) {
      this.AssertIsValid(data);

      this.Load(data);
    }

    static internal new Task Parse(int id) {
      return BaseObject.ParseId<Task>(id);
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

    protected override void AssertIsValid(JsonObject data) {
      base.AssertIsValid(data);
    }

    protected override void Load(JsonObject data) {
      base.Load(data);
      base.LoadDateFields(data);
    }

    protected override void OnSave() {
      ProjectItemData.WriteTask(this);
    }

    #endregion Private methods

  } // class Task

} // namespace Empiria.ProjectManagement
