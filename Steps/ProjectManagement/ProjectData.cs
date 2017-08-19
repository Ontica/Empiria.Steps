/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.dll                                Pattern : Data Service                        *
*  Type     : ProjectData                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Project's data read and write methods.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;
using Empiria.Contacts;
using Empiria.Data;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Project's data read and write methods.</summary>
  static internal class ProjectData {

    static internal List<Project> GetProjects(Contact ownerOrManager) {
      string sql = $"SELECT * FROM BPMProjectObjects " +
                   $"WHERE ProjectObjectTypeId = {ProjectObjectType.ProjectType.Id} AND " +
                   $"(OwnerId = {ownerOrManager.Id} OR ResponsibleId = {ownerOrManager.Id}) " +
                   $"AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Project>(x));
    }

    static internal FixedList<Activity> GetAllProjectActivities(Project project) {
      string sql = $"SELECT * FROM BPMProjectObjects " +
                   $"WHERE ProjectObjectTypeId = {ProjectObjectType.ActivityType.Id} AND " +
                   $"BaseProjectId = {project.Id} AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Activity>(x))
                       .ToFixedList();
    }

    static internal List<Activity> GetChildrenActivities(ProjectObject parent) {
      string sql = $"SELECT * FROM BPMProjectObjects " +
                   $"WHERE ProjectObjectTypeId = {ProjectObjectType.ActivityType.Id} AND " +
                   $"ParentId = {parent.Id} AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Activity>(x));
    }


    static internal List<Task> GetProjectActivityTasks(Activity activity) {
      string sql = $"SELECT * FROM BPMProjectObjects " +
                   $"WHERE ProjectObjectTypeId = {ProjectObjectType.TaskType.Id} AND " +
                   $"ParentId = {activity.Id} AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Task>(x));
    }


    static internal FixedList<Contact> GetProjectResponsibles(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (61, 62)";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }


    static internal FixedList<Contact> GetProjectRequesters(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (63, 64, 65)";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }


    static internal FixedList<Contact> GetProjectTaskManagers(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (62, 66, 67, 68, 69)";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }


    static internal void WriteActivity(Activity o) {
      var op = DataOperation.Parse("writeBPMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(),
                o.EstimatedStart, o.EstimatedEnd, o.EstimatedDuration,
                o.ActualStart, o.ActualEnd, o.CompletionProgress,
                o.RelatedProcedure.Id, o.Resource.Id,
                o.Owner.Id, o.Responsible.Id, o.RequestedTime, o.RequestedBy.Id,
                o.Project.Id, o.Parent.Id, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteProject(Project o) {
      var op = DataOperation.Parse("writeBPMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(),
                o.EstimatedStart, o.EstimatedEnd, o.EstimatedDuration,
                o.ActualStart, o.ActualEnd, o.CompletionProgress,
                o.RelatedProcedure.Id, o.Resource.Id,
                o.Owner.Id, o.Manager.Id, ExecutionServer.DateMinValue, Contact.Empty.Id,
                Project.Empty.Id, Project.Empty.Id, (char) o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteTask(Task o) {
      var op = DataOperation.Parse("writeBPMProjectObject",
                o.Id, o.ProjectObjectType.Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(),
                o.EstimatedStart, o.EstimatedEnd, o.EstimatedDuration,
                o.ActualStart, o.ActualEnd, o.CompletionProgress,
                o.RelatedProcedure.Id, Resource.Empty.Id,
                o.Owner.Id, o.AssignedTo.Id, o.AssignationTime, o.AssignedTo,
                o.Activity.Project.Id, o.Activity.Id, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class ProjectData

}  // namespace Empiria.Steps.ProjectManagement
