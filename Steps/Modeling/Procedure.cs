/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Procedure                                        License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Provides services that gets process definition models.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Linq;

using Empiria.DataTypes;
using Empiria.Json;
using Empiria.Steps.Legal;
using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Steps.Modeling {

  public class Procedure : BaseObject {

    #region Constructors and parsers

    private Procedure() {
      // Required by Empiria Framework.
    }

    public Procedure(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Load(data);
    }

    static public Procedure Parse(int id) {
      return BaseObject.ParseId<Procedure>(id);
    }

    static public Procedure Parse(string uid) {
      return BaseObject.ParseKey<Procedure>(uid);
    }

    static public Procedure Empty {
      get {
        return BaseObject.ParseEmpty<Procedure>();
      }
    }

    static public FixedList<Procedure> GetList(string filter = "", string keywords = "") {
      return ProcedureData.GetProcedureList(filter, keywords);
    }

    static public FixedList<KeyValue> StartsWhenList {
      get {
        var list = KeyValueList.Parse("Steps.ProcedureStartsWhen.List");

        return list.GetItems();
      }
    }

    static public FixedList<string> ThemesList {
      get {
        var list = GeneralList.Parse("Steps.ProcedureThemes.List");

        return list.GetItems<string>();
      }
    }

    static public FixedList<KeyValue> TermTimeUnitsList {
      get {
        var list = KeyValueList.Parse("Steps.TermTimeUnits.List");

        return list.GetItems();
      }
    }

    static public void UpdateAll() {
      var procedures = BaseObject.GetList<Procedure>();

      foreach (var procedure in procedures) {
        procedure.Save();

        UpdateAllRelatedClauses(procedure, Legal.Contract.Parse(565), procedure.LegalInfo.Ronda13Consorcio);
        UpdateAllRelatedClauses(procedure, Legal.Contract.Parse(566), procedure.LegalInfo.Ronda13Individual);
        UpdateAllRelatedClauses(procedure, Legal.Contract.Parse(567), procedure.LegalInfo.Ronda14Consorcio);
        UpdateAllRelatedClauses(procedure, Legal.Contract.Parse(568), procedure.LegalInfo.Ronda14Individual);
        UpdateAllRelatedClauses(procedure, Legal.Contract.Parse(569), procedure.LegalInfo.Ronda21Consorcio);
        UpdateAllRelatedClauses(procedure, Legal.Contract.Parse(570), procedure.LegalInfo.Ronda21Individual);
      }
    }

    internal void SetBpmnDiagram(BpmnDiagram diagram) {
      this.BpmnDiagram = diagram;
      Save();
    }

    private static void UpdateAllRelatedClauses(Procedure procedure,
                                                Contract contract, string clausesAsText) {
      if (clausesAsText.Contains("Anexo")) {
        return;
      }

      clausesAsText = clausesAsText.Replace(',', ' ')
                                   .Replace(';', ' ');

      var clauseTextParts = clausesAsText.Split(' ')
                                         .Where((x) => EmpiriaString.IsQuantity(x));

      if (clauseTextParts == null) {
        return;
      }
      var clauses = contract.Clauses.FindAll((x) => clauseTextParts.Contains(x.Number) && x.Section == "Cláusulas");

      if (clauses == null || clauses.Count == 0) {
        return;
      }
      foreach (var clause in clauses) {
        if (clause.RelatedProcedures.Contains((x) => x.Procedure.Equals(procedure))) {
          continue;
        }
        clause.AddRelatedProcedure(procedure);
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("UID")]
    public string UID {
      get;
      private set;
    }


    [DataField("ProcedureName")]
    public string Name {
      get;
      private set;
    }

    [DataField("ShortName")]
    public string ShortName {
      get;
      private set;
    }

    [DataField("Code")]
    public string Code {
      get;
      private set;
    }

    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }

    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Code, this.Name, this.EntityName,
                                           this.LegalInfo.LegalBasis, this.LegalInfo.Obligation, this.ProjectType);
      }
    }

    [DataField("OfficialURL")]
    public string OfficialURL {
      get;
      private set;
    }


    [DataField("RegulationURL")]
    public string RegulationURL {
      get;
      private set;
    }


    [DataField("Theme")]
    public string Theme {
      get;
      private set;
    }

    [DataField("ExecutionMode")]
    public string ExecutionMode {
      get;
      private set;
    }

    [DataField("ProjectType")]
    public string ProjectType {
      get;
      private set;
    }

    [DataField("Entity")]
    public string EntityName {
      get;
      private set;
    }

    [DataField("Authority")]
    public string AuthorityName {
      get;
      private set;
    }

    [DataField("AuthorityTitle")]
    public string AuthorityTitle {
      get;
      private set;
    }

    [DataField("AuthorityContact")]
    public string AuthorityContact {
      get;
      private set;
    }

    [DataObject]
    public Authority Authority {
      get;
      private set;
    } = Authority.Empty;


    [DataObject]
    public LegalInfo LegalInfo {
      get;
      private set;
    } = LegalInfo.Empty;


    [DataObject]
    public FilingCondition FilingCondition {
      get;
      private set;
    } = FilingCondition.Empty;


    [DataObject]
    public FilingDocuments FilingDocuments {
      get;
      private set;
    } = FilingDocuments.Empty;


    [DataObject]
    public FilingFee FilingFee {
      get;
      private set;
    } = FilingFee.Empty;


    [DataField("BpmnDiagramId")]
    public BpmnDiagram BpmnDiagram {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    protected override void OnBeforeSave() {
      if (this.UID.Length == 0) {
        this.UID = EmpiriaString.BuildRandomString(6, 24);
      }
    }

    protected override void OnSave() {
      ProcedureData.WriteProcedure(this);
    }

    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Load(data);
    }

    #endregion Public methods

    #region Private methods

    private void Load(JsonObject data) {
      this.Name = data.Get<string>("name", this.Name);
      this.ShortName = data.Get<string>("shortName", this.Name);
      this.Code = data.Get<string>("code", this.Code);
      this.Notes = data.Get<string>("notes", this.Notes);
      this.OfficialURL = data.Get<string>("officialUrl", this.OfficialURL);
      this.RegulationURL = data.Get<string>("regulationUrl", this.OfficialURL);

      this.Authority = Authority.Parse(data.Slice("authority"));
      this.LegalInfo = LegalInfo.Parse(data.Slice("legalInfo"));
      this.FilingCondition = FilingCondition.Parse(data.Slice("filingCondition"));
      this.FilingDocuments = FilingDocuments.Parse(data.Slice("filingDocuments"));
      this.FilingFee = FilingFee.Parse(data.Slice("filingFee"));
    }

    #endregion Private methods

  }  // class Procedure

}  // namespace Empiria.Steps.Modeling
