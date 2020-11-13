/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Workflow Definition                 *
*  Assembly : Empiria.WorkflowDefinition.dll                   Pattern : Data Service                        *
*  Type     : WorkflowDefinitionData                           License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Data read and write methods for workflow objects.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Data;

namespace Empiria.Workflow.Definition {

  /// <summary>Data read and write methods for workflow objects.</summary>
  static internal class WorkflowDefinitionData {


    static internal List<Process> GetProcesses(string filter) {
      string sql = $"SELECT * FROM WFWorkflowObjects " +
                   $"WHERE WorkflowObjectTypeId = {WorkflowObjectType.Process.Id} " +
                   $"AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Process>(x));
    }


    static internal FixedList<BpmnDiagram> SearchBpmnDiagrams(string keywords,
                                                              string sort = "") {
      string filter = SearchExpression.ParseAndLikeKeywords("ObjectKeywords", keywords);

      sort = sort ?? "ObjectName";

      return BaseObject.GetList<BpmnDiagram>(filter, sort)
                       .ToFixedList();
    }

    #region Write data methods


    static internal void WriteBpmnDiagram(BpmnDiagram o) {
      var op = DataOperation.Parse("writeSimpleObject",
                      o.Id, o.GetEmpiriaType().Id, o.UID,
                      o.Name, o.Xml, o.Keywords, o.Status);

      DataWriter.Execute(op);
    }


    static internal void WriteProcess(Process o) {
      throw new NotImplementedException();
    }

    #endregion Write data methods

  }  // class WorkflowDefinitionData

}  // namespace Empiria.Workflow.Definition
