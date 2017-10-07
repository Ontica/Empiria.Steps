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

        return new SingleObjectModel(this.Request, fullActivitiesList.ToResponse(),
                                     typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/activities/as-gantt")]
    public SingleObjectModel GetProjectActivitiesListAsGantt(string projectUID,
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
    [Route("v1/project-management/activities/{activityUID}")]
    public SingleObjectModel GetProjectActivity(string activityUID) {
      try {
        Activity activity = null;
        if (EmpiriaString.IsInteger(activityUID)) {
          activity = Activity.Parse(int.Parse(activityUID));
        } else {
          activity = Activity.Parse(activityUID);
        }

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/activities/{activityUID}/tasks")]
    public CollectionModel GetActivityTasks(string activityUID) {
      try {
        Activity activity = null;
        if (EmpiriaString.IsInteger(activityUID)) {
          activity = Activity.Parse(int.Parse(activityUID));
        } else {
          activity = Activity.Parse(activityUID);
        }


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

        project.AddItem(bodyAsJson);

        var fullActivitiesList = project.GetAllActivities();

        return new SingleObjectModel(this.Request, fullActivitiesList.ToResponse(),
                                     typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/close")]
    public SingleObjectModel CloseActivity(string projectUID, string activityUID,
                                           [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);

        Activity activity = null;
        if (EmpiriaString.IsInteger(activityUID)) {
          activity = Activity.Parse(int.Parse(activityUID));
        } else {
          activity = Activity.Parse(activityUID);
        }

        Assertion.Assert(activity.Project.Equals(project),
                         $"Activity with uid ({activityUID}) is not part of project with uid ({projectUID}).");

        activity.Close(bodyAsJson);

        var fullActivitiesList = project.GetAllActivities();

        return new SingleObjectModel(this.Request, fullActivitiesList.ToResponse(),
                                     typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPut, HttpPatch]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}")]
    public SingleObjectModel UpdateActivity(string projectUID, string activityUID,
                                            [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);

        Activity activity = null;
        if (EmpiriaString.IsInteger(activityUID)) {
          activity = Activity.Parse(int.Parse(activityUID));
        } else {
          activity = Activity.Parse(activityUID);
        }

        Assertion.Assert(activity.Project.Equals(project),
                         $"Activity with uid ({activityUID}) is not part of project with uid ({projectUID}).");

        activity.Update(bodyAsJson);

        activity.Save();

        var fullActivitiesList = project.GetAllActivities();

        return new SingleObjectModel(this.Request, fullActivitiesList.ToResponse(),
                                     typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class ProjectActivitiesController

}  // namespace Empiria.Steps.WebApi
