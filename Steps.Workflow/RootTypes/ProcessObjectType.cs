/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Process Definition                           Component : Domain services                       *
*  Assembly : Empiria.Workflow.dll                         Pattern   : Power type                            *
*  Type     : ProcessObjectType                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Power type that defines a process object type: an activity, event or gateway.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Workflow.Definition {

  /// <summary>Power type that defines a process object type: an activity, event or gateway.</summary>
  [Powertype(typeof(BaseProcessObject))]
  public sealed class ProcessObjectType : Powertype {

    #region Constructors and parsers

    private ProcessObjectType() {
      // Empiria power types always have this constructor.
    }

    static public new ProcessObjectType Parse(int typeId) {
      return ObjectTypeInfo.Parse<ProcessObjectType>(typeId);
    }

    static internal new ProcessObjectType Parse(string typeName) {
      return ObjectTypeInfo.Parse<ProcessObjectType>(typeName);
    }

    #endregion Constructors and parsers

    #region Types constants

    public static ProcessObjectType ActivityType {
      get {
        return ObjectTypeInfo.Parse<ProcessObjectType>("ObjectType.ProjectItem.Activity");
      }
    }


    public static ProcessObjectType SummaryType {
      get {
        return ObjectTypeInfo.Parse<ProcessObjectType>("ObjectType.ProjectItem.Summary");
      }
    }

    public static ProcessObjectType TaskType {
      get {
        return ObjectTypeInfo.Parse<ProcessObjectType>("ObjectType.ProjectItem.Task");
      }
    }

    #endregion Types constants

  } // class ProcessObjectType

} // namespace Empiria.Workflow.Definition
