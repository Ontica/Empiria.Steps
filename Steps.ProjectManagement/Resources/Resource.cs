﻿/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : Resource                                         License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes a project involved resource.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Ontology;

namespace Empiria.ProjectManagement.Resources {

  /// <summary>Describes a project involved resource.</summary>
  [PartitionedType(typeof(ResourceType))]
  public class Resource : BaseObject {

    #region Constructors and parsers

    protected Resource(ResourceType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public Resource Parse(int id) {
      return BaseObject.ParseId<Resource>(id);
    }

    static public Resource Parse(string uid) {
      return BaseObject.ParseKey<Resource>(uid);
    }

    static public Resource Empty {
      get {
        return BaseObject.ParseEmpty<Resource>();
      }
    }

    static public FixedList<Resource> GetList(string filter = "") {
      var owner = Contact.Parse(51);

      var list = ResourcesData.GetResourcesList(owner);

      return list.ToFixedList();
    }

    #endregion Constructors and parsers

    #region Public properties

    public ResourceType ResourceType {
      get {
        return (ResourceType) base.GetEmpiriaType();
      }
    }

    [DataField("Name")]
    public string Name {
      get;
      private set;
    }


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }

    #endregion Public properties

  } // class Resource

} // namespace Empiria.ProjectManagement.Resources
