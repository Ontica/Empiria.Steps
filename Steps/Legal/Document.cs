/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Requirement                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Handles information about a document.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Json;
using Empiria.Contacts;

namespace Empiria.Steps.Legal {

  /// <summary>Handles information about a document.</summary>
  public class Document : BaseObject {

    #region Constructors and parsers

    protected Document() {
      // Required by Empiria Framework.
    }


    static internal Document Parse(int id) {
      return BaseObject.ParseId<Document>(id);
    }


    static public Document Parse(string uid) {
      return BaseObject.ParseKey<Document>(uid);
    }


    static public FixedList<Document> GetList(string type = "",
                                              string keywords = "") {
      string filter = String.Empty;
      string orderBy = "DocumentType, DocumentName";

      if (!String.IsNullOrWhiteSpace(type)) {
        filter = SearchExpression.ParseEquals("DocumentType", type);
      }
      if (!String.IsNullOrWhiteSpace(keywords)) {
        filter += filter.Length != 0 ? " AND " : String.Empty;
        filter += SearchExpression.ParseAndLike("Keywords", keywords);
      }

      return BaseObject.GetList<Document>(filter, orderBy)
                       .FindAll((x) => !x.IsSpecialCase)
                       .ToFixedList();
    }

    static public void UpdateAll() {
      var documents = BaseObject.GetList<Document>();

      foreach (var document in documents) {
        document.Save();
     }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("UID")]
    public string UID {
      get;
      private set;
    } = String.Empty;


    [DataField("DocumentType")]
    public string DocumentType {
      get;
      private set;
    } = String.Empty;


    [DataField("DocumentName")]
    public string Name {
      get;
      private set;
    } = String.Empty;


    [DataField("Code")]
    public string Code {
      get;
      private set;
    } = String.Empty;


    [DataField("Description")]
    public string Description {
      get;
      private set;
    } = String.Empty;


    [DataField("Observations")]
    public string Observations {
      get;
      private set;
    } = String.Empty;


    [DataField("DocumentURL")]
    public string Url {
      get;
      private set;
    }


    [DataField("SampleURL")]
    public string SampleURL {
      get;
      private set;
    }


    [DataField("InstructionsURL")]
    public string InstructionsUrl {
      get;
      private set;
    } = String.Empty;


    [DataField("OwnerId")]
    public Contact Owner {
      get;
      private set;
    }


    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Code, this.Name, this.Description, this.Observations);
      }
    }

    #endregion Public properties

    #region Public methods

    protected override void OnBeforeSave() {
      if (this.UID.Length == 0) {
        this.UID = EmpiriaString.BuildRandomString(6, 24);
      }
    }


    protected override void OnSave() {
      DocumentsData.WriteDocument(this);
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


    private void Load(JsonObject data) {

    }

    #endregion Private methods

  }  // class Document

}  // namespace Empiria.Steps.Legal
