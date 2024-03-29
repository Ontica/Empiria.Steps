﻿/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ActivityTemplateController                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API with operations used to set activity templates data.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API with operations used to set activity templates data.</summary>
  public class ActivityTemplateController : WebApiController {

    #region Fields

    internal const string ACTIVITY_TEMPLATE_TYPE_NAME = "ObjectType.ProjectItem.ActivityTemplate";

    #endregion Fields


    #region Update methods


    [HttpPost]
    [Route("v1/project-management/project-templates/{projectTemplateUID}/activities/{activityTemplateUID}/change-parent/{newParentUID}")]
    public SingleObjectModel ChangeParent(string projectTemplateUID, string activityTemplateUID,
                                          string newParentUID) {
      try {
        var project = Project.Parse(projectTemplateUID);

        ProjectItem activity = project.GetActivity(activityTemplateUID);
        ProjectItem newParent = project.GetActivity(newParentUID);

        activity = project.ChangeParentKeepingPosition(activity, newParent);

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/project-templates/{projectTemplateUID}/activities/{activityTemplateUID}/change-position/{newPosition}")]
    public SingleObjectModel ChangePosition(string projectTemplateUID, string activityTemplateUID,
                                            int newPosition) {
      try {

        var project = Project.Parse(projectTemplateUID);

        ProjectItem activity = project.GetActivity(activityTemplateUID);

        activity = project.ChangePosition(activity, newPosition);

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/project-templates/{projectTemplateUID}/activities/{activityTemplateUID}/copyTo/{targetProjectTemplateUID}")]
    public SingleObjectModel CopyActivityTemplate(string projectTemplateUID, string activityTemplateUID,
                                                  string targetProjectTemplateUID) {
      try {
        var project = Project.Parse(projectTemplateUID);
        var targetProject = Project.Parse(targetProjectTemplateUID);

        Activity activity = project.GetActivity(activityTemplateUID);

        var copy = targetProject.CopyActivity(activity);

        return new SingleObjectModel(this.Request, copy.ToActivityTemplateResponse(),
                                     ACTIVITY_TEMPLATE_TYPE_NAME);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpDelete]
    [Route("v1/project-management/project-templates/{projectTemplateUID}/activities/{activityTemplateUID}")]
    public NoDataModel DeleteActivityTemplate(string projectTemplateUID, string activityTemplateUID) {
      try {
        var project = Project.Parse(projectTemplateUID);

        Activity activity = project.GetActivity(activityTemplateUID);

        project.DeleteActivity(activity);

        return new NoDataModel(this.Request);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/project-templates/{projectTemplateUID}/activities")]
    public SingleObjectModel InsertActivityTemplate(string projectTemplateUID,
                                                    [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectTemplateUID);

        Activity activity = project.AddActivity(bodyAsJson);

        return new SingleObjectModel(this.Request, activity.ToActivityTemplateResponse(),
                                     ACTIVITY_TEMPLATE_TYPE_NAME);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/project-templates/{projectTemplateUID}/activities/{activityTemplateUID}/moveTo/{targetProjectTemplateUID}")]
    public SingleObjectModel MoveActivityTemplateToProject(string projectTemplateUID, string activityTemplateUID,
                                                           string targetProjectTemplateUID) {
      try {
        Assertion.Require(projectTemplateUID != targetProjectTemplateUID, "Source and target projects must be different.");

        var project = Project.Parse(projectTemplateUID);
        var targetProject = Project.Parse(targetProjectTemplateUID);

        Activity activity = project.GetActivity(activityTemplateUID);

        activity = targetProject.MoveActivity(activity);

        return new SingleObjectModel(this.Request, activity.ToActivityTemplateResponse(),
                                     ACTIVITY_TEMPLATE_TYPE_NAME);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/project-management/project-templates/{projectTemplateUID}/activities/{activityTemplateUID}")]
    public SingleObjectModel UpdateActivityTemplate(string projectTemplateUID, string activityTemplateUID,
                                                    [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectTemplateUID);

        Activity activity = project.GetActivity(activityTemplateUID);

        activity.Update(bodyAsJson);

        return new SingleObjectModel(this.Request, activity.ToActivityTemplateResponse(),
                                     ACTIVITY_TEMPLATE_TYPE_NAME);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Update methods

  }  // class ActivityTemplateController

}  // namespace Empiria.ProjectManagement.WebApi
