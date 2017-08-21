/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Response methods                    *
*  Type     : BpmnDiagramResponseModel                         License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Response static methods for BPMN diagrams.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Steps.WebApi {

  /// <summary>Response static methods for BPMN diagrams.</summary>
  static internal class BpmnDiagramResponseModel {

    static internal ICollection ToResponse(this IList<BpmnDiagram> list) {
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

    static internal object ToResponse(this BpmnDiagram bpmnDiagram) {
      return new {
        uid = bpmnDiagram.UID,
        name = bpmnDiagram.Name,
        version = "1.0",
        xml = bpmnDiagram.Xml,
      };
    }

  }  // class BpmnDiagramResponseModel

}  // namespace Empiria.Steps.WebApi
