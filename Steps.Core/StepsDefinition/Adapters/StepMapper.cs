/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Definition                           Component : Interface adapters                      *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Mapper class                            *
*  Type     : StepMapper                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Methods used to map steps definitions to StepDto objects.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Definition.Adapters {

  static internal class StepMapper {

    static internal FixedList<StepDto> Map(FixedList<Process> list) {
      var mappedItems = list.Select((x) => Map(x));

      return new FixedList<StepDto>(mappedItems);
    }

    internal static FixedList<StepShortModel> MapToShortModel(FixedList<Process> list) {
      var mappedItems = list.Select((x) => MapToShortModel(x));

      return new FixedList<StepShortModel>(mappedItems);
    }

    static internal StepDto Map(Process process) {
      var dto = new StepDto {
        UID = process.UID,
        Type = process.StepType.Name,
        TypeName = process.StepType.DisplayName,
        Kind = process.Kind,
        Name = process.Name,
        Description = process.Description,
        Topics = process.Topics,
        Tags = process.Tags,
        Entity = process.Entity.Alias
      };

      return dto;
    }

    static internal StepShortModel MapToShortModel(Process process) {
      var dto = new StepShortModel {
        UID = process.UID,
        Type = process.StepType.Name,
        TypeName = process.StepType.DisplayName,
        Kind = process.Kind,
        Name = process.Name,
        Description = process.Description,
        Topics = process.Topics,
        Tags = process.Tags,
        Entity = process.Entity.Alias
      };

      return dto;
    }


  }  // class StepMapper

}  // namespace Empiria.Steps.Definition.Adapters
