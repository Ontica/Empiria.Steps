﻿/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : ResourcesController                              License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Public API to retrieve and set project resources.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Contacts;
using Empiria.Steps.ProjectManagement;

namespace Empiria.Steps.WebApi {

  /// <summary>Public API to retrieve and set project resources.</summary>
  public class ResourcesController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/project-management/resources")]
    public CollectionModel GetResourcesList([FromUri] string filter = "") {
      try {
        var list = Resource.GetList();

        return new CollectionModel(this.Request, BuildResponse(list),
                                   typeof(Project).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/project-management/resources/{project_UID}/activities")]
    public CollectionModel GetProjectActivitiesList(string project_UID,
                                                    [FromUri] string filter = "") {
      try {
        var project = Project.Parse(project_UID);

        return new CollectionModel(this.Request, BuildActivitiesResponse(project.Activities),
                                   typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPost]
    [Route("v1/project-management/resources/{project_UID}/activities")]
    public CollectionModel AppendActivity(string project_UID,
                                          [FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(project_UID);

        project.AddActivity(bodyAsJson);

        return new CollectionModel(this.Request, BuildActivitiesResponse(project.Activities),
                                   typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

    #region Private methods

    private ICollection BuildActivitiesResponse(IList<ProjectItem> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var activity in list) {
        var item = new {
          id = activity.Id,
          type = activity.ProjectItemType.Name,
          text = activity.Name,
          start_date = activity.EstimatedStart.ToString("yyyy-MM-dd HH:mm"),
          duration = activity.EstimatedEnd.Subtract(activity.EstimatedStart).Days,
          progress = activity.CompletionProgress,
          parent = activity.Parent.IsEmptyInstance ? 0 : activity.Parent.Id
        };
        array.Add(item);
      }
      return array;
    }

    private ICollection BuildResponse(IList<Resource> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var resource in list) {
        var item = new {
          uid = resource.UID,
          type = this.BuildResponse(resource.ResourceType),
          name = resource.Name,
          notes = resource.Notes,
        };
        array.Add(item);
      }
      return array;
    }

    private object BuildResponse(ResourceType resourceType) {
      return new {
        id = resourceType.Id,
        name = resourceType.DisplayName
      };
    }

    #endregion Private methods

  }  // class ResourcesController

}  // namespace Empiria.Steps.WebApi
