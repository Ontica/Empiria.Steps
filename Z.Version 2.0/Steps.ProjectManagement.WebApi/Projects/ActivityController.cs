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

using Empiria.Postings;
using Empiria.ProjectManagement.Messaging;
using Empiria.ProjectManagement.Services;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API to retrieve and set project activities.</summary>
  public class ActivityController : WebApiController {

    #region Get methods

    [HttpGet]
    [Route("v1/project-management/activities/all-activities")]
    public CollectionModel GetAllProjectsActivities() {
      try {
        FixedList<ProjectItem> list = ProjectItem.GetList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


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
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/change-parent/{newParentUID}")]
    public SingleObjectModel ChangeParent(string projectUID, string activityUID,
                                          string newParentUID) {
      try {
        var project = Project.Parse(projectUID);

        ProjectItem activity = project.GetActivity(activityUID);
        ProjectItem newParent = project.GetActivity(newParentUID);

        activity = project.ChangeParentKeepingPosition(activity, newParent);

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/change-position/{newPosition}")]
    public SingleObjectModel ChangePosition(string projectUID, string activityUID,
                                            int newPosition) {
      try {
        var project = Project.Parse(projectUID);

        ProjectItem activity = project.GetActivity(activityUID);

        activity = project.ChangePosition(activity, newPosition);

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

        DateTime completedDate = bodyAsJson.Get<DateTime>("actualEndDate", DateTime.Today);

        activity.Update(bodyAsJson);

        ProjectUpdater.Complete(activity, completedDate);

        return new SingleObjectModel(this.Request, activity.ToResponse());

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

        ProjectUpdater.Reactivate(activity);

        return new SingleObjectModel(this.Request, activity.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/copyTo/{targetProjectUID}")]
    public SingleObjectModel CopyActivity(string projectUID, string activityUID,
                                          string targetProjectUID) {
      try {
        var project = Project.Parse(projectUID);
        var targetProject = Project.Parse(targetProjectUID);

        Activity activity = project.GetActivity(activityUID);

        var copy = targetProject.CopyActivity(activity);

        return new SingleObjectModel(this.Request, copy.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/moveTo/{targetProjectUID}")]
    public SingleObjectModel MoveActivityToProject(string projectUID, string activityUID,
                                                   string targetProjectUID) {
      try {
        Assertion.Assert(projectUID != targetProjectUID, "Source and target projects must be different.");

        var project = Project.Parse(projectUID);
        var targetProject = Project.Parse(targetProjectUID);

        Activity activity = project.GetActivity(activityUID);

        activity = targetProject.MoveActivity(activity);

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

        if (!activity.ReminderData.IsEmptyInstance) {
          EventsNotifier.RemindActivity(activity, activity.ReminderData);
        }

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


    [HttpPost]
    [Route("v1/postings-new/{objectUID}")]
    public SingleObjectModel CreateObjectPosting(string objectUID,
                                                 [FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var posting = new ObjectPosting(objectUID, bodyAsJson);

        posting.Save();

        if (!posting.SendTo.IsEmptyInstance) {
          var activity = Activity.Parse(objectUID);

          EventsNotifier.SendNotification(activity, posting.SendTo, posting.Title, posting.Body);
        }

        return new SingleObjectModel(this.Request, posting.ToResponse(),
                                     typeof(ObjectPosting).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

  }  // class ActivityController

}  // namespace Empiria.ProjectManagement.WebApi
