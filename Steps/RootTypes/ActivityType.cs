/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : ActivityType                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes the type of an activity.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps {

  /// <summary>Describes the type of an activity.</summary>
  public class ActivityType : GeneralObject {

    #region Constructors and parsers

    private ActivityType() {
      // Required by Empiria Framework.
    }

    static public ActivityType Empty {
      get {
        return BaseObject.ParseEmpty<ActivityType>();
      }
    }

    static public ActivityType NA {
      get {
        return BaseObject.ParseId<ActivityType>(-2);
      }
    }

    static public FixedList<ActivityType> GetList() {
      var list = BaseObject.GetList<ActivityType>();

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      list.Insert(0, ActivityType.NA);
      list.Insert(0, ActivityType.Empty);

      return list.ToFixedList();
    }

    #endregion Constructors and parsers

    #region Public properties

    public string UID {
      get {
        return base.NamedKey;
      }
    }

    #endregion Public properties

  } // class ActivityType

} // namespace Empiria.Steps
