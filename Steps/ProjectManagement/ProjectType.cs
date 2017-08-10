/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Power type                          *
*  Type     : ProjectType                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Power type that defines a project type.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Power type that defines a project type.</summary>
  [Powertype(typeof(Project))]
  public sealed class ProjectType : Powertype {

    #region Constructors and parsers

    private ProjectType() {
      // Empiria power types always have this constructor.
    }

    static public new ProjectType Parse(int typeId) {
      return ObjectTypeInfo.Parse<ProjectType>(typeId);
    }


    static internal new ProjectType Parse(string typeName) {
      return ObjectTypeInfo.Parse<ProjectType>(typeName);
    }

    #endregion Constructors and parsers

    #region Public methods

    /// <summary>Factory method to create Project instances of this ProjectType.</summary>
    internal Project CreateInstance() {
      return base.CreateObject<Project>();
    }

    #endregion Public methods

  } // class ProjectType

} // namespace Empiria.Steps.ProjectManagement
