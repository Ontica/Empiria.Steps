/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : BpmnDiagram                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Provides services that gets process definition models.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Json;

namespace Empiria.Steps.WorkflowDefinition {

  public class BpmnDiagram : BaseObject {

    #region Constructors and parsers

    private BpmnDiagram() {
      // Required by Empiria Framework.
    }

    public BpmnDiagram(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.AssertIsValid(data);
      this.Load(data);
    }

    static public BpmnDiagram Parse(string uid) {
      return BaseObject.ParseKey<BpmnDiagram>(uid);
    }

    static public FixedList<BpmnDiagram> GetList() {
      return BaseObject.GetList<BpmnDiagram>(sort: "ObjectName")
                       .ToFixedList();
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("ObjectKey")]
    public string UID {
      get;
      private set;
    }


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


    [DataField("ObjectStatus", Default = ObjectStatus.Active)]
    public ObjectStatus Status {
      get;
      private set;
    }

    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Name);
      }
    }

    #endregion Public properties

    #region Public methods


    protected virtual void AssertIsValid(JsonObject data) {
      Assertion.AssertObject(data, "data");

    }


    protected virtual void Load(JsonObject data) {
      this.Name = data.GetClean("name", this.Name);
      this.Xml = data.Get<string>("xml", String.Empty);
    }


    protected override void OnBeforeSave() {
      if (this.IsNew) {
        this.UID = EmpiriaString.BuildRandomString(32);
      }
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

}  // namespace Empiria.Steps.WorkflowDefinition
