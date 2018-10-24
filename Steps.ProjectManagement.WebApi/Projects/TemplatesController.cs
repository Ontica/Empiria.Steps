/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : TemplatesController                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API to retrieve and set projects templates data.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class TemplatesController : WebApiController {

    #region Get methods

    [HttpGet]
    [Route("v1/project-management/templates")]
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
    [Route("v1/project-management/templates/events")]
    public CollectionModel GetEventsList() {
      try {
        var list = Project.GetEventsList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Project).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Get methods

    #region Update methods

    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/create-from-event")]
    public SingleObjectModel CreateFromEvent(string projectUID,
                                            [FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var eventModel = bodyAsJson.Get<Activity>("eventUID");
        var eventDate = bodyAsJson.Get<DateTime>("eventDate", DateTime.Today);

        var project = Project.Parse(projectUID);

        var handler = new ActivityCreator(project);

        handler.CreateFromEvent(eventModel, eventDate);

        return new SingleObjectModel(this.Request, eventModel.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion Update methods

  }  // class TemplatesController

}  // namespace Empiria.ProjectManagement.WebApi
