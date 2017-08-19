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

using Empiria.Json;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Contacts;
using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Steps.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class ProjectsModelsController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/projects/process-models")]
    public CollectionModel GetProjectsProcessModelsList() {
      try {

        var list = Process.GetList();

        return new CollectionModel(this.Request, BuildResponse(list),
                                   typeof(Process).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion Public APIs

    #region Private methods

    private ICollection BuildResponse(IList<Process> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var process in list) {
        var item = new {
          uid = process.UID,
          type = process.WorkflowObjectType.Name,
          name = process.Name,
          notes = process.Notes,
          ownerUID = process.Owner.UID,
          resourceTypeId = process.ResourceType.Id,
          status = process.Status,
          links = process.Links
        };
        array.Add(item);
      }
      return array;
    }

    #endregion Private methods

  }  // class ProjectsModelsController

}  // namespace Empiria.Steps.WebApi
