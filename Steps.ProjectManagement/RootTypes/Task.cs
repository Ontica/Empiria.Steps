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

    protected internal Task(Project project, ProjectItem parent, JsonObject data) :
                            base(ProjectItemType.TaskType, project, parent, data) {
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

    #region Public properties

    public Activity Activity {
      get {
        return base.Parent.IsEmptyInstance ?
                          Activity.Empty : (Activity) base.Parent;
      }
    }


    [DataField("ResponsibleId")]
    public Contact AssignedTo {
      get;
      private set;
    }


    [DataField("RequestedTime")]
    public DateTime AssignationTime {
      get;
      private set;
    }


    public bool IsAssigned {
      get {
        return !this.AssignedTo.IsEmptyInstance;
      }
    }

    #endregion Public properties

    #region Private methods

    protected override void AssertIsValid(JsonObject data) {

    }

    protected override void Load(JsonObject data) {

    }

    protected override void OnSave() {
      ProjectData.WriteTask(this);
    }

    #endregion Private methods

  } // class Task

} // namespace Empiria.ProjectManagement
