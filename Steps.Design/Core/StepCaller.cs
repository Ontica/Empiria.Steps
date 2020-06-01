/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Domain Types                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Information Holder                      *
*  Type     : StepCaller                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a step caller.                                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Design {

  /// <summary>Describes a process model.</summary>
  public class StepCaller : Step {

    #region Constructors and parsers

    internal StepCaller() : base(StepType.StepCaller) {
      // no-op
    }


    private StepCaller(StepType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public new StepCaller Parse(string uid) {
      return BaseObject.ParseKey<StepCaller>(uid);
    }

    #endregion Constructors and parsers

  }  // class StepCaller

}  // namespace Empiria.Steps.Design
