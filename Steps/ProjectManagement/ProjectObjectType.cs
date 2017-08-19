/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.dll                                Pattern : Power type                          *
*  Type     : ProjectObjectType                                License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Power type that defines a project object type.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Power type that defines a project object type
  /// (a project, a project activity, an event, a milestone, deadlines, etc).</summary>
  [Powertype(typeof(ProjectObject))]
  public sealed class ProjectObjectType : Powertype {

    #region Constructors and parsers

    private ProjectObjectType() {
      // Empiria power types always have this constructor.
    }

    static public new ProjectObjectType Parse(int typeId) {
      return ObjectTypeInfo.Parse<ProjectObjectType>(typeId);
    }

    static internal new ProjectObjectType Parse(string typeName) {
      return ObjectTypeInfo.Parse<ProjectObjectType>(typeName);
    }

    #endregion Constructors and parsers

    #region Types constants

    public static ProjectObjectType ActivityType {
      get {
        return ObjectTypeInfo.Parse<ProjectObjectType>("ObjectType.ProjectObject.Activity");
      }
    }

    public static ProjectObjectType ProjectType {
      get {
        return ObjectTypeInfo.Parse<ProjectObjectType>("ObjectType.ProjectObject.Project");
      }
    }

    public static ProjectObjectType TaskType {
      get {
        return ObjectTypeInfo.Parse<ProjectObjectType>("ObjectType.ProjectObject.Task");
      }
    }

    #endregion Types constants

  } // class ProjectObjectType

} // namespace Empiria.Steps.ProjectManagement
