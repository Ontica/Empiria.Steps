/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : ProjectsModelsController                         License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Public API to get and set workflow definition objects.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.ProjectManagement;

namespace Empiria.Steps.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class ProjectsModelsController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/projects/process-models/for-activities")]
    public CollectionModel GetProcessModelsForActivities() {
      try {

        var list = ProjectModel.GetActivitiesModelsList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(ProjectModel).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/projects/process-models/for-events")]
    public CollectionModel GetProcessModelsForEvents() {
      try {

        var list = ProjectModel.GetEventsModelsList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(ProjectModel).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

  }  // class ProjectsModelsController

}  // namespace Empiria.Steps.WebApi
