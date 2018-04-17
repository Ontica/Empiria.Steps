/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Web Api Controller                  *
*  Type     : ProjectsController                               License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Public API to retrieve and set projects data.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Contacts;
using Empiria.Steps.ProjectManagement;

namespace Empiria.Governance.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class ProjectsController : WebApiController {

    #region GET methods

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

        return new CollectionModel(this.Request, project.Responsibles.ToResponse(),
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

        return new CollectionModel(this.Request, project.Requesters.ToResponse(),
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

        return new CollectionModel(this.Request, project.TaskManagers.ToResponse(),
                                   typeof(Contact).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

  }  // class ProjectsController

}  // namespace Empiria.Governance.WebApi
