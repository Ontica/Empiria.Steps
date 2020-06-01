/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Domain Types                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Information Holder                      *
*  Type     : Process                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a process model.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Design {

  /// <summary>Describes a process model.</summary>
  public class Process : Step {

    #region Constructors and parsers

    internal Process() : base(StepType.Process) {
      // no-op
    }


    private Process(StepType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public new Process Parse(string uid) {
      return BaseObject.ParseKey<Process>(uid);
    }


    #endregion Constructors and parsers

    #region Properties

    #endregion Properties

  }  // class Process

}  // namespace Empiria.Steps.Design
