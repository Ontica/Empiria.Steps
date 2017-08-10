/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : ProjectsController                               License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Public API to retrieve and set projects data.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Contacts;
using Empiria.Steps.ProjectManagement;

namespace Empiria.Steps.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class ProjectsController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/project-management/projects")]
    public CollectionModel GetProjectsList([FromUri] string filter = "") {
      try {
        var list = Project.GetList(filter ?? String.Empty);


        return new CollectionModel(this.Request, BuildResponse(list),
                                   typeof(Project).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/project-management/projects/{project_UID}/activities")]
    public CollectionModel GetProjectActivitiesList(string project_UID, [FromUri] string filter = "") {
      try {
        var project = Project.Parse(project_UID);

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
          parent = activity.Parent.IsEmptyInstance ? (int?) null : activity.Parent.Id
        };
        array.Add(item);
      }
      return array;
    }

    private ICollection BuildResponse(IList<Project> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var project in list) {
        var item = new {
          uid = project.UID,
          type = this.BuildResponse(project.ProjectType),
          name = project.Name,
          notes = project.Notes,
          owner = this.BuildResponse(project.Owner),
          manager = this.BuildResponse(project.Manager),
          //appliedTo = this.BuildResponse(project.AppliedTo),
          subprojects = this.BuildResponse(project.Subprojects),
          status = project.Status
        };
        array.Add(item);
      }
      return array;
    }

    private object BuildResponse(Contact contact) {
      return new {
        uid = contact.UID,
        name = contact.FullName,
        shortName = contact.Nickname
      };
    }

    private object BuildResponse(Resource resource) {
      return new {
        uid = resource.UID,
        type= BuildResponse(resource.ResourceType),
        name = resource.Name,
        notes = resource.Notes
      };
    }

    private object BuildResponse(ProjectType projectType) {
      return new {
        id = projectType.Id,
        name = projectType.DisplayName
      };
    }

    private object BuildResponse(ResourceType resourceType) {
      return new {
        id = resourceType.Id,
        name = resourceType.DisplayName
      };
    }

    #endregion Private methods

  }  // class ProjectsController

}  // namespace Empiria.Steps.WebApi
