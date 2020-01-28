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


    static internal FixedList<ProjectItem> GetAllActivities() {
      string projectList = UserProjectSecurity.GetUserProjectList();

      string sql = $"SELECT * FROM PMProjectObjects " +
                   $"WHERE BaseProjectId IN ({projectList}) AND ProjectObjectTypeId <> 1203 AND Status <> 'X' AND " +
                   $"BaseProjectId NOT IN (SELECT ProjectObjectId FROM PMProjectObjects WHERE Status = 'X' OR ExtData LIKE '%isTemplate%')";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<ProjectItem>(op).ToFixedList();
    }

    static internal List<Project> GetProjects() {
      string projectList = UserProjectSecurity.GetUserProjectList();

      string filter = $"ProjectObjectId IN ({projectList}) " +
                      $"AND Status <> 'X' AND ExtData NOT LIKE '%isTemplate%'";

      return BaseObject.GetList<Project>(filter, "ItemPosition");
    }


    static internal List<Project> GetTemplates() {
      string filter = $"Status <> 'X' AND ExtData LIKE '%isTemplate%'";

      return BaseObject.GetList<Project>(filter, "ItemPosition");
    }


    static internal List<Project> GetTemplates(Project forProject) {
      if (forProject.TemplatesList.Length == 0) {
        return new List<Project>();
      }

      string filter = $"Status <> 'X' AND ExtData LIKE '%isTemplate%' AND " +
                      $"ProjectObjectId IN (${forProject.TemplatesList})";

      return BaseObject.GetList<Project>(filter, "ItemPosition");
    }


    static internal FixedList<ProjectItem> GetEvents(Project forProject, string filter = "") {
      var templateProjects = GetTemplates(forProject);

      var events = new List<ProjectItem>();

      foreach (var project in templateProjects) {
        var items = project.GetItems();

        items = items.FindAll(x => x is Activity);
        items = items.FindAll(x => ((Activity) x).Template.ActivityType == "Event" ||
                                   (((Activity) x).Template.ActivityType == "Subprocess"));

        events.AddRange(items);
      }

      events.Sort((x, y) => x.Name.CompareTo(y.Name));

      return events.ToFixedList();
    }

    #region Project structure methods

    static internal List<ProjectItem> GetProjectActivities(Project project) {
      string sql = $"SELECT * FROM PMProjectObjects " +
                   $"WHERE ProjectObjectTypeId <> {ProjectItemType.TaskType.Id} AND " +
                   $"BaseProjectId = {project.Id} AND Status <> 'X' " +
                   $"ORDER BY ItemPosition";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<ProjectItem>(op);
    }


    static internal List<ProjectItem> GetNoSummaryActivities(string filter = "", string orderBy = "") {

      filter = GeneralDataOperations.BuildSqlAndFilter($"ProjectObjectTypeId <> {ProjectItemType.SummaryType.Id}",
                                                       "Status <> 'X'", filter);
      string sql = $"SELECT * FROM PMProjectObjects " +
                   GeneralDataOperations.GetFilterSortSqlString(filter, orderBy);

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<ProjectItem>(op);
    }


    #endregion Project structure methods

    #region Project contacts methods

    static internal FixedList<Contact> GetProjectInvolvedContacts(Project project) {
      string[] stringList = UserProjectSecurity.GetUserAssigneeList().Split(',');

      var list = new Contact[stringList.Length];

      for (int i = 0; i < stringList.Length; i++) {
        list[i] = Contact.Parse(int.Parse(stringList[i]));
      }
      return new FixedList<Contact>(list);
    }


    static internal FixedList<Contact> GetProjectAssignees(Project project) {
      string AssigneeList = UserProjectSecurity.GetUserAssigneeList();

      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN ({AssigneeList})" +
                   $"ORDER BY Nickname, ShortName";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Contact>(op);
    }


    static internal FixedList<Contact> GetProjectRequesters(Project project) {
      return GetProjectAssignees(project);
    }


    static internal FixedList<Contact> GetProjectTaskManagers(Project project) {
      return GetProjectAssignees(project);
    }

    #endregion Project contacts methods

    #region Write methods

    static internal void WriteProject(Project o) {
      var op = DataOperation.Parse("writePMProjectObject",
                o.Id, o.GetEmpiriaType().Id, o.UID, o.Name, o.Notes,
                o.ExtensionData.ToString(), o.EstimatedDuration.ToString(),
                o.ActualStartDate, o.ActualEndDate, o.PlannedEndDate, o.Deadline,
                o.Tags.ToString(), o.Keywords, o.Position, -1,
                o.Resource, o.Owner.Id,
                o.Responsible.Id, ExecutionServer.DateMaxValue, -1,
                -1, o.Parent.Id, 'U', (char) o.Status, String.Empty);

      DataWriter.Execute(op);
    }

    #endregion Write methods


  }  // class ProjectData


}  // namespace Empiria.ProjectManagement
