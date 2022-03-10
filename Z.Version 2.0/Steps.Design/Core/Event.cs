/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Domain Types                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Information Holder                      *
*  Type     : Event                                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a process event.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Design {

  /// <summary>Describes a process event.</summary>
  public class Event : Step {

    #region Constructors and parsers

    internal Event() : base(StepType.Event) {
      // no-op
    }


    private Event(StepType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public new Event Parse(string uid) {
      return BaseObject.ParseKey<Event>(uid);
    }

    #endregion Constructors and parsers

  }  // class Event

}  // namespace Empiria.Steps.Design
