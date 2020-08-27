/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                        Component : Web Api                               *
*  Assembly : Empiria.Steps.Design.WebApi.dll              Pattern   : Response methods                      *
*  Type     : StepDataObjectModels                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response methods for process design data.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Data.DataObjects;

namespace Empiria.Steps.Design.WebApi {

  /// <summary>Response methods for process design data.</summary>
  static internal class StepDataSourceResponseModels {

    static private string libraryBaseAddress = ConfigurationData.GetString("Empiria.Governance", "DocumentsLibrary.BaseAddress");

    #region Response methods

    static internal ICollection ToResponse(this IList<DataStore> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        array.Add(item.ToResponse());
      }
      return array;
    }


    static internal object ToResponse(this DataStore dataStore) {
      return new {
        uid = dataStore.UID,
        type = dataStore.NamedKey,
        family = dataStore.Family,
        name = dataStore.Name,
        description = dataStore.Description,
        templateUrl = dataStore.Template.Replace("~", libraryBaseAddress)
      };
    }

    #endregion Response methods

  }  // class StepDataSourceResponseModels

}  // namespace Empiria.Steps.Design.WebApi
