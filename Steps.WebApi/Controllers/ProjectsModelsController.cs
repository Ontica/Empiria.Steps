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
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.ProjectManagement;
using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Steps.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class ProjectsModelsController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/projects/activities-models")]
    public CollectionModel GetProjectsActivityModelsList() {
      try {

        var list = ProjectModel.GetActivitiesModelsList();

        return new CollectionModel(this.Request, BuildResponse(list),
                                   typeof(ProjectModel).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/projects/events-models")]
    public CollectionModel GetProjectsEventsModelsList() {
      try {

        var list = ProjectModel.GetEventsModelsList();

        return new CollectionModel(this.Request, BuildResponse(list),
                                   typeof(ProjectModel).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

    #region Private methods

    private ICollection BuildResponse(IList<ProjectModel> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var model in list) {
        var process = model.BaseProcess;
        var item = new {
          uid = process.UID,
          type = process.WorkflowObjectType.Name,
          name = process.Name,
          notes = process.Notes,
          ownerUID = process.Owner.UID,
          resourceTypeId = process.ResourceType.Id,
          links = process.Links,
          steps = BuildResponse(model.Steps)
        };
        array.Add(item);
      }
      return array;
    }

    private ICollection BuildResponse(IList<ProcessActivity> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var activity in list) {
        var item = new {
          uid = activity.UID,
          taskType = activity.TaskType,
          involvedParty = activity.InvolvedParty.IsEmptyInstance
                                        ? String.Empty : activity.InvolvedParty.Alias,
          stepNo = activity.InnerTag,
          name = activity.Name,
          notes = activity.Notes,
          ownerUID = activity.Owner.UID,
          resourceTypeId = activity.ResourceType.Id,
          status = activity.Status,
          links = activity.Links
        };
        array.Add(item);
      }
      return array;
    }

    #endregion Private methods

  }  // class ProjectsModelsController

}  // namespace Empiria.Steps.WebApi
