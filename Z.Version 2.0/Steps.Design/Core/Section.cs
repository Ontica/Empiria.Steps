/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Domain Types                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Information Holder                      *
*  Type     : Section                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a section or block of process tasks.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Design {

  /// <summary>Describes a section or block of process tasks.</summary>
  public class Section : Step {

    #region Constructors and parsers

    internal Section() : base(StepType.Section) {
      // no-op
    }


    private Section(StepType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public new Section Parse(string uid) {
      return BaseObject.ParseKey<Section>(uid);
    }

    #endregion Constructors and parsers

  }  // class Section

}  // namespace Empiria.Steps.Design
