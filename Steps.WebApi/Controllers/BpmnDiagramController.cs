/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : BpmnDiagramController                            License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Provides services for read and write BPMN diagrams.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using System.Collections;
using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Steps.WebApi {

  /// <summary>Provides services for read and write BPMN diagrams.</summary>
  public class BpmnDiagramController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/process-definitions")]
    public CollectionModel GetProcessDefinitionList() {
      try {
        var list = BpmnDiagram.GetList();

        return new CollectionModel(this.Request, this.BuildResponse(list), typeof(BpmnDiagram).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/process-definitions/{processDef_ID}")]
    public SingleObjectModel GetProcessDefinition(string processDef_ID) {
      try {
        base.RequireResource(processDef_ID, "processDef_ID");

        var processDefinition = BpmnDiagram.Parse(processDef_ID);

        return new SingleObjectModel(this.Request, BuildResponse(processDefinition),
                                     typeof(BpmnDiagram).FullName);

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

        var processDefinition = new BpmnDiagram(bodyAsJson);

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

        var processDefinition = BpmnDiagram.Parse(processDef_ID);

        processDefinition.Update(bodyAsJson);

        processDefinition.Save();

        return new SingleObjectModel(this.Request, processDefinition);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

    private ICollection BuildResponse(FixedList<BpmnDiagram> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var processDefinition in list) {
        var item = new {
          uid = processDefinition.UID,
          name = processDefinition.Name,
          version = "1.0",
        };
        array.Add(item);
      }
      return array;
    }

    private object BuildResponse(BpmnDiagram bpmnDiagram) {
      return new {
          uid = bpmnDiagram.UID,
          name = bpmnDiagram.Name,
          version = "1.0",
          bpmnXml = bpmnDiagram.Xml,
      };
    }

  }  // class BpmnDiagramController

}  // namespace Empiria.Steps.WebApi
