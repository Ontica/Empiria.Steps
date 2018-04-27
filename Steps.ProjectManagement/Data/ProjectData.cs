/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Data Service                        *
*  Type     : ProjectData                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Project's data read and write methods.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Data;

using Empiria.ProjectManagement.Resources;

namespace Empiria.ProjectManagement {

  /// <summary>Project's data read and write methods.</summary>
  static internal class ProjectData {

    static internal List<Project> GetProjects(Contact ownerOrManager) {
      string sql = $"SELECT * FROM BPMProjectObjects " +
                   $"WHERE ProjectObjectTypeId = {ProjectObjectType.ProjectType.Id} AND " +
                   $"(OwnerId = {ownerOrManager.Id} OR ResponsibleId = {ownerOrManager.Id}) " +
                   $"AND Status <> 'X' " +
                   $"ORDER BY ItemOrdering";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Project>(x));
    }


    static internal List<ProjectObject> GetAllActivities(ProjectObject parent) {
      string sql = $"SELECT * FROM BPMProjectObjects " +
                   $"WHERE ParentId = {parent.Id} AND Status <> 'X' " +
                   $"ORDER BY ItemOrdering";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<ProjectObject>(x));
    }

    static internal List<ProjectObject> GetProjectActivities(Project project,
                                                             ActivityFilter filter = null,
                                                             ActivityOrder orderBy = ActivityOrder.Default) {

      if (filter == null) {
        filter = new ActivityFilter();
      }
      string sql = $"SELECT * FROM BPMProjectObjects " +
                   $"WHERE ProjectObjectTypeId <> {ProjectObjectType.TaskType.Id} AND " +
                   $"BaseProjectId = {project.Id} AND Status <> 'X' " +
                   $"ORDER BY ItemOrdering";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<ProjectObject>(x));
    }

    static internal List<Activity> GetChildrenActivities(ProjectObject parent) {
      string sql = $"SELECT * FROM BPMProjectObjects " +
                   $"WHERE ProjectObjectTypeId = {ProjectObjectType.ActivityType.Id} AND " +
                   $"ParentId = {parent.Id} AND Status <> 'X' " +
                   $"ORDER BY ItemOrdering";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Activity>(x));
    }

    static internal List<ProjectObject> GetNoSummaryActivities(string filter = "", string orderBy = "") {

      filter = GeneralDataOperations.BuildSqlAndFilter($"ProjectObjectTypeId <> {ProjectObjectType.SummaryType.Id}",
                                                       "Status <> 'X'", filter);
      string sql = $"SELECT * FROM BPMProjectObjects " +
                   GeneralDataOperations.GetFilterSortSqlString(filter, orderBy);

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<ProjectObject>(x));
    }

    static internal List<Task> GetProjectActivityTasks(Activity activity) {
      string sql = $"SELECT * FROM BPMProjectObjects " +
                   $"WHERE ProjectObjectTypeId = {ProjectObjectType.TaskType.Id} AND " +
                   $"ParentId = {activity.Id} AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Task>(x));
    }


    static internal void WriteActivity(Activity o) {
      var op = DataOperation.Parse("writeBPMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.StartDate, o.TargetDate, o.EndDate, o.DueDate, (char) o.RagStatus,
                o.Tags.ToString(), o.Keywords, o.Position, o.WorkflowObject.Id, o.CreatedFrom.Id,
                o.Resource.Id, o.Owner.Id, o.Responsible.Id, o.RequestedTime, o.RequestedBy.Id,
                o.Project.Id, o.Parent.Id, (char) o.Stage, (char) o.Status);

      DataWriter.Execute(op);
    }

    internal static void UpdatePositionsStartingFrom(Activity from) {
      var op = DataOperation.Parse("doBPMUpdateProjectObjectsPositionFrom",
                                    from.Project.Id, from.Id,
                                    from.Position,
                                    (char) from.Status);

      DataWriter.Execute(op);
    }

    static internal void WriteSummary(Summary o) {
      var op = DataOperation.Parse("writeBPMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.StartDate, o.TargetDate, o.EndDate, o.DueDate, (char) o.RagStatus,
                o.Tags.ToString(), o.Keywords, o.Position, o.WorkflowObject.Id, o.CreatedFrom.Id,
                o.Resource.Id, o.Owner.Id, o.Responsible.Id, o.RequestedTime, o.RequestedBy.Id,
                o.Project.Id, o.Parent.Id, (char) o.Stage, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteProject(Project o) {
      var op = DataOperation.Parse("writeBPMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.StartDate, o.TargetDate, o.EndDate, o.DueDate, (char) o.RagStatus,
                o.Tags.ToString(), o.Keywords, o.Position, o.WorkflowObject.Id, o.CreatedFrom.Id,
                o.Resource.Id, o.Owner.Id, o.Manager.Id, ExecutionServer.DateMinValue, Contact.Empty.Id,
                Project.Empty.Id, Project.Empty.Id, (char) o.Stage, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteTask(Task o) {
      var op = DataOperation.Parse("writeBPMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.StartDate, o.TargetDate, o.EndDate, o.DueDate, (char) o.RagStatus,
                o.Tags.ToString(), o.Keywords, o.Position, o.WorkflowObject.Id, o.CreatedFrom.Id,
                Resource.Empty.Id, o.Owner.Id, o.AssignedTo.Id, o.AssignationTime, o.AssignedTo.Id,
                o.Activity.Project.Id, o.Activity.Id, (char) o.Stage, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class ProjectData

}  // namespace Empiria.ProjectManagement
