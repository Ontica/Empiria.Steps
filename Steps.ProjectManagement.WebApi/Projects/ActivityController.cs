﻿/* Empiria Steps *********************************************************************************************
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
        Activity activity = null;

        if (EmpiriaString.IsInteger(activityUIDOrId)) {
          activity = Activity.Parse(int.Parse(activityUIDOrId));

        } else {
          activity = Activity.Parse(activityUIDOrId);

        }

        return new SingleObjectModel(this.Request, activity.ToResponse(),
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

        Activity activity = (Activity) project.AddActivity(bodyAsJson);

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(ProjectItem).FullName);

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

    #endregion Update methods

  }  // class ActivityController

}  // namespace Empiria.ProjectManagement.WebApi