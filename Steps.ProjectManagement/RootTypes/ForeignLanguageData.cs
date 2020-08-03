/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Domain Layer                          *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Information Holder                    *
*  Type     : ForeignLanguageData                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Contains foreign language data fields for project items.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.ProjectManagement {

  /// <summary>Contains foreign language data fields for project items.</summary>
  public class ForeignLanguageData {

    #region Constructors and parsers

    private ForeignLanguageData() {

    }


    static internal ForeignLanguageData Parse(string data) {
      var json = JsonObject.Parse(data);

      return Parse(json);
    }


    static internal ForeignLanguageData Parse(JsonObject data) {
      var o = new ForeignLanguageData();

      o.Name = data.Get<string>("name", String.Empty);
      o.Notes = data.Get<string>("notes", String.Empty);
      o.ContractClause = data.Get<string>("contractClause", String.Empty);
      o.LegalBasis = data.Get<string>("legalBasis", String.Empty);

      return o;
    }


    static public ForeignLanguageData Empty {
      get {
        return new ForeignLanguageData();
      }
    }


    #endregion Constructors and parsers

    #region Properties

    [DataField("Name")]
    public string Name {
      get;
      private set;
    } = String.Empty;


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    } = String.Empty;


    [DataField("ContractClause")]
    public string ContractClause {
      get;
      private set;
    } = String.Empty;


    [DataField("LegalBasis")]
    public string LegalBasis {
      get;
      private set;
    } = String.Empty;


    #endregion Properties

    #region Methods


    public JsonObject ToJson() {
      var json = new JsonObject();

      json.AddIfValue("name", this.Name);
      json.AddIfValue("notes", this.Notes);
      json.AddIfValue("contractClause", this.ContractClause);
      json.AddIfValue("legalBasis", this.LegalBasis);

      return json;
    }

    public override string ToString() {
      return this.ToJson().ToString();
    }


    #endregion Methods

  } // class ForeignLanguageData

} // namespace Empiria.ProjectManagement
