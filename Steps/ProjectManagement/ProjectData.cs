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

    static internal List<Project> GetProjects(Contact contact) {
      string sql = $"SELECT * FROM BPMProjects " +
                   $"WHERE (OwnerId = {contact.Id} OR ManagerId = {contact.Id}) " +
                   $"AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<Project>(op, (x) => BaseObject.ParseList<Project>(x));
    }

    static internal List<ProjectItem> GetProjectActivities(Project project) {
      string sql = $"SELECT * FROM BPMProjectItems " +
                   $"WHERE ProjectItemTypeId <> 667 AND ProjectId = {project.Id} AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<ProjectItem>(op, (x) => BaseObject.ParseList<ProjectItem>(x));
    }

    static internal List<Task> GetProjectActivityTasks(ProjectItem projectItem) {
      string sql = $"SELECT * FROM BPMProjectItems " +
                   $"WHERE ProjectItemTypeId = 667 AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<Task>(op, (x) => BaseObject.ParseList<Task>(x));
    }

    static internal FixedList<Contact> GetProjectResponsibles(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (61, 62)";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<Contact>(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }

    static internal FixedList<Contact> GetProjectRequesters(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (63, 64, 65)";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<Contact>(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }

    static internal FixedList<Contact> GetProjectTaskManagers(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (62, 66, 67, 68, 69)";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<Contact>(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }

    static internal void WriteProjectItem(ProjectItem o) {
      var op = DataOperation.Parse("writeBPMProjectItem",
                o.Id, o.ProjectItemType.Id, o.Project.Id,
                o.RelatedProcedure.Id, o.Resource.Id,
                o.Name, o.Notes, "", o.EstimatedStart, o.EstimatedEnd, o.EstimatedDuration,
                o.ActualStart, o.ActualEnd, o.CompletionProgress, o.Responsible.Id,
                o.RequestedTime, o.RequestedBy.Id, o.Parent.Id, (char) o.Status);

      DataWriter.Execute(op);
    }

  }  // class ProjectData

}  // namespace Empiria.Steps.ProjectManagement
