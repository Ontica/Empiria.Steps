/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ActivityController                           License   : Please read LICENSE.txt file          *
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
  public class ActivityController : WebApiController {

    #region Get methods

    [HttpGet]
    [Route("v1/project-management/activities/{activityUIDOrId}")]
    public SingleObjectModel GetProjectActivity(string activityUIDOrId) {
      try {
        Activity projectItem;

        if (EmpiriaString.IsInteger(activityUIDOrId)) {
          projectItem = Activity.Parse(int.Parse(activityUIDOrId));

        } else {
          projectItem = Activity.Parse(activityUIDOrId);
        }

        return new SingleObjectModel(this.Request, projectItem.ToResponse(),
                                     typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Get methods

    #region Update methods

    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities")]
    public SingleObjectModel AddActivity(string projectUID,
                                         [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);

        Activity activity = project.AddActivity(bodyAsJson);

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/complete")]
    public SingleObjectModel CompleteActivity(string projectUID, string activityUID,
                                              [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);

        Activity activity = project.GetActivity(activityUID);

        activity.Complete(bodyAsJson);

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/reactivate")]
    public SingleObjectModel ReactivateActivity(string projectUID, string activityUID) {
      try {
        var project = Project.Parse(projectUID);

        Activity activity = project.GetActivity(activityUID);

        activity.Reactivate();

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/copyTo/{targetProjectUID}")]
    public SingleObjectModel CopyActivity(string projectUID, string activityUID,
                                          string targetProjectUID, [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);
        var targetProject = Project.Parse(targetProjectUID);

        Activity activity = project.GetActivity(activityUID);

        var copy = targetProject.CopyActivity(activity, bodyAsJson);

        return new SingleObjectModel(this.Request, copy.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/moveTo/{targetProjectUID}")]
    public SingleObjectModel MoveActivityToProject(string projectUID, string activityUID,
                                                   string targetProjectUID, [FromBody] object body) {
      try {
        Assertion.Assert(projectUID != targetProjectUID, "Source and target projects must be different.");

        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);
        var targetProject = Project.Parse(targetProjectUID);

        Activity activity = project.GetActivity(activityUID);

        activity = targetProject.MoveActivity(activity, bodyAsJson);

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

        project.DeleteActivity(activity);

        return new NoDataModel(this.Request);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Update methods

  }  // class ActivityController

}  // namespace Empiria.ProjectManagement.WebApi
