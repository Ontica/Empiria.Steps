/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Entity                                           License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes a government agency or ministry.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Json;

using Empiria.Contacts;

namespace Empiria.Steps.Modeling {

  public class Entity : Organization {

    #region Constructors and parsers

    private Entity() {
      // Required by Empiria Framework.
    }

    static public Entity Parse(string uid) {
      return BaseObject.ParseKey<Entity>(uid);
    }

    static public new FixedList<Entity> GetList(string filter) {
      var list = BaseObject.GetList<Entity>(filter, "Nickname");
      return list.ToFixedList();
    }

    #endregion Constructors and parsers

    #region Properties

    private FixedList<Authority> _authoritiesList = null;
    public FixedList<Authority> Authorities {
      get {
        if (_authoritiesList == null) {
          _authoritiesList = base.GetLinks<Authority>("Entity->Authorities");

          _authoritiesList.Sort((x, y) => x.FullName.CompareTo(y.FullName));
        }
        return _authoritiesList;
      }
    }

    private FixedList<Position> _positionsList = null;
    public FixedList<Position> Positions {
      get {
        if (_positionsList == null) {
          _positionsList = base.GetLinks<Position>("Entity->Positions");

          _positionsList.Sort((x, y) => x.FullName.CompareTo(y.FullName));
        }
        return _positionsList;
      }
    }

    #endregion Properties

  }  // class Entity

}  // namespace Empiria.Steps.Modeling
