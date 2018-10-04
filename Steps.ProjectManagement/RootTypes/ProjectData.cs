/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Data Service                        *
*  Type     : ProjectData                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Project data read and write methods.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Data;

namespace Empiria.ProjectManagement {

  /// <summary>Project data read and write methods.</summary>
  static internal class ProjectData {

    static internal List<Project> GetProjects(Contact ownerOrManager) {
      string filter = $"(OwnerId = {ownerOrManager.Id} OR ResponsibleId = {ownerOrManager.Id}) " +
                      $"AND Status <> 'X' AND ExtData LIKE '%designPattern%'";

      return BaseObject.GetList<Project>(filter, "ItemPosition");
    }

    #region Project structure methods

    static internal List<ProjectItem> GetProjectActivities(Project project,
                                                           ActivityFilter filter = null,
                                                           ActivityOrder orderBy = ActivityOrder.Default) {

      if (filter == null) {
        filter = new ActivityFilter();
      }
      string sql = $"SELECT * FROM PMProjectObjects " +
                   $"WHERE ProjectObjectTypeId <> {ProjectItemType.TaskType.Id} AND " +
                   $"BaseProjectId = {project.Id} AND Status <> 'X' " +
                   $"ORDER BY ItemPosition";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<ProjectItem>(x));
    }


    static internal List<ProjectItem> GetNoSummaryActivities(string filter = "", string orderBy = "") {

      filter = GeneralDataOperations.BuildSqlAndFilter($"ProjectObjectTypeId <> {ProjectItemType.SummaryType.Id}",
                                                       "Status <> 'X'", filter);
      string sql = $"SELECT * FROM PMProjectObjects " +
                   GeneralDataOperations.GetFilterSortSqlString(filter, orderBy);

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<ProjectItem>(x));
    }

    #endregion Project structure methods

    #region Project contacts methods

    static internal FixedList<Contact> GetProjectInvolvedContacts(Project project) {
      var list = new Contact[6];

      list[0] = Contact.Parse(2);
      list[1] = Contact.Parse(4);
      list[2] = Contact.Parse(8);
      list[3] = Contact.Parse(9);
      list[4] = Contact.Parse(10);
      list[5] = Contact.Parse(11);

      return new FixedList<Contact>(list);
    }


    static internal FixedList<Contact> GetProjectResponsibles(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (61, 62, 63, 64, 65) " +
                   $"ORDER BY Nickname";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }


    static internal FixedList<Contact> GetProjectRequesters(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (65, 66, 67, 68) " +
                   $"ORDER BY Nickname";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }


    static internal FixedList<Contact> GetProjectTaskManagers(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (67, 69, 70) " +
                   $"ORDER BY Nickname";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }

    #endregion Project contacts methods

    #region Write methods

    static internal void WriteProject(Project o) {
      var op = DataOperation.Parse("writePMProjectObject",
                o.Id, o.GetEmpiriaType().Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.StartDate, o.TargetDate, o.EndDate, o.DueDate, (char) o.RagStatus,
                o.Tags.ToString(), o.Keywords, o.Position, o.WorkflowObject.Id,
                o.Resource.Id, o.Owner.Id,
                o.Responsible.Id, ExecutionServer.DateMaxValue, -1,
                -1, o.Parent.Id, 'U', (char) o.Status);

      DataWriter.Execute(op);
    }

    #endregion Write methods

  }  // class ProjectData

}  // namespace Empiria.ProjectManagement
