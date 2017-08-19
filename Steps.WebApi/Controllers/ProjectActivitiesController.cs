/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : ProjectsController                               License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Public API to retrieve and set project activities.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.ProjectManagement;
using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Steps.WebApi {

  /// <summary>Public API to retrieve and set project activities.</summary>
  public class ProjectActivitiesController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/activities")]
    public SingleObjectModel GetProjectActivitiesList(string projectUID,
                                                      [FromUri] string filter = "") {
      try {
        var project = Project.Parse(projectUID);

        var fullActivitiesList = project.GetAllActivities();

        return new SingleObjectModel(this.Request, fullActivitiesList.ToGanttResponse(),
                                     typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/activities/{activityId}")]
    public SingleObjectModel GetProjectActivity(int activityId) {
      try {
        var activity = Activity.Parse(activityId);

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/activities/{activityId}/tasks")]
    public CollectionModel GetActivityTasks(int activityId) {
      try {
        var activity = Activity.Parse(activityId);

        return new CollectionModel(this.Request, activity.Tasks.ToResponse(),
                                   typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

    #region UPDATE methods

    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities")]
    public SingleObjectModel AddActivity(string projectUID,
                                        [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);

        project.AddActivity(bodyAsJson);

        var fullActivitiesList = project.GetAllActivities();

        return new SingleObjectModel(this.Request, fullActivitiesList.ToGanttResponse(),
                                     typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/create-from-process-model/{processModelUID}")]
    public SingleObjectModel CreateActivitiesFromProcessModel(string projectUID, string processModelUID,
                                                              [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);
        var process = Process.Parse(processModelUID);

        ProjectModel projectModel = ProjectModel.Parse(process);

        projectModel.CreateInstance(project, bodyAsJson);

        var fullActivitiesList = project.GetAllActivities();

        return new SingleObjectModel(this.Request, fullActivitiesList.ToGanttResponse(),
                                     typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class ProjectActivitiesController

}  // namespace Empiria.Steps.WebApi
