/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design                               Component : Domain Layer                            *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Information Holder                      *
*  Type     : Block                                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a list of activities.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Design {

  /// <summary>Represents a list of activities.</summary>
  public class Block : Step {

    #region Constructors and parsers

    internal Block() : base(StepType.Block) {
      // no-op
    }

    private Block(StepType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new Block Parse(string uid) {
      return BaseObject.ParseKey<Block>(uid);
    }

    #endregion Constructors and parsers

  }  // class Block

}  // namespace Empiria.Steps.Design
