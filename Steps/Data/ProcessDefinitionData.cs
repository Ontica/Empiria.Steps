/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Data Service                        *
*  Type     : ProcessDefinitionData                            License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Process definition data read and write methods.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Steps.Modeling {

  static internal class ProcessDefinitionData {

    static internal FixedList<ProcessDefinition> GetProcessDefinitionList() {
      DataTable table = GeneralDataOperations.GetEntities("BPMProcessDefinitions",
                                                          sortExpression: "Name, Version");

      return BaseObject.ParseList<ProcessDefinition>(table).ToFixedList();
    }

    static internal void WriteProcessDefinition(ProcessDefinition o) {
      var op = DataOperation.Parse("writeBPMProcessDefinition",
                                    o.Id, o.UID, o.Name, o.Version, o.BpmnXml);

      DataWriter.Execute(op);
    }

  }  // class ProcessDefinitionData

}  // namespace Empiria.Steps.Modeling
