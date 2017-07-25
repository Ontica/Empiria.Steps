﻿/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : LegalDocument                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Characterizes a legal document, such as a legislation, regulation or a contract.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Json;

using Empiria.Steps.Modeling;

namespace Empiria.Steps.Legal {

  public class RelatedProcedure : BaseObject {

    #region Constructors and parsers

    protected RelatedProcedure() {
      // Required by Empiria Framework.
    }


    internal RelatedProcedure(Clause legalDocumentItem, JsonObject data) {
      Assertion.AssertObject(legalDocumentItem, "legalDocumentItem");
      Assertion.AssertObject(data, "data");

      this.LegalDocumentItemId = legalDocumentItem.Id;
      this.Load(data);
    }


    static public RelatedProcedure Parse(string uid) {
      return BaseObject.ParseKey<RelatedProcedure>(uid);
    }


    static internal List<RelatedProcedure> GetList(Clause documentItemId) {
      string filter = $"LegalDocumentItemId = {documentItemId.Id}";

      return BaseObject.GetList<RelatedProcedure>(filter, "ItemPosition, RelatedProcedureId");
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("UID")]
    public string UID {
      get;
      private set;
    }


    [DataField("LegalDocumentItemId")]
    internal int LegalDocumentItemId {
      get;
      private set;
    }


    [DataField("ProcedureId")]
    private int ProcedureId {
      get;
      set;
    }


    public Procedure Procedure {
      get {
        return Procedure.Parse(this.ProcedureId);
      }
    }


    [DataField("MaxFilingTerm")]
    public int MaxFilingTerm {
      get;
      private set;
    } = -1;


    [DataField("MaxFilingTermType")]
    public string MaxFilingTermType {
      get;
      private set;
    }


    [DataField("StartsWhen")]
    public string StartsWhen {
      get;
      private set;
    }


    [DataField("StartsWhenTrigger")]
    public string StartsWhenTrigger {
      get;
      private set;
    }


    public int ItemPosition {
      get;
      private set;
    }


    public JsonObject ExtensionData {
      get;
      internal set;
    } = new JsonObject();


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }


    [DataField("Status", Default = GeneralObjectStatus.Active)]
    public GeneralObjectStatus Status {
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
      ContractsData.WriteRelatedProcedure(this);
    }


    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Load(data);

      this.Save();
    }

    #endregion Public methods

    #region Private methods

    private void Load(JsonObject data) {
      this.ProcedureId = Procedure.Parse(data.Get<string>("procedure/uid")).Id;
      this.MaxFilingTerm = data.Get<int>("maxFilingTerm", this.MaxFilingTerm);
      this.MaxFilingTermType = data.Get<string>("maxFilingTermType", this.MaxFilingTermType);
      this.StartsWhen = data.Get<string>("startsWhen", this.StartsWhen);
      this.StartsWhenTrigger = data.Get<string>("startsWhenTrigger", this.StartsWhenTrigger);
      this.Notes = data.Get<string>("notes", this.Notes);
      this.Status = data.Get<GeneralObjectStatus>("status", this.Status);
    }

    #endregion Private methods

  }  // class RelatedProcedure

}  // namespace Empiria.Steps.Modeling
