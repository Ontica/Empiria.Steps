/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Definition                           Component : Domain Layer                            *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Partitioned Type / Information Holder   *
*  Type     : Step                                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract class that describes a process, a procedure, a protocol, a gateway, a task or other   *
*             kinds of atomic steps.                                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Contacts;
using Empiria.Ontology;
using Empiria.StateEnums;

namespace Empiria.Steps.Definition {

  /// <summary>Abstract class that describes a process, a procedure, a protocol, a gateway,
  /// a task or other kinds of atomic steps.</summary>
  [PartitionedType(typeof(StepType))]
  abstract public class Step : BaseObject {

    #region Constructors and parsers

    protected Step(StepType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public Step Parse(string uid) {
      return BaseObject.ParseKey<Step>(uid);
    }


    #endregion Constructors and parsers

    #region Properties

    public StepType StepType {
      get {
        return (StepType) base.GetEmpiriaType();
      }
    }

    [DataField("StepKind")]
    public string Kind {
      get;
      private set;
    }

    [DataField("StepName")]
    public string Name {
      get;
      private set;
    }

    [DataField("StepNotes")]
    public string Notes {
      get;
      private set;
    }

    [DataField("Themes")]
    public string Topics {
      get;
      private set;
    }

    [DataField("Tags")]
    public string Tags {
      get;
      private set;
    }

    [DataField("ProcedureEntityId")]
    public Contact Entity {
      get;
      private set;
    }

    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Name, this.StepType.DisplayName,
                                           this.Entity.Keywords, this.Topics, this.Tags, this.Notes);
      }
    }


    [DataField("Accessibility")]
    public string Accesibility {
      get;
      private set;
    }

    [DataField("OwnerId")]
    public int OwnerId {
      get;
      internal set;
    } = -1;


    [DataField("DesignStatus", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get;
      private set;
    } = EntityStatus.Pending;


    #endregion Properties

  }  // class Step

}  // namespace Empiria.Steps.Definition
