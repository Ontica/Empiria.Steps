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

  internal static class ProcessDefinitionData {

    static internal FixedList<ProcessDefinition> GetProcessDefinitionList() {
      DataTable table = GeneralDataOperations.GetEntities("BPMProcessDefinitions");

      return BaseObject.ParseList<ProcessDefinition>(table).ToFixedList();
    }

  }  // class ProcessDefinitionData

}  // namespace Empiria.Steps.Modeling
