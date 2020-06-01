/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Domain Types                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Information Holder                      *
*  Type     : Dependency                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a dependency between two step objects.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Design {

  /// <summary>DDescribes a dependency between two step objects.</summary>
  public class Dependency : StepRelation {

    #region Constructors and parsers

    internal Dependency() : base(StepRelationType.Dependency) {
      // no-op
    }


    private Dependency(StepRelationType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public new Dependency Parse(string uid) {
      return BaseObject.ParseKey<Dependency>(uid);
    }

    #endregion Constructors and parsers

  }  // class Dependency

}  // namespace Empiria.Steps.Design
