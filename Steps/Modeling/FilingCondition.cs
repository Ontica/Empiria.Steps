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

      o.StartsWhen = data.Get<string>("startsWhen", o.StartsWhen);
      o.StartsWhenTrigger = data.Get<string>("startsWhenTrigger", o.StartsWhenTrigger);

      o.MaxFilingTerm = data.Get<string>("maxFilingTerm", o.MaxFilingTerm);
      o.IssuanceLegalTerm = data.Get<string>("issuanceLegalTerm", o.IssuanceLegalTerm);
      o.HowToFile = data.Get<string>("howToFile", o.HowToFile);

      o.AllowsDeferrals = data.Get<string>("allowsDeferrals", o.AllowsDeferrals);
      o.DeferralsTermNotes = data.Get<string>("deferralsTermNotes", o.DeferralsTermNotes);
      o.DeferralsConditionNotes = data.Get<string>("deferralsConditionNotes", o.DeferralsConditionNotes);
      o.ValidityTermWhenIssued = data.Get<string>("validityTermWhenIssued", o.ValidityTermWhenIssued);

      o.SimultaneousDelivery = data.Get<string>("simultaneousDelivery", o.SimultaneousDelivery);
      o.StartsWhenNotes = data.Get<string>("startsWhenNotes", o.StartsWhenNotes);
      o.MaxFilingTermNotes = data.Get<string>("maxFilingTermNotes", o.MaxFilingTermNotes);
      o.IssuanceLegalTermNotes = data.Get<string>("issuanceLegalTermNotes", o.IssuanceLegalTermNotes);

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


    [DataField("IssuanceLegalTerm")]
    public string IssuanceLegalTerm {
      get;
      private set;
    } = String.Empty;

    [DataField("HowToFile")]
    public string HowToFile {
      get;
      private set;
    } = String.Empty;


    [DataField("AllowsDeferrals")]
    public string AllowsDeferrals {
      get;
      private set;
    } = String.Empty;


    [DataField("DeferralsTermText")]
    public string DeferralsTermNotes {
      get;
      private set;
    } = String.Empty;


    [DataField("DeferralsConditionText")]
    public string DeferralsConditionNotes {
      get;
      private set;
    } = String.Empty;


    [DataField("ValidityTermWhenIssued")]
    public string ValidityTermWhenIssued {
      get;
      private set;
    } = String.Empty;


    [DataField("SimultaneousDelivery")]
    public string SimultaneousDelivery {
      get;
      private set;
    } = String.Empty;


    [DataField("StartsWhenText")]
    public string StartsWhenNotes {
      get;
      private set;
    } = String.Empty;


    [DataField("MaxFilingTermText")]
    public string MaxFilingTermNotes {
      get;
      private set;
    } = String.Empty;


    [DataField("IssuanceLegalTermText")]
    public string IssuanceLegalTermNotes {
      get;
      private set;
    } = String.Empty;


    #endregion Properties

  }  // class FilingCondition

}  // namespace Empiria.Steps.Modeling
