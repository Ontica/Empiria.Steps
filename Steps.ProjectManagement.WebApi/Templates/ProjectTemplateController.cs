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
        var insertionRule  = bodyAsJson.Get<TreeItemInsertionRule>("insertionPosition",
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


    #endregion Update methods

  }  // class ProjectTemplateController

}  // namespace Empiria.ProjectManagement.Templates.WebApi
