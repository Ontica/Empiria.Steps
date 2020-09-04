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
using Empiria.Postings.Media;

using Empiria.ProjectManagement;
using Empiria.Steps.Design.Integration;


namespace Empiria.Steps.Design.DataObjects {

  /// <summary>Defines a step data object.</summary>
  public class StepDataObject : BaseObject {

    #region Constructors and parsers

    private StepDataObject() {
      // Required by Empiria Framework
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
    public MediaFile MediaFile {
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


    public string AutofillFileUrl {
      get {
        return this.Configuration.Get<string>("autofillFileUrl", String.Empty);
      }
      internal set {
        this.Configuration.Set("autofillFileUrl", value);
      }
    }

    public JsonObject AutoFillFields {
      get {
        return this.FormData.Slice("autofillFields", false);
      }
      internal set {
        this.FormData.Set("autofillFields", value);
      }
    }


    public string UploadedFileUrl {
      get {
        return this.Configuration.Get<string>("uploadedFileUrl", String.Empty);
      }
      internal set {
        this.Configuration.Set("uploadedFileUrl", value);
      }
    }


    #endregion Properties


    #region Methods


    public IDictionary<string, object> GetFormFields() {
      return FormData.ToDictionary();
    }


    public void Delete() {
      this.Status = EntityStatus.Deleted;

      this.Save();
    }


    public void RemoveFile() {
      this.UploadedFileUrl = String.Empty;

      this.Save();
    }


    public void RemoveMediaFile() {
      this.AutofillFileUrl = String.Empty;

      this.Save();
    }


    protected override void OnSave() {
      StepsDataRepository.WriteStepDataObject(this);
    }


    public void SaveFormData(JsonObject json) {
      this.FormData = json;

      this.Save();
    }


    #endregion Methods

  }  // class StepDataObject

}  // namespace Empiria.Steps.Design
