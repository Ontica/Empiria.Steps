/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Workflow Definition                 *
*  Assembly : Empiria.Workflow.dll                             Pattern : Domain class                        *
*  Type     : BpmnDiagram                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Provides services that gets process definition models.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;
using Empiria.StateEnums;

namespace Empiria.Workflow.Definition {

  public class BpmnDiagram : BaseObject {

    #region Constructors and parsers

    private BpmnDiagram() {
      // Required by Empiria Framework.
    }

    public BpmnDiagram(string name, string xml, string tags) {
      Assertion.AssertObject(name, "name");
      Assertion.AssertObject(xml, "xml");

      this.Name = name;
      this.Xml = xml;
      this.Tags = tags;
    }

    public BpmnDiagram(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.AssertIsValid(data);
      this.Load(data);
    }

    static internal BpmnDiagram Parse(int id) {
      return BaseObject.ParseId<BpmnDiagram>(id);
    }

    static public BpmnDiagram Parse(string uid) {
      return BaseObject.ParseKey<BpmnDiagram>(uid);
    }

    static public BpmnDiagram Empty {
      get {
        return BaseObject.ParseEmpty<BpmnDiagram>();
      }
    }

    static public FixedList<BpmnDiagram> Search(string keywords) {
      return WorkflowDefinitionData.SearchBpmnDiagrams(keywords);
    }

    #endregion Constructors and parsers

    #region Public properties


    [DataField("ObjectName")]
    public string Name {
      get;
      private set;
    }


    [DataField("ObjectExtData")]
    public string Xml {
      get;
      private set;
    }


    public string Tags {
      get;
      private set;
    } = String.Empty;


    [DataField("ObjectStatus", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get;
      private set;
    }


    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Name, this.Tags);
      }
    }

    #endregion Public properties

    #region Public methods


    protected virtual void AssertIsValid(JsonObject data) {
      Assertion.AssertObject(data, "data");

    }

    protected virtual void Load(JsonObject data) {
      this.Name = data.GetClean("name", this.Name);
      this.Xml = data.GetClean("xml");
    }


    protected override void OnSave() {
      WorkflowDefinitionData.WriteBpmnDiagram(this);
    }


    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.AssertIsValid(data);
      this.Load(data);
    }


    #endregion Public methods

  }  // class BpmnDiagram

}  // namespace Empiria.Workflow.Definition
