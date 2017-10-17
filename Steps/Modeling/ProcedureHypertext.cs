/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : LegalInfo                                        License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Holds procedure hypertext strings.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Steps.Presentation;

namespace Empiria.Steps.Modeling {

  /// <summary>Holds procedure hypertext strings.</summary>
  public class ProcedureHypertext {

    #region Constructors and parsers

    public ProcedureHypertext(Procedure procedure) {
      this.Procedure = procedure;
    }

    #endregion Constructors and parsers

    #region Properties

    private Procedure Procedure {
      get;
      set;
    }

    private string _legalBasisHypertext = null;
    public string LegalBasis {
      get {
        if (_legalBasisHypertext == null) {
          _legalBasisHypertext =
              Hypertext.ToAcronymHypertext(this.Procedure.LegalInfo.LegalBasis);
        }
        return _legalBasisHypertext;
      }
    }


    private string _notesHypertext = null;
    public string Notes {
      get {
        if (_notesHypertext == null) {
          _notesHypertext = Hypertext.ToAcronymHypertext(this.Procedure.Notes);
        }
        return _notesHypertext;
      }
    }

    private string _maxFilingTermNotesHypertext = null;
    public string MaxFilingTermNotes {
      get {
        if (_maxFilingTermNotesHypertext == null) {
          _maxFilingTermNotesHypertext =
              Hypertext.ToAcronymHypertext(this.Procedure.FilingCondition.MaxFilingTermNotes);
        }
        return _maxFilingTermNotesHypertext;
      }
    }


    private string _deferralsTermNotesHypertext = null;
    public string DeferralsTermNotes {
      get {
        if (_deferralsTermNotesHypertext == null) {
          _deferralsTermNotesHypertext =
              Hypertext.ToAcronymHypertext(this.Procedure.FilingCondition.DeferralsTermNotes);
        }
        return _deferralsTermNotesHypertext;
      }
    }

    #endregion Properties

  }  // class ProcedureHypertext

}  // namespace Empiria.Steps.Modeling
