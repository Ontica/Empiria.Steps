/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Definition                           Component : Domain Layer                            *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Information Holder                      *
*  Type     : Task                                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : A task represents a single unit of work that is not or cannot be broken down to a further      *
*             level of business process detail. It is referred to as an atomic activity.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Definition {

  /// <summary>Describes an atomic task.</summary>
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

}  // namespace Empiria.Steps.Definition
