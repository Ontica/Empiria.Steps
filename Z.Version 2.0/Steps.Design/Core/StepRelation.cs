/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Domain Types                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Information Holder                      *
*  Type     : StepRelation                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract class that describes a step relationship (eg. a work dependency).                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Steps.Design {

  /// <summary>Abstract class that describes a step relationship (eg. a work dependency).</summary>
  [PartitionedType(typeof(StepRelationType))]
  abstract public class StepRelation : BaseObject {

    #region Constructors and parsers

    protected StepRelation(StepRelationType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public StepRelation Parse(string uid) {
      return BaseObject.ParseKey<StepRelation>(uid);
    }

    #endregion Constructors and parsers

    #region Properties

    public StepRelationType RelationType {
      get {
        return (StepRelationType) base.GetEmpiriaType();
      }
    }

    #endregion Properties

  }  // class StepRelation

}  // namespace Empiria.Steps.Design
