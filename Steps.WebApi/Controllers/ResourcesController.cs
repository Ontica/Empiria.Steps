/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : ResourcesController                              License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Public API to retrieve and set project resources.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.ProjectManagement;

namespace Empiria.Steps.WebApi {

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

}  // namespace Empiria.Steps.WebApi
