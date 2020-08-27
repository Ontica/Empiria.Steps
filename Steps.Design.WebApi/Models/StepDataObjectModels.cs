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

using Empiria.WebApi;

using Empiria.Steps.Design.DataObjects;

namespace Empiria.Steps.Design.WebApi {

  /// <summary>Response methods for process design data.</summary>
  static internal class StepDataObjectModels {

    static private string libraryBaseAddress = ConfigurationData.GetString("Empiria.Governance", "DocumentsLibrary.BaseAddress");

    #region Response methods

    static internal ICollection ToResponse(this IList<StepDataObject> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        array.Add(item.ToResponse());
      }
      return array;
    }


    static internal object ToResponse(this StepDataObject dataObject) {
      return new {
        uid = dataObject.UID,
        type = dataObject.DataItem.NamedKey,
        entity = dataObject.Step.ToIdentifiableResponse(x => x.Name),
        subject = dataObject.Activity.ToIdentifiableResponse(x => x.Name),
        action = dataObject.Action,
        family = dataObject.DataItem.Family,
        name = dataObject.DataItem.Name,
        description = dataObject.DataItem.Description,
        mediaFormat = dataObject.MediaFormat,
        autofillFileUrl = dataObject.AutofillFileUrl,
        uploadedFileUrl = dataObject.UploadedFileUrl,
        templateUrl = dataObject.DataItem.Template.Replace("~", libraryBaseAddress),
        status = dataObject.Status
      };
    }

    #endregion Response methods

  }  // class StepDataObjectModels

}  // namespace Empiria.Steps.Design.WebApi
