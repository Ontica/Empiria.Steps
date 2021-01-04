/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Definition                           Component : Domain Layer                            *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Information Holder                      *
*  Type     : Process                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a process definition.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Definition {

  /// <summary>Describes a process definition.</summary>
  internal class Process : Step {

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

  }  // class Process

}  // namespace Empiria.Steps.Definition
