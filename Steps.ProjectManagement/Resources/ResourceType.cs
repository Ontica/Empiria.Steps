/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Power type                          *
*  Type     : ResourceType                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Powertype that defines a resource such as a product, service or result.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.ProjectManagement.Resources {

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

    #region Types constants

    public static ResourceType Empty {
      get {
        return ObjectTypeInfo.Parse<ResourceType>("ObjectType.Resource.Empty");
      }
    }

    #endregion Types constants

  } // class ResourceType

} // namespace Empiria.ProjectManagement.Resources
