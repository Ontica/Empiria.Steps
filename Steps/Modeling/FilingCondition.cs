/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : FilingCondition                                  License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Contains data about a procedure filing conditions.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Steps.Modeling {

  public class FilingCondition {

    #region Constructors and parsers

    private FilingCondition() {
      // Required by Empiria Framework.
    }

    static internal FilingCondition Parse(JsonObject data) {
      var o = new FilingCondition();

      //o.StartsWhen = data.Get<StartsWhen>("startsWhen", o.StartsWhen);
      //o.StartsWhenTrigger = data.Get<string>("startsWhenTrigger", o.StartsWhenTrigger);

      o.MaxFilingTerm = data.Get<string>("maxFilingTerm", o.MaxFilingTerm);
      o.MaxFilingTermUnit = data.Get<string>("maxFilingTermUnit", o.MaxFilingTermUnit);
      o.IssuanceLegalTerm = data.Get<string>("issuanceLegalTerm", o.IssuanceLegalTerm);
      o.IssuanceLegalTermUnit = data.Get<string>("issuanceLegalTermUnit", o.IssuanceLegalTermUnit);
      o.HowToFile = data.Get<string>("howToFile", o.HowToFile);
      o.HowToFileAddress = data.Get<string>("howToFileAddress", o.HowToFileAddress);

      o.DeferralsTerm = data.Get<string>("deferralsTerm", o.DeferralsTerm);
      o.DeferralsTermUnit = data.Get<string>("deferralsTermUnit", o.DeferralsTermUnit);
      o.DeferralsTermNotes = data.Get<string>("deferralsTermNotes", o.DeferralsTermNotes);

      o.ValidityTermWhenIssued = data.Get<string>("validityTermWhenIssued", o.ValidityTermWhenIssued);
      o.ValidityTermUnitWhenIssued = data.Get<string>("validityTermUnitWhenIssued", o.ValidityTermUnitWhenIssued);
      o.Ficta = data.Get<string>("ficta", o.Ficta);

      o.HasInnerInteraction = data.Get<string>("simultaneousDelivery", o.HasInnerInteraction);
      o.StartsWhenNotes = data.Get<string>("startsWhenNotes", o.StartsWhenNotes);
      o.MaxFilingTermNotes = data.Get<string>("maxFilingTermNotes", o.MaxFilingTermNotes);

      return o;
    }

    static public FilingCondition Empty {
      get {
        return new FilingCondition();
      }
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("StartsWhen")]
    public string StartsWhen {
      get;
      private set;
    }


    [DataField("StartsWhenNotes")]
    public string StartsWhenNotes {
      get;
      private set;
    } = String.Empty;


    [DataField("StartsWhenTrigger")]
    public string StartsWhenTrigger {
      get;
      private set;
    } = String.Empty;


    [DataField("MaxFilingTerm")]
    public string MaxFilingTerm {
      get;
      private set;
    } = String.Empty;


    [DataField("MaxFilingTermUnit")]
    public string MaxFilingTermUnit {
      get;
      private set;
    } = String.Empty;

    [DataField("MaxFilingTermNotes")]
    public string MaxFilingTermNotes {
      get;
      private set;
    } = String.Empty;


    [DataField("IssuanceLegalTerm")]
    public string IssuanceLegalTerm {
      get;
      private set;
    } = String.Empty;

    [DataField("IssuanceLegalTermUnit")]
    public string IssuanceLegalTermUnit {
      get;
      private set;
    } = String.Empty;


    [DataField("HowToFile")]
    public string HowToFile {
      get;
      private set;
    } = String.Empty;


    [DataField("HowToFileAddress")]
    public string HowToFileAddress {
      get;
      private set;
    } = String.Empty;


    [DataField("DeferralsTerm")]
    public string DeferralsTerm {
      get;
      private set;
    } = String.Empty;


    [DataField("DeferralsTermUnit")]
    public string DeferralsTermUnit {
      get;
      private set;
    } = String.Empty;


    [DataField("DeferralsTermNotes")]
    public string DeferralsTermNotes {
      get;
      private set;
    } = String.Empty;


    [DataField("ValidityTermWhenIssued")]
    public string ValidityTermWhenIssued {
      get;
      private set;
    } = String.Empty;


    [DataField("ValidityTermUnitWhenIssued")]
    public string ValidityTermUnitWhenIssued {
      get;
      private set;
    } = String.Empty;


    [DataField("Ficta")]
    public string Ficta {
      get;
      private set;
    } = String.Empty;


    [DataField("HasInnerInteraction")]
    public string HasInnerInteraction {
      get;
      private set;
    } = String.Empty;


    #endregion Properties

  }  // class FilingCondition

}  // namespace Empiria.Steps.Modeling
