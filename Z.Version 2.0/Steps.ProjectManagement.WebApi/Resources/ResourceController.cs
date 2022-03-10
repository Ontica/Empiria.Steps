/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ResourceController                           License   : Please read LICENSE.txt file          *
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
  public class ResourceController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/project-management/resources")]
    public CollectionModel GetResourcesList() {
      try {
        var list = Project.ResourcesList;

        return new CollectionModel(this.Request, list);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    //[HttpGet]
    //[Route("v1/project-management/projects/{projectUID}/resources")]
    //public CollectionModel GetProjectResourcesList(string projectUID) {
    //  try {
    //    var list = Resource.GetList();

    //    return new CollectionModel(this.Request, list.ToResponse(),
    //                               typeof(Project).FullName);

    //  } catch (Exception e) {
    //    throw base.CreateHttpException(e);
    //  }
    //}


    [HttpGet]
    [Route("v1/project-management/tags")]
    public CollectionModel GetTagsList() {
      try {

        var tags = Project.TagsList;

        return new CollectionModel(this.Request, tags);

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

  }  // class ResourceController

}  // namespace Empiria.ProjectManagement.Resources.WebApi
