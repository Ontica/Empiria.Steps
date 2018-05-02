/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ProjectModelsController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web services used to interact with project models.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

using Empiria.Workflow.Definition;

using Empiria.ProjectManagement.WebApi;

namespace Empiria.ProjectManagement.Modeling.WebApi {

  /// <summary>Web services used to interact with project models.</summary>
  public class ProjectModelController : WebApiController {

    #region Get methods

    [HttpGet]
    [Route("v1/projects/process-models/for-activities")]
    public CollectionModel GetProcessModelsForActivities() {
      try {

        var list = ProjectModel.GetActivitiesModelsList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(ProjectModel).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/projects/process-models/for-events")]
    public CollectionModel GetProcessModelsForEvents() {
      try {

        var list = ProjectModel.GetEventsModelsList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(ProjectModel).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Get methods

    #region Update methods

    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/create-from-process-model/{processModelUID}")]
    public CollectionModel CreateActivitiesFromProcessModel(string projectUID, string processModelUID,
                                                            [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);
        var process = Process.Parse(processModelUID);

        ProjectModel projectModel = ProjectModel.Parse(process);

        projectModel.CreateInstance(project, bodyAsJson);

        var fullActivitiesList = project.GetItems();

        return new CollectionModel(this.Request, fullActivitiesList.ToResponse(),
                                   typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Update methods

  }  // class ProjectModelsController

}  // namespace Empiria.ProjectManagement.WebApi
