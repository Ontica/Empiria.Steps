/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : DocumentRule                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Characterizes an obligation or right enforced by law or by a legal agreement.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Data;
using Empiria.Json;

namespace Empiria.Steps.Legal {

  /// <summary>Characterizes an obligation or right enforced by law or by a legal agreement.</summary>
  public class DocumentRule : BaseObject {

    #region Constructors and parsers

    protected DocumentRule() {
      // Required by Empiria Framework.
    }


    internal DocumentRule(Contract legalDocument, JsonObject data) {
      Assertion.AssertObject(legalDocument, "legalDocument");
      Assertion.AssertObject(data, "data");

      this.DocumentId = legalDocument.Id;

      this.AssertIsValid(data);

      this.Load(data);
    }


    static internal DocumentRule Parse(int id) {
      return BaseObject.ParseId<DocumentRule>(id);
    }


    static public DocumentRule Parse(string uid) {
      return BaseObject.ParseKey<DocumentRule>(uid);
    }


    static internal List<DocumentRule> GetList(Clause clause) {
      string filter = $"DocumentItemId = {clause.Id}";

      return BaseObject.GetList<DocumentRule>(filter, "ItemPosition, RuleName");
    }

    static internal List<DocumentRule> GetList(Contract legalDocument, string keywords) {
      string filter = $"DocumentId = {legalDocument.Id}";

      keywords = keywords.Replace("'", "");

      if (keywords.Length != 0) {
        var keywordsFilter = SearchExpression.ParseAndLike("Keywords", keywords);

        filter = GeneralDataOperations.BuildSqlAndFilter(filter, keywordsFilter);
      }

      return BaseObject.GetList<DocumentRule>(filter, "RuleName");
    }

    static public void UpdateAll() {
      var rules = BaseObject.GetList<DocumentRule>();

      foreach (var rule in rules) {
        var contract = Contract.Parse(rule.DocumentId);

        var clauses = contract.GetClausesFromText(rule.DocumentItems);

        if (clauses.Count == 0) {
          rule.Save();
        }
        for (int i = 0; i < clauses.Count; i++) {
          var clause = clauses[i];
          if (i == 0) {
            rule.DocumentItemId = clause.Id;
            rule.Save();
          } else {
            var newRule = rule.Clone();

            newRule.DocumentItemId = clause.Id;
            newRule.ReferenceRuleId = rule.Id;
            newRule.Save();
          }
        }
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("UID")]
    public string UID {
      get;
      private set;
    }


    [DataField("DocumentId")]
    internal int DocumentId {
      get;
      private set;
    }


    public Contract Document {
      get {
        return Contract.Parse(this.DocumentId);
      }
    }


    [DataField("DocumentItemId")]
    public int DocumentItemId {
      get;
      private set;
    }


    [DataField("ReferenceRuleId")]
    public int ReferenceRuleId {
      get;
      private set;
    } = -1;


    [DataField("RuleType", Default = RuleType.Undefined)]
    public RuleType RuleType {
      get;
      private set;
    }


    [DataField("RuleName")]
    public string Name {
      get;
      private set;
    }


    [DataField("Description")]
    public string Description {
      get;
      private set;
    }


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }


    [DataField("ItemPosition")]
    public int Position {
      get;
      private set;
    }


    [DataField("AppliesTo")]
    public string AppliesTo {
      get;
      private set;
    }


    [DataField("Verb")]
    public string Verb {
      get;
      private set;
    }


    [DataField("Action")]
    public string Action {
      get;
      private set;
    }


    [DataField("WhenPredicate")]
    public string WhenPredicate {
      get;
      private set;
    }


    [DataField("ActionTimeCondition")]
    public string ActionTimeCondition {
      get;
      private set;
    }


    [DataField("Tags")]
    public string Tags {
      get;
      private set;
    }


    public JsonObject ExtensionData {
      get;
      internal set;
    } = new JsonObject();


    [DataField("WorkflowObjectId")]
    public int WorkflowObjectId {
      get;
      private set;
    }


    [DataField("DocumentItems")]
    internal string DocumentItems {
      get;
      private set;
    }


    private string _asHypertext = null;
    public string AsHypertext {
      get {
        if (_asHypertext == null) {
          _asHypertext = Presentation.Hypertext.ToTermDefinitionHypertext(this.Name, this.Document);
        }
        return _asHypertext;
      }
    }


    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Name, this.Tags, this.Description);
      }
    }

    #endregion Public properties

    #region Public methods
    protected override void OnSave() {
      if (this.UID.Length == 0) {
        this.UID = EmpiriaString.BuildRandomString(6, 24);
      }
      ContractsData.WriteDocumentRule(this);
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

    }


    private DocumentRule Clone() {
      var clone = new DocumentRule();

      clone.Name = this.Name;
      clone.RuleType = this.RuleType;
      clone.DocumentId = this.DocumentId;
      clone.Description = this.Description;
      clone.DocumentItems = this.DocumentItems;

      return clone;
    }


    private void Load(JsonObject data) {

    }

    #endregion Private methods

  }  // class DocumentRule

}  // namespace Empiria.Steps.Legal
