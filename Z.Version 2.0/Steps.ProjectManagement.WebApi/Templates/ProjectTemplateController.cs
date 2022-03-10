/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ProjectTemplateController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API to retrieve and set ProjectTemplates data.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

using Empiria.ProjectManagement.Services;

using Empiria.ProjectManagement.WebApi;

namespace Empiria.ProjectManagement.Templates.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class ProjectTemplateController : WebApiController {

    #region Get methods

    [HttpGet]
    [Route("v1/project-management/project-templates")]
    public CollectionModel GetTemplatesList() {
      try {
        var list = Project.GetTemplatesList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Project).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/project-templates/{projectTemplateUID:guid}")]
    public SingleObjectModel GetProjectTemplate([FromUri] string projectTemplateUID) {
      try {
        var template = (Activity) ProjectItem.Parse(projectTemplateUID);

        return new SingleObjectModel(this.Request, template.ToActivityTemplateResponse(),
                                    typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/project-templates/{projectTemplateUID}/as-tree")]
    public CollectionModel GetProjectTemplateAsTree([FromUri] string projectTemplateUID) {
      try {
        var project = Project.Parse(projectTemplateUID);

        var activityModels = project.GetItems();

        return new CollectionModel(this.Request, activityModels.ToActivityTemplateResponse(),
                                   typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }



    [HttpGet]
    [Route("v1/project-management/project-templates/start-events/{projectUID}")]
    public CollectionModel GetStartEventsList([FromUri] string projectUID) {
      try {
        var project = Project.Parse(projectUID);

        var list = project.GetEventsList();

        return new CollectionModel(this.Request, list.ToActivityTemplateResponse(),
                                   typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Get methods


    #region Update methods


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/create-from-activity-template")]
    public CollectionModel CreateFromActivityTemplate(string projectUID,
                                                      [FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var activityModel = bodyAsJson.Get<Activity>("activityTemplateUID");
        var eventDate = bodyAsJson.Get<DateTime>("eventDate", DateTime.Today);
        var insertionPoint = bodyAsJson.Get<ProjectItem>("insertionPointUID", ProjectItem.Empty);
        var insertionRule = bodyAsJson.Get<TreeItemInsertionRule>("insertionPosition",
                                                                   TreeItemInsertionRule.AsTreeRootAtEnd);

        var project = Project.Parse(projectUID);

        FixedList<ProjectItem> createdActivities =
                       ProjectUpdater.CreateActivitiesFromModel(activityModel, project, eventDate,
                                                                insertionPoint, insertionRule);

        return new CollectionModel(this.Request, createdActivities.ToResponse());

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/update-all-deadlines")]
    public SingleObjectModel UpdateAllProjectDeadlines(string projectUID) {
      try {
        var project = Project.Parse(projectUID);

        FixedList<ProjectProcess> processes = ModelingServices.ProcessesCheckList(project);

        foreach (var process in processes) {
          Activity startActivity = project.GetActivity(process.StartActivity.UID);

          var updater = new ProcessUpdater(startActivity);

          updater.UpdateDeadlines();

        }

        return new SingleObjectModel(this.Request, processes.ToResponse(),
                                     typeof(WhatIfResult).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/update-all-with-last-process-changes")]
    public SingleObjectModel UpdateAllProjectProcessesWithLastChanges(string projectUID) {
      try {
        var project = Project.Parse(projectUID);

        var result = ModelingServices.ProcessesCheckList(project);

        foreach (var process in result) {
          Activity startActivity = project.GetActivity(process.StartActivity.UID);

          var updater = new ProcessUpdater(startActivity);

          updater.UpdateWithLastProcessChanges();
        }

        return new SingleObjectModel(this.Request, result.ToResponse(),
                                     typeof(WhatIfResult).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/update-deadlines")]
    public CollectionModel UpdateProjectDeadlines(string projectUID, string activityUID) {
      try {
        var project = Project.Parse(projectUID);

        Activity activity = project.GetActivity(activityUID);

        var updater = new ProcessUpdater(activity);

        FixedList<ProjectItem> result = updater.UpdateDeadlines();

        return new CollectionModel(this.Request, result.ToResponse(), typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/what-if-deadlines-updated")]
    public SingleObjectModel WhatIfUpdateProjectDeadlines(string projectUID, string activityUID) {
      try {
        var project = Project.Parse(projectUID);

        Activity activity = project.GetActivity(activityUID);
        var updater = new ProcessUpdater(activity);

        WhatIfResult result = updater.OnUpdateDeadlines();
        return new SingleObjectModel(this.Request, result.ToResponse(),
                                     typeof(WhatIfResult).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }



    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/update-with-last-process-changes")]
    public CollectionModel UpdateWithLastProcessChanges(string projectUID, string activityUID) {
      try {
        var project = Project.Parse(projectUID);

        Activity activity = project.GetActivity(activityUID);
        var updater = new ProcessUpdater(activity);

        FixedList<ProjectItem> activities = updater.UpdateWithLastProcessChanges();

        return new CollectionModel(this.Request, activities.ToResponse());

      } catch (Exception e) {
          throw base.CreateHttpException(e);
      }
    }


    #endregion Update methods

  }  // class ProjectTemplateController

}  // namespace Empiria.ProjectManagement.Templates.WebApi
