/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ActivityTaskController                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API to retrieve and control activities tasks (checklists).                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

using Empiria.ProjectManagement.Services;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API to retrieve and control activities tasks (checklists).</summary>
  public class ActivityTaskController : WebApiController {

    #region Get methods

    [HttpGet]
    [Route("v1/project-management/activities/{activityUID}/tasks")]
    public CollectionModel GetTasks(string activityUID) {
      try {
        ProjectItem activity = ParseActivityWithUID(activityUID);

        if (EmpiriaString.IsInteger(activityUID)) {
          activity = ProjectItem.Parse(int.Parse(activityUID));
        } else {
          activity = ProjectItem.Parse(activityUID);
        }

        return new CollectionModel(this.Request, activity.Tasks.ToResponse(),
                                   typeof(Task).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Get methods

    #region Update methods


    [HttpPost]
    [Route("v1/project-management/activities/{activityUID}/tasks")]
    public SingleObjectModel AddTask(string activityUID,
                                    [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        ProjectItem activity = ParseActivityWithUID(activityUID);

        Task task = activity.AddTask(bodyAsJson);

        task.Save();

        return new SingleObjectModel(this.Request, task.ToResponse(),
                                     typeof(Task).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/activities/{activityUID}/tasks/{taskUID}/complete")]
    public SingleObjectModel CompleteTask(string activityUID, string taskUID,
                                          [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var task = Task.Parse(taskUID);

        Assertion.Assert(task.Activity.UID == activityUID, "Task belongs to a distinct activity.");

        DateTime completedDate = bodyAsJson.Get<DateTime>("actualEndDate", DateTime.Today);

        task.Update(bodyAsJson);
        ProjectUpdater.Complete(task, completedDate);

        return new SingleObjectModel(this.Request, task.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/activities/{activityUID}/tasks/{taskUID}/reactivate")]
    public SingleObjectModel ReactivateActivity(string activityUID, string taskUID) {
      try {
        var task = Task.Parse(taskUID);

        Assertion.Assert(task.Activity.UID == activityUID, "Task belongs to a distinct activity.");

        ProjectUpdater.Reactivate(task);

        return new SingleObjectModel(this.Request, task.ToResponse(),
                                     typeof(Activity).FullName);


      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/project-management/activities/{activityUID}/tasks/{taskUID}")]
    public SingleObjectModel UpdateTask(string activityUID, string taskUID,
                                        [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var task = Task.Parse(taskUID);

        Assertion.Assert(task.Activity.UID == activityUID, "Task belongs to a distinct activity.");


        task.Update(bodyAsJson);


        return new SingleObjectModel(this.Request, task.ToResponse(),
                                     typeof(Task).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Update methods

    #region Private methods

    private ProjectItem ParseActivityWithUID(string activityUID) {
      if (EmpiriaString.IsInteger(activityUID)) {
        return ProjectItem.Parse(int.Parse(activityUID));
      } else {
        return ProjectItem.Parse(activityUID);
      }
    }

    #endregion Private methods

  }  // class ActivityTaskController

} // namespace Empiria.ProjectManagement.WebApi
