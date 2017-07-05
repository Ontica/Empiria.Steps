/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : FilingFee                                        License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Contains data about a procedure filing fee conditions.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Steps.Modeling {

  public class FilingFee {

    #region Constructors and parsers

    private FilingFee() {
      // Required by Empiria Framework.
    }

    static internal FilingFee Parse(JsonObject data) {
      var o = new FilingFee();

      o.FilingFeeType = data.Get<string>("filingFeeType", o.FilingFeeType);
      o.FeeAmount = data.Get<decimal>("feeAmount", o.FeeAmount);
      o.Rule = data.Get<string>("rule", o.Rule);
      o.LegalBasis = data.Get<string>("legalBasis", o.LegalBasis);

      return o;
    }

    static public FilingFee Empty {
      get {
        return new FilingFee();
      }
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("FilingFeeType")]
    public string FilingFeeType {
      get;
      private set;
    } = String.Empty;


    [DataField("FilingFeeAmount")]
    public decimal FeeAmount {
      get;
      private set;
    } = decimal.Zero;


    [DataField("FilingFeeRule")]
    public string Rule {
      get;
      private set;
    } = String.Empty;


    [DataField("FilingFeeLegalBasis")]
    public string LegalBasis {
      get;
      private set;
    } = String.Empty;


    #endregion Properties

  }  // class FilingFee

}  // namespace Empiria.Steps.Modeling
