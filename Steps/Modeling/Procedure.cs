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
using Empiria.Json;

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

    static public Procedure Parse(string uid) {
      return BaseObject.ParseKey<Procedure>(uid);
    }

    static public FixedList<Procedure> GetList(string filter = "") {
      return ProcedureData.GetProcedureList(filter);
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

    [DataField("Obligation")]
    public string Obligation {
      get;
      private set;
    }

    [DataField("URL")]
    public string URL {
      get;
      private set;
    }

    [DataField("Entity")]
    public string Entity {
      get;
      private set;
    }

    [DataField("Authority")]
    public string Authority {
      get;
      private set;
    }

    [DataField("AuthorityContact")]
    public string AuthorityContact {
      get;
      private set;
    }

    [DataField("IsRegulated")]
    public bool IsRegulated {
      get;
      private set;
    }

    [DataField("Requirements")]
    public string Requirements {
      get;
      private set;
    }

    [DataField("Stage")]
    public string Stage {
      get;
      private set;
    }

    [DataField("StageInnerNo")]
    public int StageInnerNo {
      get;
      private set;
    }

    [DataField("Category")]
    public string Category {
      get;
      private set;
    }

    [DataField("Theme")]
    public string Theme {
      get;
      private set;
    }

    [DataField("ContractClausesAndAnnexes")]
    public string ContractClausesAndAnnexes {
      get;
      private set;
    }

    [DataField("LegalBasis")]
    public string LegalBasis {
      get;
      private set;
    }

    [DataField("StartsWhen")]
    public string StartsWhen {
      get;
      private set;
    }

    [DataField("MaxComplianceTerm")]
    public string MaxComplianceTerm {
      get;
      private set;
    }

    [DataField("EmissionLegalTerm")]
    public string EmissionLegalTerm {
      get;
      private set;
    }

    [DataField("Deferrals")]
    public string Deferrals {
      get;
      private set;
    }

    [DataField("DeferralsTerm")]
    public string DeferralsTerm {
      get;
      private set;
    }

    [DataField("Cost")]
    public string Cost {
      get;
      private set;
    }

    [DataField("CostLegalBasis")]
    public string CostLegalBasis {
      get;
      private set;
    }

    [DataField("ValidityTermWhenEmitted")]
    public string ValidityTermWhenEmitted {
      get;
      private set;
    }

    [DataField("SimultaneousDelivery")]
    public string SimultaneousDelivery {
      get;
      private set;
    }

    [DataField("Notes")]
    public string Notes {
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
      this.Obligation = data.Get<string>("obligation", this.Obligation);
      this.URL = data.Get<string>("url", this.URL);
      this.Entity = data.Get<string>("entity", this.Entity);
      this.Authority = data.Get<string>("authority", this.Authority);
      this.AuthorityContact = data.Get<string>("authorityContact", this.AuthorityContact);
      this.IsRegulated = data.Get<bool>("isRegulated", this.IsRegulated);
      this.Requirements = data.Get<string>("requirements", this.Requirements);
      this.Stage = data.Get<string>("stage", this.Stage);
      this.StageInnerNo = data.Get<int>("stageInnerNo", this.StageInnerNo);
      this.Category = data.Get<string>("category", this.Category);
      this.Theme = data.Get<string>("theme", this.Theme);
      this.ContractClausesAndAnnexes = data.Get<string>("contractClausesAndAnnexes", this.ContractClausesAndAnnexes);
      this.LegalBasis = data.Get<string>("legalBasis", this.LegalBasis);
      this.StartsWhen = data.Get<string>("startsWhen", this.StartsWhen);
      this.MaxComplianceTerm = data.Get<string>("maxComplianceTerm", this.MaxComplianceTerm);
      this.EmissionLegalTerm = data.Get<string>("emissionLegalTerm", this.EmissionLegalTerm);
      this.Deferrals = data.Get<string>("deferrals", this.Deferrals);
      this.DeferralsTerm = data.Get<string>("deferralsTerm", this.DeferralsTerm);
      this.Cost = data.Get<string>("cost", this.Cost);
      this.CostLegalBasis = data.Get<string>("costLegalBasis", this.CostLegalBasis);
      this.ValidityTermWhenEmitted = data.Get<string>("validityTermWhenEmitted", this.ValidityTermWhenEmitted);
      this.SimultaneousDelivery = data.Get<string>("simultaneousDelivery", this.SimultaneousDelivery);
      this.Notes = data.Get<string>("notes", this.Notes);
    }

    #endregion Private methods

  }  // class Procedure

}  // namespace Empiria.Steps.Modeling
