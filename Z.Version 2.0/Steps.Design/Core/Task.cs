/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Domain Types                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Information Holder                      *
*  Type     : Task                                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a process task.                                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Design {

  /// <summary>Describes a process task.</summary>
  public class Task : Step {

    #region Constructors and parsers

    internal Task() : base(StepType.Task) {
      // no-op
    }


    private Task(StepType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public new Task Parse(string uid) {
      return BaseObject.ParseKey<Task>(uid);
    }

    #endregion Constructors and parsers

  }  // class Task

}  // namespace Empiria.Steps.Design
