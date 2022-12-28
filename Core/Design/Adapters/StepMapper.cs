/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design                               Component : Interface adapters                      *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Mapper class                            *
*  Type     : StepMapper                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Methods used to map Steps Design    s to StepDto objects.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Design.Adapters {

  static internal class StepMapper {

    static internal FixedList<StepDto> Map(FixedList<Step> list) {
      var mappedItems = list.Select((x) => Map(x));

      return new FixedList<StepDto>(mappedItems);
    }


    static internal StepDto Map(Step step) {
      var dto = new StepDto {
        UID = step.UID,
        Type = step.StepType.Name,
        TypeName = step.StepType.DisplayName,
        Kind = step.Kind,
        Name = step.Name,
        Description = step.Description,
        Topics = step.Topics,
        Tags = step.Tags,
        Entity = step.Entity.ShortName
      };

      return dto;
    }


    static internal FixedList<StepDescriptorDto> MapToShortModel(FixedList<Step> list) {
      var mappedItems = list.Select((x) => MapToShortModel(x));

      return new FixedList<StepDescriptorDto>(mappedItems);
    }


    static internal StepDescriptorDto MapToShortModel(Step step) {
      var dto = new StepDescriptorDto {
        UID = step.UID,
        Type = step.StepType.Name,
        TypeName = step.StepType.DisplayName,
        Kind = step.Kind,
        Name = step.Name,
        Tags = step.Tags,
        Topics = step.Topics,
        Entity = step.Entity.ShortName
      };

      return dto;
    }

  }  // class StepMapper

}  // namespace Empiria.Steps.Design.Adapters
