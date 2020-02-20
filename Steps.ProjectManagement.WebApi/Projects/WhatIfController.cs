/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : WhatIfController                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API to retrieve a list of activity state updates if an operation is performed.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

using Empiria.ProjectManagement.Services;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Web API to retrieve a list of activity state updates if an operation is performed.</summary>
  public class WhatIfController : WebApiController {

    #region Get methods


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/what-if-completed")]
    public SingleObjectModel WhatIfCompleted(string projectUID, string activityUID,
                                             [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);

        Activity activity = project.GetActivity(activityUID);

        DateTime completedDate = bodyAsJson.Get<DateTime>("completedDate", DateTime.Today);

        WhatIfResult result = ModelingServices.WhatIfCompleted(activity, completedDate, true);

        return new SingleObjectModel(this.Request, result.ToResponse(),
                                     typeof(WhatIfResult).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/what-if-created-from-event")]
    public SingleObjectModel WhatIfCreatedFromEvent(string projectUID,
                                                   [FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var activityModel = bodyAsJson.Get<Activity>("activityTemplateUID");
        var eventDate = bodyAsJson.Get<DateTime>("eventDate", DateTime.Today);

        var project = Project.Parse(projectUID);

        WhatIfResult result = ModelingServices.WhatIfCreatedFromEvent(activityModel, project, eventDate);

        return new SingleObjectModel(this.Request, result.ToResponse(),
                                     typeof(WhatIfResult).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/activities/{activityUID}/what-if-reactivated")]
    public SingleObjectModel WhatIfReactivated(string projectUID, string activityUID) {
      try {
        var project = Project.Parse(projectUID);

        Activity activity = project.GetActivity(activityUID);

        WhatIfResult result = ModelingServices.WhatIfReactivated(activity);

        return new SingleObjectModel(this.Request, result.ToResponse(),
                                     typeof(WhatIfResult).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion Get methods

  }  // class WhatIfController

}  // namespace Empiria.ProjectManagement.WebApi
