/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Data Service                        *
*  Type     : RequirementsData                                 License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Data read and write methods for procedure requirements.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Steps.Modeling {

  /// <summary>Data read and write methods for procedure requirements.</summary>
  static internal class RequirementsData {

    static internal void WriteRequirement(Requirement o) {
      var op = DataOperation.Parse("writeBPMRequirement",
                                   o.Id, o.UID);

      DataWriter.Execute(op);
    }

  }  // class RequirementsData

}  // namespace Empiria.Steps.Legal
