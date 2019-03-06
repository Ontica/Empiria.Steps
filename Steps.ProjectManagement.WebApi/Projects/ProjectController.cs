/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ProjectController                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API to retrieve and set projects data.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Contacts;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class ProjectController : WebApiController {

    #region Get methods

    [HttpGet]
    [Route("v1/project-management/projects")]
    public CollectionModel GetProjectsList() {
      try {
        var list = Project.GetList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Project).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/responsibles")]
    public CollectionModel GetProjectResponsiblesList(string projectUID) {
      try {
        var project = Project.Parse(projectUID);

        return new CollectionModel(this.Request, project.Responsibles.ToShortResponse(),
                                   typeof(Contact).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/requesters")]
    public CollectionModel GetProjectRequestersList(string projectUID) {
      try {
        var project = Project.Parse(projectUID);

        return new CollectionModel(this.Request, project.Requesters.ToShortResponse(),
                                   typeof(Contact).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/task-managers")]
    public CollectionModel GetProjectTaskManagersList(string projectUID) {
      try {
        var project = Project.Parse(projectUID);

        return new CollectionModel(this.Request, project.TaskManagers.ToShortResponse(),
                                   typeof(Contact).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/themes")]
    public CollectionModel GetProjectThemes() {
      try {
        var list = Project.ThemesList;

        return new CollectionModel(this.Request, list);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/as-tree")]
    public CollectionModel GetProjectActivitiesAsTree([FromUri] string projectUID) {
      try {

        var project = Project.Parse(projectUID);

        var fullActivitiesList = project.GetItems();

        return new CollectionModel(this.Request, fullActivitiesList.ToResponse(),
                                   typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion Get methods

  }  // class ProjectController

}  // namespace Empiria.ProjectManagement.WebApi
