/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Position                                         License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes a government office position.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;

namespace Empiria.Steps.Modeling {

  /// <summary>Describes a government office position.</summary>
  public class Position : Contact {

    #region Constructors and parsers

    private Position() {
      // Required by Empiria Framework.
    }

    static public new Position Parse(string uid) {
      return BaseObject.ParseKey<Position>(uid);
    }

    static public new Position Empty {
      get {
        return BaseObject.ParseEmpty<Position>();
      }
    }

    #endregion Constructors and parsers

    #region Properties

    public string Phone {
      get {
        return base.ExtendedData.Get("Phone", "No determinado");
      }
    }


    private Person _officer = null;
    public Person Officer {
      get {
        if (_officer == null) {
          _officer = base.GetLink<Person>("Position->Officer", Person.Empty);
        }
        return _officer;
      }
    }

    #endregion Properties

  }  // class Position

}  // namespace Empiria.Steps.Modeling
