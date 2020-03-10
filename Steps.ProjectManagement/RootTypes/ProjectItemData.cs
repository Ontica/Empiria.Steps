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

    static private string ProcessID = String.Empty;
    static private string SubProcessID = String.Empty;


    #region Read methods

    static internal List<Task> GetTasks(ProjectItem projectItem) {
      string sql = $"SELECT * FROM PMProjectObjects " +
                   $"WHERE ProjectObjectTypeId = {ProjectItemType.TaskType.Id} AND " +
                   $"ParentId = {projectItem.Id} AND Status <> 'X' " +
                   "ORDER BY ItemPosition";


      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Task>(x));
    }


    internal static void ClearProcessID() {
      ProcessID = String.Empty;
      SubProcessID = String.Empty;
    }


    internal static void ResetProcessID() {
      ProcessID = Guid.NewGuid().ToString().ToLower();
      SubProcessID = String.Empty;
    }


    internal static void ResetSubprocessID(string processID) {
      ProcessID = processID;
      SubProcessID = Guid.NewGuid().ToString().ToLower();
    }

    #endregion Read methods

    #region Write methods

    static internal void UpdatePosition(ProjectItem projectItem) {
      var op = DataOperation.Parse("setPMProjectObjectPosition",
                                    projectItem.Id, projectItem.Position);

      DataWriter.Execute(op);
    }


    static internal void WriteActivity(Activity o) {
      if (o.IsNew && !o.HasProcess) {
        o.SetProcess(ProcessID, SubProcessID);
      }

      var op = DataOperation.Parse("writePMProjectObject",
              o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
              o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
              o.ActualStartDate, o.ActualEndDate, o.PlannedEndDate, o.Deadline,
              o._theme, o.Tag, o.Keywords, o.Position,
              o.TemplateId, o.Resource, o.Project.Owner.Id,
              o.Responsible.Id, o.AssignedDate, o.AssignedBy.Id,
              o.Project.Id, o.Parent.Id, (char) o.Stage, (char) o.Status, o.ProcessID, o.SubprocessID);

      DataWriter.Execute(op);
    }


    static internal void WriteSummary(Summary o) {
      if (o.IsNew && !o.HasProcess) {
        o.SetProcess(ProcessID, SubProcessID);
      }

      var op = DataOperation.Parse("writePMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.ActualStartDate, o.ActualEndDate, o.PlannedEndDate, o.Deadline,
                o._theme, o.Tag, o.Keywords, o.Position,
                o.TemplateId, o.Resource, o.Project.Owner.Id,
                -1, ExecutionServer.DateMaxValue, -1,
                o.Project.Id, o.Parent.Id, (char) o.Stage, (char) o.Status, o.ProcessID, o.SubprocessID);

      DataWriter.Execute(op);
    }


    static internal void WriteTask(Task o) {
      if (o.IsNew && !o.HasProcess) {
        o.SetProcess(ProcessID, SubProcessID);
      }

      var op = DataOperation.Parse("writePMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.ActualStartDate, o.ActualEndDate, o.PlannedEndDate, o.Deadline,
                o._theme, o.Tag.ToString(), o.Keywords, o.Position,
                o.TemplateId, o.Resource, o.Project.Owner.Id,
                o.Responsible.Id, o.AssignedDate, o.AssignedBy.Id,
                o.Activity.Project.Id, o.Activity.Id, (char) o.Stage, (char) o.Status, o.ProcessID, o.SubprocessID);

      DataWriter.Execute(op);
    }

    #endregion Write methods

  }  // class ProjectItemData

}  // namespace Empiria.ProjectManagement
