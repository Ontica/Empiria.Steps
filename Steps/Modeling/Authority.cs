/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Authority                                        License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes a government department, agency or office.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Json;

using Empiria.Contacts;

namespace Empiria.Steps.Modeling {

  public class Authority : Organization {

    #region Constructors and parsers

    private Authority() {
      // Required by Empiria Framework.
    }

    static public Authority Parse(string uid) {
      return BaseObject.ParseKey<Authority>(uid);
    }

    #endregion Constructors and parsers

    #region Properties

    private Position _headPosition = null;
    public Position HeadPosition {
      get {
        if (_headPosition == null) {
          _headPosition = base.GetLink<Position>("Authority->HeadPosition");
        }
        return _headPosition;
      }
    }

    #endregion Properties

  }  // class Authority

}  // namespace Empiria.Steps.Modeling
