/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Domain Types                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Information Holder                      *
*  Type     : Step                                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Abstract class that describes a process, project or protocol step.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Ontology;

using Empiria.Steps.Design.Integration;

namespace Empiria.Steps.Design {

  /// <summary>Abstract class that describes a process, project or protocol step.</summary>
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


    public string Name {
      get {
        return this.DataHolder.StepName;
      }
      set {
        this.DataHolder.StepName = EmpiriaString.TrimAll(value);
      }
    }


    public string Kind {
      get {
        return this.DataHolder.StepKind;
      }
      set {
        this.DataHolder.StepKind = EmpiriaString.TrimAll(value);
      }
    }


    public string Notes {
      get {
        return this.DataHolder.Notes;
      }
      set {
        this.DataHolder.Notes = EmpiriaString.TrimAll(value);
      }
    }


    public string Theme {
      get {
        return this.DataHolder.Themes;
      }
      set {
        this.DataHolder.Themes = EmpiriaString.TrimAll(value);
      }
    }


    public string Accesibility {
      get {
        return this.DataHolder.Accessibility;
      }
      set {
        this.DataHolder.Accessibility = EmpiriaString.TrimAll(value);
      }
    }


    [DataObject]
    internal StepDataHolder DataHolder {
      get;
      private set;
    }

    #endregion Properties

  }  // class Step

}  // namespace Empiria.Steps.Design
