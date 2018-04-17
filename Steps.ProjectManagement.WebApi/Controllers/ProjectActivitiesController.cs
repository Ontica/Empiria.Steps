/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Web Api Controller                  *
*  Type     : ProjectsController                               License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Public API to retrieve and set project activities.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

using Empiria.Steps.ProjectManagement;

namespace Empiria.Governance.WebApi {

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
    public CollectionModel GetProjectActivitiesAsGantt([FromUri] ActivityFilter filter = null,
                                                       [FromUri] ActivityOrder orderBy = ActivityOrder.Default) {
      try {

        if (filter == null) {
          filter = new ActivityFilter();
        }

        var projectUID = "sdlkjfh34";

        var project = Project.Parse(projectUID);

        var fullActivitiesList = project.GetActivities();

        return new CollectionModel(this.Request, fullActivitiesList.ToGanttResponse(),
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

        Activity activity = (Activity) project.AddItem(bodyAsJson);

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

        Activity activity = null;
        if (EmpiriaString.IsInteger(activityUID)) {
          activity = Activity.Parse(int.Parse(activityUID));
        } else {
          activity = Activity.Parse(activityUID);
        }

        Assertion.Assert(activity.Project.Equals(project),
                         $"Activity with uid ({activityUID}) is not part of project with uid ({projectUID}).");

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

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class ProjectActivitiesController

}  // namespace Empiria.Governance.WebApi
