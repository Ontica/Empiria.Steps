/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : FilingDocuments                                  License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Contains data about a procedure filing fee conditions.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Steps.Modeling {

  public class FilingDocuments {

    #region Constructors and parsers

    private FilingDocuments() {
      // Required by Empiria Framework.
    }

    static internal FilingDocuments Parse(JsonObject data) {
      var o = new FilingDocuments();

      o.Notes = data.Get<string>("notes", o.Notes);

      return o;
    }

    static public FilingDocuments Empty {
      get {
        return new FilingDocuments();
      }
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("RequirementsNotes")]
    public string Notes {
      get;
      private set;
    } = String.Empty;

    #endregion Properties

  }  // class FilingDocuments

}  // namespace Empiria.Steps.Modeling
