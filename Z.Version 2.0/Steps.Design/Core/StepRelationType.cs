/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Domain Types                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Power type                              *
*  Type     : StepRelationType                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that defines a step object relationship type: dependency, triggerOf, etc.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Steps.Design {

  /// <summary>Power type that defines a step object relationship type: dependency, triggerOf, etc.</summary>
  [Powertype(typeof(StepRelation))]
  public sealed class StepRelationType : Powertype {

    #region Constructors and parsers

    private StepRelationType() {
      // Empiria power types always have this constructor.
    }

    static public new StepRelationType Parse(int typeId) {
      return ObjectTypeInfo.Parse<StepRelationType>(typeId);
    }

    static internal new StepRelationType Parse(string typeName) {
      return ObjectTypeInfo.Parse<StepRelationType>(typeName);
    }

    #endregion Constructors and parsers

    #region Types constants

    public static StepRelationType Dependency => ObjectTypeInfo.Parse<StepRelationType>("ObjectType.StepRelation.Dependency");

    #endregion Types constants

  } // class StepRelationType

} // namespace Empiria.Steps.Design
