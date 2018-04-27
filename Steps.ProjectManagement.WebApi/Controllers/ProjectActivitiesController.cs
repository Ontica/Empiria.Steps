/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ProjectActivitiesController                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API to retrieve and set project activities.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API to retrieve and set project activities.</summary>
  public class ProjectActivitiesController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/activities")]
    [Route("v1/project-management/projects/{projectUID}/activities/as-tree")]
    public CollectionModel GetProjectActivitiesAsTree([FromUri] string projectUID,
                                                      [FromUri] ActivityFilter filter = null,
                                                      [FromUri] ActivityOrder orderBy = ActivityOrder.Default) {
      try {

        if (filter == null) {
          filter = new ActivityFilter();
        }

        var project = Project.Parse(projectUID);

        var fullActivitiesList = project.GetActivities(filter, orderBy);

        return new CollectionModel(this.Request, fullActivitiesList.ToResponse(),
                                   typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/projects/activities/as-work-list")]
    public CollectionModel GetProjectActivitiesAsWorklist([FromUri] ActivityFilter filter = null,
                                                          [FromUri] ActivityOrder orderBy = ActivityOrder.Default) {
      try {

        if (filter == null) {
          filter = new ActivityFilter();
        }

        var finder = new ProjectFinder(filter);

        FixedList<ProjectObject> activities = finder.GetActivitiesList(orderBy);

        return new CollectionModel(this.Request, activities.ToResponse(),
                                   typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/activities/as-gantt")]
    public CollectionModel GetProjectActivitiesAsGantt(string projectUID,
                                                       [FromUri] ActivityFilter filter = null,
                                                       [FromUri] ActivityOrder orderBy = ActivityOrder.Default) {
      try {

        if (filter == null) {
          filter = new ActivityFilter();
        }

        var project = Project.Parse(projectUID);

        var fullActivitiesList = project.GetActivities();

        return new CollectionModel(this.Request, fullActivitiesList.ToGanttResponse(),
                                   typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/activities/{activityUIDOrId}")]
    public SingleObjectModel GetProjectActivity(string activityUIDOrId) {
      try {
        Activity activity = null;

        if (EmpiriaString.IsInteger(activityUIDOrId)) {
          activity = Activity.Parse(int.Parse(activityUIDOrId));

        } else {
          activity = Activity.Parse(activityUIDOrId);

        }

        return new SingleObjectModel(this.Request, activity.ToResponse(),
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

        Activity activity = (Activity) project.AddActivity(bodyAsJson);

        return new SingleObjectModel(this.Request, activity.ToResponse(),
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

        Activity activity = project.GetActivity(activityUID);

        activity.Close(bodyAsJson);

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(Activity).FullName);

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

        Activity activity = project.GetActivity(activityUID);

        activity.Update(bodyAsJson);

        activity.Save();

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpDelete]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}")]
    public NoDataModel DeleteActivity(string projectUID, string activityUID) {
      try {
        var project = Project.Parse(projectUID);

        Activity activity = project.GetActivity(activityUID);

        project.RemoveActivity(activity);

        return new NoDataModel(this.Request);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class ProjectActivitiesController

}  // namespace Empiria.ProjectManagement.WebApi
