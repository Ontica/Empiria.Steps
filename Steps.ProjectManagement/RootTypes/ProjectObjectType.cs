/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Power type                          *
*  Type     : ProjectItemType                                  License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Power type that defines a project object type: a project activity, an event, a milestone, etc. *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.ProjectManagement {

  /// <summary>Power type that defines a project object type:
  /// a project activity, an event, a milestone, deadlines, etc.</summary>
  [Powertype(typeof(ProjectItem))]
  public sealed class ProjectItemType : Powertype {

    #region Constructors and parsers

    private ProjectItemType() {
      // Empiria power types always have this constructor.
    }

    static public new ProjectItemType Parse(int typeId) {
      return ObjectTypeInfo.Parse<ProjectItemType>(typeId);
    }

    static internal new ProjectItemType Parse(string typeName) {
      return ObjectTypeInfo.Parse<ProjectItemType>(typeName);
    }

    #endregion Constructors and parsers

    #region Types constants

    public static ProjectItemType ActivityType {
      get {
        return ObjectTypeInfo.Parse<ProjectItemType>("ObjectType.ProjectItem.Activity");
      }
    }


    public static ProjectItemType SummaryType {
      get {
        return ObjectTypeInfo.Parse<ProjectItemType>("ObjectType.ProjectItem.Summary");
      }
    }

    public static ProjectItemType TaskType {
      get {
        return ObjectTypeInfo.Parse<ProjectItemType>("ObjectType.ProjectItem.Task");
      }
    }

    #endregion Types constants

  } // class ProjectItemType

} // namespace Empiria.ProjectManagement
