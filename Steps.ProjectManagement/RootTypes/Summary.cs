/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : Summary                                          License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project activity.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Json;

namespace Empiria.ProjectManagement {

  /// <summary>Describes a project activity.</summary>
  public class Summary : ProjectItem {

    #region Constructors and parsers

    protected Summary() : this(ProjectItemType.SummaryType) {

    }


    protected Summary(ProjectItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    protected internal Summary(Project project, JsonObject data) :
                               base(ProjectItemType.SummaryType, project, data) {

      this.AssertIsValid(data);
      this.Load(data);
    }


    static public new Activity Parse(string uid) {
      return BaseObject.ParseKey<Activity>(uid);
    }


    static internal new Activity Parse(int id) {
      return BaseObject.ParseId<Activity>(id);
    }


    static public new Activity Empty {
      get {
        return BaseObject.ParseEmpty<Activity>();
      }
    }


    #endregion Constructors and parsers

    #region Properties



    public new ProjectItem Parent {
      get {
        return base.Parent;
      }
    }

    #endregion Properties

    #region Public methods

    protected override void AssertIsValid(JsonObject data) {
      base.AssertIsValid(data);
    }


    protected override void Load(JsonObject data) {
      base.Load(data);
    }


    protected override void OnSave() {
      ProjectItemData.WriteSummary(this);
    }

    #endregion Public methods

  } // class Summary

} // namespace Empiria.ProjectManagement
