/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.dll                                Pattern : DTO                                 *
*  Type     : ActivityFilter                                   License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Holds fields used to filter project activities.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Data;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Holds fields used to filter project activities.</summary>
  public class ActivityFilter {

    #region Public properties

    public string[] Contract {
      get; set;
    } = new string[0];


    public string[] Project {
      get; set;
    } = new string[0];


    public string[] Stage {
      get; set;
    } = new string[0];


    public string[] Tag {
      get; set;
    } = new string[0];


    public string[] Responsible {
      get; set;
    } = new string[0];


    public string Keywords {
      get; set;
    } = String.Empty;


    #endregion Public properties

    #region Public methods

    public string GetKeywordsFilter() {
      if (this.Keywords.Length != 0) {
        return SearchExpression.ParseAndLike("Name", this.Keywords);
      }

      return String.Empty;
    }

    public string GetProjectsSqlFilter() {
      List<Project> list = this.GetProjectsList();

      if (list.Count == 1) {
        return $"[BaseProjectId] = {list[0].Id}";

      } else if (list.Count != 0) {

        var temp = "";
        foreach (var project in list) {
          if (temp.Length != 0) {
            temp += ", ";
          }
          temp += $"{project.Id}";
        }

        return $"[BaseProjectId] IN ({temp})";

      } else {

        return SearchExpression.NoRecordsFilter;

      }
    }


    public string GetResponsiblesSqlFilter() {
      Contact contact = null;

      if (this.Responsible.Length == 1) {
        contact = Contact.Parse(this.Responsible[0]);

        return $"([ResponsibleId] = {contact.Id})";
      }

      if (this.Responsible.Length != 0) {

        var temp = "";
        foreach (var uid in this.Responsible) {
          if (temp.Length != 0) {
            temp += ", ";
          }
          contact = Contact.Parse(uid);
          temp += $"{contact.Id}";
        }

        return $"[ResponsibleId] IN ({temp})";
      }

      return String.Empty;
    }

    public string GetTagsSqlFilter() {
      if (this.Tag.Length == 1) {
        return $"([Tags] LIKE '%''{this.Tag[0]}''%')";
      }

      if (this.Tag.Length != 0) {

        var temp = "";
        foreach (var tag in this.Tag) {
          if (temp.Length != 0) {
            temp += " OR ";
          }
          temp += $"([Tags] LIKE '%''{tag}''%')";
        }

        return $"({temp})";
      }

      return String.Empty;
    }

    #endregion Public methods

    #region Private methods

    private List<Project> GetProjectsList() {
      var list = new List<Project>(4);

      if (this.Project.Length != 0) {

        foreach (var projectUID in this.Project) {
          var project = ProjectManagement.Project.Parse(projectUID);

          list.Add(project);
        }

      } else if (this.Contract.Length != 0) {

        var allProjects = ProjectManagement.Project.GetList();

        allProjects = allProjects.FindAll((x) => EmpiriaString.IsInList(x.Contract.UID,
                                                                        this.Contract[0], this.Contract));

        list.AddRange(allProjects);

      } else {    // All projects in all contracts

        var allProjects = ProjectManagement.Project.GetList();

        list.AddRange(allProjects);

      }

      return list;
    }

    #endregion Private methods

  }  // ActivityFilter

}  // namespace Empiria.Steps.ProjectManagement
