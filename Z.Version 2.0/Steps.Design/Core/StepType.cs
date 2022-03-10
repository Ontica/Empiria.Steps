/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Domain Types                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Power type                              *
*  Type     : StepType                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that defines a step object type: process, task, summary, event, gateway, etc.       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Steps.Design {

  /// <summary>Power type that defines a step object type: process, task,
  /// summary, event, gateway, etc.</summary>
  [Powertype(typeof(Step))]
  public sealed class StepType : Powertype {

    #region Constructors and parsers

    private StepType() {
      // Empiria power types always have this constructor.
    }

    static public new StepType Parse(int typeId) {
      return ObjectTypeInfo.Parse<StepType>(typeId);
    }

    static internal new StepType Parse(string typeName) {
      return ObjectTypeInfo.Parse<StepType>(typeName);
    }

    #endregion Constructors and parsers

    #region Types constants

    public static StepType Block => ObjectTypeInfo.Parse<StepType>("ObjectType.Step.Block");


    public static StepType Event => ObjectTypeInfo.Parse<StepType>("ObjectType.Step.Event");


    public static StepType Gateway => ObjectTypeInfo.Parse<StepType>("ObjectType.Step.Gateway");


    public static StepType Process => ObjectTypeInfo.Parse<StepType>("ObjectType.Step.Process");


    public static StepType Section => ObjectTypeInfo.Parse<StepType>("ObjectType.Step.Section");


    public static StepType StepCaller => ObjectTypeInfo.Parse<StepType>("ObjectType.Step.StepCaller");


    public static StepType Task => ObjectTypeInfo.Parse<StepType>("ObjectType.Step.Task");

    #endregion Types constants

  } // class StepType

} // namespace Empiria.Steps.Design
