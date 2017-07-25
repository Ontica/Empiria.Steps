/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Clause                                           License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Characterizes a contract clause (or document item).                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Json;

namespace Empiria.Steps.Legal {

  public class Clause : BaseObject {

    #region Fields

    private Lazy<List<RelatedProcedure>> procedures = null;

    #endregion Fields

    #region Constructors and parsers

    protected Clause() {
      // Required by Empiria Framework.
    }


    internal Clause(Contract legalDocument, JsonObject data) {
      Assertion.AssertObject(legalDocument, "legalDocument");
      Assertion.AssertObject(data, "data");

      this.ContractId = legalDocument.Id;
      this.Load(data);
    }


    static public Clause Parse(string uid) {
      return BaseObject.ParseKey<Clause>(uid);
    }


    static internal List<Clause> GetList(Contract legalDocument) {
      string filter = $"LegalDocumentId = {legalDocument.Id}";

      return BaseObject.GetList<Clause>(filter, "ItemNumber, ItemPosition");
    }


    protected override void OnInitialize() {
      procedures = new Lazy<List<RelatedProcedure>>(() => RelatedProcedure.GetList(this));
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("UID")]
    public string UID {
      get;
      private set;
    }


    [DataField("LegalDocumentId")]
    internal int ContractId {
      get;
      private set;
    }


    [DataField("ItemTypeId")]
    public int ItemTypeId {
      get;
      private set;
    } = -1;


    [DataField("ItemTitle")]
    public string Title {
      get;
      private set;
    }


    [DataField("ItemNumber")]
    public string Number {
      get;
      private set;
    }


    [DataField("ItemText")]
    public string Text {
      get;
      private set;
    }


    [DataField("ItemPosition")]
    public int Position {
      get;
      private set;
    }


    [DataField("DocumentPageNo")]
    public int DocumentPageNo {
      get;
      private set;
    }


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }


    [DataField("Status")]
    public string Status {
      get;
      private set;
    }


    public FixedList<RelatedProcedure> RelatedProcedures {
      get {
        return procedures.Value.ToFixedList();
      }
    }

    #endregion Public properties

    #region Public methods

    public RelatedProcedure AddRelatedProcedure(JsonObject data) {
      var relatedProcedure = new RelatedProcedure(this, data);

      relatedProcedure.Save();

      procedures.Value.Add(relatedProcedure);

      return relatedProcedure;
    }


    public RelatedProcedure GetRelatedProcedure(string relatedProcedureUID) {
      RelatedProcedure item = procedures.Value.Find((x) => x.UID == relatedProcedureUID);

      Assertion.AssertObject(item, $"A related procedure with uid = '{relatedProcedureUID}' " +
                                   $"was not found in contract clause with uid = '{this.UID}'");

      return item;
    }


    protected override void OnBeforeSave() {
      if (this.IsNew) {
        this.UID = EmpiriaString.BuildRandomString(32);
      }
    }


    protected override void OnSave() {
      ContractsData.WriteClause(this);
    }


    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Load(data);

      this.Save();
    }

    #endregion Public methods

    #region Private methods

    private void Load(JsonObject data) {
      this.Number = data.Get<string>("clauseNo", this.Number);
      this.Title = data.Get<string>("title", this.Title);
      this.Text = data.Get<string>("text", this.Text);
      this.DocumentPageNo = data.Get<int>("sourcePageNo", this.DocumentPageNo);
      this.Notes = data.Get<string>("notes", this.Notes);
      this.Status = data.Get<string>("status", this.Status);
    }

    #endregion Private methods

  }  // class Clause

}  // namespace Empiria.Steps.Legal
