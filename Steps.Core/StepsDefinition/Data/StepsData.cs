/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Definition                           Component : Data Access Layer                       *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Data Services                           *
*  Type     : StepsData                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data read and write services for steps definition data.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

using Empiria.Steps.Definition.Adapters;

namespace Empiria.Steps.Definition.Data {

  /// <summary>Data read and write services for steps definition data.</summary>
  static internal class StepsData {

    static internal FixedList<Process> GetProcessList(SearchStepsCommand searchCommand) {
      var processType = StepType.Process;

      var sql = $"SELECT * FROM STSteps WHERE " +
                $"(StepTypeId = {processType.Id}) AND (DesignStatus <> 'X')";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Process>(op);
    }


    static internal FixedList<Step> GetStepsList(SearchStepsCommand searchCommand) {
      var sql = $"SELECT * FROM STSteps WHERE " +
                $"(DesignStatus <> 'X')";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<Step>(op);

    }
  }  // class StepsData

}  // namespace Empiria.Steps.Definition.Data
