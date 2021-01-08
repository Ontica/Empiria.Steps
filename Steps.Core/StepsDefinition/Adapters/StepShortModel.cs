/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Definition                           Component : Interface adapters                      *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Data Transfer Object                    *
*  Type     : StepShortModel                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO that holds minimal step definition data to be used as list items.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Steps.Definition.Adapters {

  /// <summary>Output DTO that holds minimal step definition data to be used as list items.</summary>
  public class StepShortModel {

    public string UID {
      get; internal set;
    }

    public string Type {
      get; internal set;
    }

    public string TypeName {
      get; internal set;
    }

    public string Kind {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Topics {
      get; internal set;
    }

    public string Tags {
      get; internal set;
    }

    public string Entity {
      get; internal set;
    }

  }  // class StepShortModel

}  // namespace Empiria.Steps.Definition.Adapters
