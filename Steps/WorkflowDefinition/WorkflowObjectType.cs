/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.dll                                Pattern : Power type                          *
*  Type     : WorkflowObjectType                               License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Powertype that defines a workflow object type.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

namespace Empiria.Steps.WorkflowDefinition {

  /// <summary>Powertype that defines a workflow object type.</summary>
  [Powertype(typeof(WorkflowObject))]
  public sealed class WorkflowObjectType : Powertype {

    #region Constructors and parsers

    private WorkflowObjectType() {
      // Empiria powertypes always have this constructor.
    }

    static public new WorkflowObjectType Parse(int typeId) {
      return ObjectTypeInfo.Parse<WorkflowObjectType>(typeId);
    }

    static internal new WorkflowObjectType Parse(string typeName) {
      return ObjectTypeInfo.Parse<WorkflowObjectType>(typeName);
    }

    #endregion Constructors and parsers

    #region Types constants

    public static WorkflowObjectType Activity {
      get {
        return ObjectTypeInfo.Parse<WorkflowObjectType>("ObjectType.WorkflowObject.Activity");
      }
    }

    public static WorkflowObjectType Event {
      get {
        return ObjectTypeInfo.Parse<WorkflowObjectType>("ObjectType.WorkflowObject.Event");
      }
    }

    public static WorkflowObjectType Process {
      get {
        return ObjectTypeInfo.Parse<WorkflowObjectType>("ObjectType.WorkflowObject.Process");
      }
    }

    public static WorkflowObjectType Task {
      get {
        return ObjectTypeInfo.Parse<WorkflowObjectType>("ObjectType.WorkflowObject.Task");
      }
    }

    #endregion Types constants

  } // class WorkflowObjectType

} // namespace Empiria.Steps.WorkflowDefinition
