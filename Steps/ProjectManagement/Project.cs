/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Project                                          License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project, as as set of well defined activities.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Describes a project, as as set of well defined activities.</summary>
  public class Project : GeneralObject {

    #region Constructors and parsers

    private Project() {
      // Required by Empiria Framework.
    }

    static public Project Empty {
      get {
        return BaseObject.ParseEmpty<Project>();
      }
    }

    static public FixedList<Project> GetList() {
      var list = BaseObject.GetList<Project>();

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      list.Insert(0, Project.Empty);

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

  } // class Project

} // namespace Empiria.Steps.ProjectManagement
