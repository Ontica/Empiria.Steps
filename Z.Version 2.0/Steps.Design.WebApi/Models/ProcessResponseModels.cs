/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                        Component : Web Api                               *
*  Assembly : Empiria.Steps.Design.WebApi.dll              Pattern   : Response methods                      *
*  Type     : ProcessResponseModels                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response methods for process design data.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.Steps.Design.WebApi {

  /// <summary>Response methods for process design data.</summary>
  static internal class ProcessResponseModels {

    #region Response methods

    static internal ICollection ToResponse(this IList<Process> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        array.Add(item.ToResponse());
      }
      return array;
    }


    static internal object ToResponse(this Process process) {
      return new {
        uid = process.UID,
        type= process.StepType.Name,
        name = process.Name,
        theme = process.Theme
      };
    }

    #endregion Response methods

  }  // class ProcessResponseModels

}  // namespace Empiria.Steps.Design.WebApi
