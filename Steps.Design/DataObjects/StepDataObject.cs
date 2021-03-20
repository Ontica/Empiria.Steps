/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Data Objects                            *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Information Holder                      *
*  Type     : StepDataObject                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Defines a step data object.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Json;
using Empiria.StateEnums;

using Empiria.Data.DataObjects;
using Empiria.Storage;

using Empiria.ProjectManagement;
using Empiria.Steps.Design.Integration;


namespace Empiria.Steps.Design.DataObjects {

  /// <summary>Defines a step data object.</summary>
  public class StepDataObject : BaseObject {

    #region Constructors and parsers

    private StepDataObject() {
      // Required by Empiria Framework
    }


    public StepDataObject(ProjectItem step, JsonObject requirement) {
      this.Step = step;
      this.LoadRequirement(requirement);
    }


    public StepDataObject(ProjectItem step, DataStore dataStore) {
      this.Step = step;
      this.DataItem = dataStore;
    }

    static internal StepDataObject Parse(int id) {
      return BaseObject.ParseId<StepDataObject>(id);
    }


    static public StepDataObject Parse(string uid) {
      return BaseObject.ParseKey<StepDataObject>(uid);
    }


    static public FixedList<StepDataObject> GetListFor(ProjectItem step) {
      return StepsDataRepository.GetDataObjects(step);
    }


    static public FixedList<StepDataObject> GetListForAction(ProjectItem step) {
      return StepsDataRepository.GetActionDataObjects(step);
    }


    #endregion Constructors and parsers


    #region Properties


    [DataField("DataItemId")]
    public DataStore DataItem {
      get;
      private set;
    }


    [DataField("StepId")]
    public ProjectItem Step {
      get;
      private set;
    }

    [DataField("ActivityId")]
    public ProjectItem Activity {
      get;
      private set;
    }


    public string Action {
      get {
        return this.GetActionName();
      }
    }


    [DataField("MediaId")]
    public FormerMediaFile MediaFile {
      get;
      private set;
    }


    public string MediaFormat {
      get {
        return this.GetMediaFormat();
      }
    }


    [DataField("FormId")]
    public int FormId {
      get;
      private set;
    } = -1;


    [DataField("StepDataObjectFormData")]
    public JsonObject FormData {
      get;
      private set;
    } = new JsonObject();


    [DataField("StepDataObjectConfig")]
    public JsonObject Configuration {
      get;
      private set;
    } = new JsonObject();


    [DataField("StepDataObjectExtData")]
    public JsonObject ExtensionData {
      get;
      private set;
    } = new JsonObject();


    [DataField("StepDataObjectStatus", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get;
      private set;
    }

    public string Name {
      get {
        return this.ExtensionData.Get("requirement/name", this.DataItem.Name);
      }
    }

    public string Description {
      get {
        return this.ExtensionData.Get("requirement/description", this.DataItem.Description);
      }
    }

    public string Optional {
      get {
        return this.ExtensionData.Get("requirement/optional", "Mandatory");
      }
    }

    public string LegalBasis {
      get {
        return this.ExtensionData.Get("requirement/legalBasis", String.Empty);
      }
    }

    #endregion Properties


    #region Methods

    public IDictionary<string, object> GetFormFields() {
      return FormData.ToDictionary();
    }


    private void LoadRequirement(JsonObject requirementData) {
      if (requirementData.Contains("dataObject/uid")) {
        this.DataItem = DataStore.Parse(requirementData.Get<string>("dataObject/uid"));
      } else {
        this.DataItem = DataStore.Empty;
      }

      requirementData.Remove("uid");
      requirementData.Remove("dataObject");

      this.ExtensionData.Set("requirement", requirementData);
    }


    public void Update(JsonObject requirement) {
      this.LoadRequirement(requirement);
    }


    public void Delete() {
      this.Status = EntityStatus.Deleted;

      this.Save();
    }


    protected override void OnSave() {
      StepsDataRepository.WriteStepDataObject(this);
    }


    public void ToggleStatus() {
      if (this.Status == EntityStatus.Active) {
        this.Status = EntityStatus.Pending;
        this.Save();
      } else if (this.Status == EntityStatus.Pending) {
        this.Status = EntityStatus.Active;
        this.Save();
      }
    }

    #endregion Methods

  }  // class StepDataObject

}  // namespace Empiria.Steps.Design
