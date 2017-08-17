/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.dll                                Pattern : Power type                          *
*  Type     : ResourceType                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Powertype that defines a resource such as a product, service or result.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Steps {

  /// <summary>Powertype that defines a resource such as a product, service
  /// or result achieved by a project or used or defined by a workflow.</summary>
  [Powertype(typeof(Resource))]
  public sealed class ResourceType : Powertype {

    #region Constructors and parsers

    private ResourceType() {
      // Empiria power types always have this constructor.
    }

    static public new ResourceType Parse(int typeId) {
      return ObjectTypeInfo.Parse<ResourceType>(typeId);
    }

    static internal new ResourceType Parse(string typeName) {
      return ObjectTypeInfo.Parse<ResourceType>(typeName);
    }

    #endregion Constructors and parsers

    #region Public methods

    /// <summary>Factory method to create Resources instances of this ResourceType.</summary>
    internal Resource CreateInstance() {
      return base.CreateObject<Resource>();
    }

    #endregion Public methods

  } // class ResourceType

} // namespace Empiria.Steps
