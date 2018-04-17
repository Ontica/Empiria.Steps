/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ResourcesController                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API to retrieve and set project resources.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Collections;
using Empiria.WebApi;

namespace Empiria.ProjectManagement.Resources.WebApi {

  /// <summary>Public API to retrieve and set project resources.</summary>
  public class ResourcesController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/resources")]
    public CollectionModel GetResourcesList(string projectUID) {
      try {
        var list = Resource.GetList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Project).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/project-management/tags")]
    public CollectionModel GetTagsList() {
      try {

        var projects = Project.GetList();

        var tags = new TagsCollection();
        foreach (var project in projects) {
          tags.AddRange(project.Tags);
        }
        tags.Sort();

        return new CollectionModel(this.Request, tags.ToResponse(),
                                   typeof(Project).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    //[HttpGet]
    //[Route("v1/project-management/resources/as-tree")]
    //public CollectionModel GetResourcesAsTreeList([FromUri] string filter = "") {
    //  try {
    //    var tree = ResourceTree.Parse();

    //    return new CollectionModel(this.Request, tree.ToResponse(),
    //                               typeof(Project).FullName);

    //  } catch (Exception e) {
    //    throw base.CreateHttpException(e);
    //  }
    //}

    #endregion Public APIs

  }  // class ResourcesController

}  // namespace Empiria.ProjectManagement.Resources.WebApi
