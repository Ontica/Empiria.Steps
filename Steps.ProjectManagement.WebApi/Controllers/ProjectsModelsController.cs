/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Web Api Controller                  *
*  Type     : ProjectsModelsController                         License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Public API to get and set workflow definition objects.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

using Empiria.Steps.ProjectManagement;
using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Governance.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class ProjectsModelsController : WebApiController {

    #region GET methods

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

    #endregion GET methods

    #region UPDATE methods

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

        var fullActivitiesList = project.GetActivities();

        return new CollectionModel(this.Request, fullActivitiesList.ToResponse(),
                                   typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class ProjectsModelsController

}  // namespace Empiria.Governance.WebApi
