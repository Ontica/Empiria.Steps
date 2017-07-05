/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : LegalInfo                                        License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Holds information about a procedure legal information.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Steps.Modeling {

  public class LegalInfo {

    #region Constructors and parsers

    private LegalInfo() {
      // Required by Empiria Framework.
    }

    static internal LegalInfo Parse(JsonObject data) {
      var o = new LegalInfo();

      o.IsRegulated = data.Get<bool>("isRegulated", o.IsRegulated);
      o.Obligation = data.Get<string>("obligation", o.Obligation);
      o.LegalBasis = data.Get<string>("legalBasis", o.LegalBasis);

      o.ContractClausesAndAnnexes = data.Get<string>("contractClausesAndAnnexes",
                                                      o.ContractClausesAndAnnexes);
      return o;
    }

    static public LegalInfo Empty {
      get {
        return new LegalInfo();
      }
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("IsRegulated")]
    public bool IsRegulated {
      get;
      private set;
    } = true;


    [DataField("Obligation")]
    public string Obligation {
      get;
      private set;
    } = String.Empty;


    [DataField("LegalBasis")]
    public string LegalBasis {
      get;
      private set;
    } = String.Empty;


    [DataField("ContractClausesAndAnnexes")]
    public string ContractClausesAndAnnexes {
      get;
      private set;
    } = String.Empty;


    #endregion Properties

  }  // class LegalInfo

}  // namespace Empiria.Steps.Modeling
