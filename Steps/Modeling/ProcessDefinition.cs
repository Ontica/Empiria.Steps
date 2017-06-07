/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : ProcessDefinition                                License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Provides services that gets process definition models.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Json;

namespace Empiria.Steps.Modeling {

  public class ProcessDefinition : BaseObject {

    #region Constructors and parsers

    private ProcessDefinition() {
      // Required by Empiria Framework.
    }

    public ProcessDefinition(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Name = data.Get<string>("name");
      this.Version = data.Get("version", String.Empty);
      this.BpmnXml = data.Get<string>("bpmnXml", String.Empty);
    }

    static public ProcessDefinition Parse(string uid) {
      return BaseObject.ParseKey<ProcessDefinition>(uid);
    }

    static public FixedList<ProcessDefinition> GetList() {
      return ProcessDefinitionData.GetProcessDefinitionList();
    }

    #endregion Constructors and parsers

    #region Public properties


    [DataField("UID")]
    public string UID {
      get;
      private set;
    }


    [DataField("Name")]
    public string Name {
      get;
      private set;
    }


    [DataField("Version")]
    public string Version {
      get;
      private set;
    }


    [DataField("BpmnXML")]
    public string BpmnXml {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    protected override void OnBeforeSave() {
      if (this.IsNew) {
        this.UID = EmpiriaString.BuildRandomString(32);
      }
    }

    protected override void OnSave() {
      ProcessDefinitionData.WriteProcessDefinition(this);
    }

    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Name = data.Get<string>("name", this.Name);
      this.Version = data.Get<string>("version", this.Version);
      this.BpmnXml = data.Get<string>("bpmnXml", this.BpmnXml);
    }

    #endregion Public methods

  }  // class ProcessDefinition

}  // namespace Empiria.Steps.Modeling
