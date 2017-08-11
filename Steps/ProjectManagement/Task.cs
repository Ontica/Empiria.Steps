/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Task                                             License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a task.                                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Steps.ProjectManagement {

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
                                    base(project, ProjectItemType.TaskType, data) {
      this.AssertIsValid(data);

      this.Load(data);
    }

    static public new Task Parse(int id) {
      return BaseObject.ParseId<Task>(id);
    }

    static public new Task Empty {
      get {
        return BaseObject.ParseEmpty<Task>();
      }
    }

    protected override void OnLoadObjectData(System.Data.DataRow row) {
      base.OnLoadObjectData(row);
    }

    #endregion Constructors and parsers

    #region Public properties

    #endregion Public properties

    #region Private methods

    private void AssertIsValid(JsonObject data) {

    }

    private void Load(JsonObject data) {

    }

    #endregion Private methods

  } // class Task

} // namespace Empiria.Steps.ProjectManagement
