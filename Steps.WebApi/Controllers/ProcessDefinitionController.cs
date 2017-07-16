/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : ProcessDefinitionController                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Provides services that gets process definition models.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.Modeling;
using System.Collections;

namespace Empiria.Steps.WebApi {

  /// <summary>Provides services that gets or sets process definition models.</summary>
  public class ProcessDefinitionController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/process-definitions")]
    public CollectionModel GetProcessDefinitionList() {
      try {
        var list = ProcessDefinition.GetList();

        return new CollectionModel(this.Request, this.BuildResponse(list), typeof(ProcessDefinition).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/process-definitions/{processDef_ID}")]
    public SingleObjectModel GetProcessDefinition(string processDef_ID) {
      try {
        base.RequireResource(processDef_ID, "processDef_ID");

        var processDefinition = ProcessDefinition.Parse(processDef_ID);

        return new SingleObjectModel(this.Request, processDefinition);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPost]
    [Route("v1/process-definitions")]
    public SingleObjectModel CreateProcessDefinition([FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var processDefinition = new ProcessDefinition(bodyAsJson);

        processDefinition.Save();

        return new SingleObjectModel(this.Request, processDefinition);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPut, HttpPatch]
    [Route("v1/process-definitions/{processDef_ID}")]
    public SingleObjectModel UpdateProcessDefinition(string processDef_ID, [FromBody] object body) {
      try {
        base.RequireResource(processDef_ID, "processDef_ID");
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var processDefinition = ProcessDefinition.Parse(processDef_ID);

        processDefinition.Update(bodyAsJson);

        processDefinition.Save();

        return new SingleObjectModel(this.Request, processDefinition);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

    private ICollection BuildResponse(FixedList<ProcessDefinition> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var processDefinition in list) {
        var item = new {
          uid = processDefinition.UID,
          name = processDefinition.Name,
          version = processDefinition.Version,
        };
        array.Add(item);
      }
      return array;
    }

  }  // class ProcessDefinitionController

}  // namespace Empiria.Steps.WebApi
