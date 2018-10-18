/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Data Service                        *
*  Type     : ProjectItemData                                  License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Project items data read and write methods.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Data;

namespace Empiria.ProjectManagement {

  /// <summary>Project items data read and write methods.</summary>
  static internal class ProjectItemData {

    #region Read methods

    static internal List<Task> GetActivityTasks(Activity activity) {
      string sql = $"SELECT * FROM PMProjectObjects " +
                   $"WHERE ProjectObjectTypeId = {ProjectItemType.TaskType.Id} AND " +
                   $"ParentId = {activity.Id} AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Task>(x));
    }


    #endregion Read methods

    #region Write methods

    static internal void UpdatePosition(ProjectItem projectItem) {
      var op = DataOperation.Parse("setPMProjectObjectPosition",
                                    projectItem.Id, projectItem.Position);

      DataWriter.Execute(op);
    }


    static internal void WriteActivity(Activity o) {
      var op = DataOperation.Parse("writePMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.StartDate, o.TargetDate, o.EndDate, o.DueDate, (char) o.RagStatus,
                o.Tags.ToString(), o.Keywords, o.Position, o.WorkflowObjectId,
                o.Resource.Id, o.Project.Owner.Id,
                o.Responsible.Id, o.AssignedDate, o.AssignedBy.Id,
                o.Project.Id, o.Parent.Id, (char) o.Stage, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteSummary(Summary o) {
      var op = DataOperation.Parse("writePMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.StartDate, o.TargetDate, o.EndDate, o.DueDate, (char) o.RagStatus,
                o.Tags.ToString(), o.Keywords, o.Position, o.WorkflowObjectId,
                o.Resource.Id, o.Project.Owner.Id,
                -1, ExecutionServer.DateMaxValue, -1,
                o.Project.Id, o.Parent.Id, (char) o.Stage, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteTask(Task o) {
      var op = DataOperation.Parse("writePMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.StartDate, o.TargetDate, o.EndDate, o.DueDate, (char) o.RagStatus,
                o.Tags.ToString(), o.Keywords, o.Position, o.WorkflowObjectId,
                o.Resource.Id, o.Project.Owner.Id,
                o.Responsible.Id, o.AssignedDate, o.AssignedBy.Id,
                o.Activity.Project.Id, o.Activity.Id, (char) o.Stage, (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Write methods

  }  // class ProjectItemData

}  // namespace Empiria.ProjectManagement
