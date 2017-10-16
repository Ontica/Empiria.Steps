/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Data Service                        *
*  Type     : DocumentsData                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Data read and write methods for the documents library.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Steps.Legal {

  /// <summary>Data read and write methods for the documents library.</summary>
  static internal class DocumentsData {

    static internal void WriteDocument(Document o) {
      var op = DataOperation.Parse("writeBPMDocument",
                                   o.Id, o.UID, o.Keywords);

      DataWriter.Execute(op);
    }

  }  // class DocumentsData

}  // namespace Empiria.Steps.Legal
