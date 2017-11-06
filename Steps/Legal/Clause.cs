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

using Empiria.Data;
using Empiria.Json;

namespace Empiria.Steps.Legal {

  public class Clause : BaseObject {

    #region Fields

    private Lazy<List<DocumentRule>> rules = null;
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

      this.AssertIsValid(data);

      this.Load(data);
    }


    static internal Clause Parse(int id) {
      return BaseObject.ParseId<Clause>(id);
    }


    static public Clause Parse(string uid) {
      return BaseObject.ParseKey<Clause>(uid);
    }


    static internal List<Clause> GetList(Contract legalDocument) {
      string filter = $"DocumentId = {legalDocument.Id}";

      return BaseObject.GetList<Clause>(filter, "ItemOrdering, ItemSection, ItemNumber");
    }

    static internal List<Clause> GetList(Contract legalDocument, string keywords) {
      string filter = $"DocumentId = {legalDocument.Id}";

      keywords = keywords.Replace("'", "");

      if (keywords.Length != 0) {
        var keywordsFilter = SearchExpression.ParseAndLike("Keywords", keywords);

        filter = GeneralDataOperations.BuildSqlAndFilter(filter, keywordsFilter);
      }

      return BaseObject.GetList<Clause>(filter, "ItemOrdering, ItemSection, ItemNumber");
    }

    static public void UpdateAll() {
      var clauses = BaseObject.GetList<Clause>();

      foreach (var clause in clauses) {
        clause.Save();
      }
    }


    protected override void OnInitialize() {
      rules = new Lazy<List<DocumentRule>>(() => DocumentRule.GetList(this));
      procedures = new Lazy<List<RelatedProcedure>>(() => RelatedProcedure.GetList(this));
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("UID")]
    public string UID {
      get;
      private set;
    }


    [DataField("DocumentId")]
    internal int ContractId {
      get;
      private set;
    }


    public Contract Contract {
      get {
        return Contract.Parse(this.ContractId);
      }
    }


    [DataField("ItemSection")]
    public string Section {
      get;
      private set;
    }


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

    private string _asHypertext = null;
    public string AsHypertext {
      get {
        if (_asHypertext == null) {
          _asHypertext = Presentation.Hypertext.ToTermDefinitionHypertext(this.Text, this.Contract);
        }
        return _asHypertext;
      }
    }

    public string Ordering {
      get {
        var temp = this.Section.Trim();

        if (temp.Equals("Cláusulas")) {
          temp = String.Empty;

        } else if ((temp.LastIndexOf(' ') == temp.Length - 2) &&
                    EmpiriaString.IsInteger(temp.Substring(temp.Length - 1))) {
          temp = temp.Replace(" " + temp.Substring(temp.Length - 1), " 0" + temp.Substring(temp.Length - 1)) + ".";

        } else {
          temp = temp + ".";
        }

        var number = this.Number.Trim().Replace(".", ".0");
        if (number.Length == 1) {
          number = "0" + number;
        } else if (number.Substring(1, 1) == ".") {
          number = "0" + number;
        }
        return temp + number;
      }
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


    internal string Keywords {
      get {
        string temp = String.Empty;

        foreach (var i in this.RelatedProcedures) {
          temp += i.Procedure.Code + " " + EmpiriaString.BuildKeywords(i.Procedure.Name, i.Procedure.EntityName);
          temp += " ";
        }
        return this.Number + " " + EmpiriaString.BuildKeywords(this.Title, temp, this.Text);
      }
    }


    public FixedList<RelatedProcedure> RelatedProcedures {
      get {
        return procedures.Value.ToFixedList();
      }
    }

    public FixedList<DocumentRule> Rules {
      get {
        return rules.Value.ToFixedList();
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

    internal RelatedProcedure AddRelatedProcedure(Modeling.Procedure procedure) {
      var relatedProcedure = new RelatedProcedure(this, procedure);

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
      if (this.UID.Length == 0) {
        this.UID = EmpiriaString.BuildRandomString(6, 24);
      }
    }


    protected override void OnSave() {
      ContractsData.WriteClause(this);
    }


    public RelatedProcedure TryGetRelatedProcedure(Predicate<RelatedProcedure> predicate) {
      return procedures.Value.Find(predicate);
    }


    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.AssertIsValid(data);

      this.Load(data);

      this.Save();
    }

    #endregion Public methods

    #region Private methods

    private void AssertIsValid(JsonObject data) {
      Assertion.AssertObject(data, "data");

      Validate.HasValue(data, "clauseNo", "Requiero conocer el número de cláusula o anexo.");
      Validate.HasValue(data, "title", "Necesito el nombre que describe a la cláusula o anexo.");

      var clauseNo = data.GetClean("clauseNo");

      if (this.IsNew) {
        var clause = this.Contract.TryGetClause((x) => x.Number.Equals(clauseNo));
        Validate.AlreadyExists(clause, $"Este contrato ya contiene una cláusula con el número '{clauseNo}'.");
      } else {
        var clause = this.Contract.TryGetClause((x) => x.Number.Equals(clauseNo) &&
                                                       x.Id != this.Id);
        Validate.AlreadyExists(clause, $"En este contrato ya existe otra cláusula con el número '{clauseNo}'.");
      }
    }


    private void Load(JsonObject data) {
      this.Section = data.GetClean("section");
      this.Number = data.GetClean("clauseNo");
      this.Title = data.GetClean("title");
      this.Text = data.Get<string>("text", this.Text);
      this.DocumentPageNo = data.Get<int>("sourcePageNo", this.DocumentPageNo);
      this.Notes = data.Get<string>("notes", this.Notes);
      this.Status = data.Get<string>("status", this.Status);
    }

    #endregion Private methods

  }  // class Clause

}  // namespace Empiria.Steps.Legal
