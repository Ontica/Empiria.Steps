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

      o.RegulationMode = data.Get<string>("regulationMode", o.RegulationMode);
      o.Obligation = data.Get<string>("obligation", o.Obligation);
      o.LegalBasis = data.Get<string>("legalBasis", o.LegalBasis);

      o.Ronda13Consorcio = data.Get<string>("ronda13Consorcio", o.Ronda13Consorcio);
      o.Ronda13Individual = data.Get<string>("ronda13Individual", o.Ronda13Individual);
      o.Ronda14Consorcio = data.Get<string>("ronda14Consorcio", o.Ronda14Consorcio);
      o.Ronda14Individual = data.Get<string>("ronda14Individual", o.Ronda14Individual);
      o.Ronda21Consorcio = data.Get<string>("ronda21Consorcio", o.Ronda21Consorcio);
      o.Ronda21Individual = data.Get<string>("ronda21Individual", o.Ronda21Individual);
      return o;
    }

    static public LegalInfo Empty {
      get {
        return new LegalInfo();
      }
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("RegulationMode")]
    public string RegulationMode {
      get;
      private set;
    } = String.Empty;


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


    [DataField("Ronda13Consorcio")]
    public string Ronda13Consorcio {
      get;
      private set;
    } = String.Empty;


    [DataField("Ronda13Individual")]
    public string Ronda13Individual {
      get;
      private set;
    } = String.Empty;


    [DataField("Ronda14Consorcio")]
    public string Ronda14Consorcio {
      get;
      private set;
    } = String.Empty;


    [DataField("Ronda14Individual")]
    public string Ronda14Individual {
      get;
      private set;
    } = String.Empty;


    [DataField("Ronda21Consorcio")]
    public string Ronda21Consorcio {
      get;
      private set;
    } = String.Empty;


    [DataField("Ronda21Individual")]
    public string Ronda21Individual {
      get;
      private set;
    } = String.Empty;

    #endregion Properties

  }  // class LegalInfo

}  // namespace Empiria.Steps.Modeling
