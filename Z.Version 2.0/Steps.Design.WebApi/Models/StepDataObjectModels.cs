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
using Empiria.ProjectManagement;

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


    static internal ICollection ToResponse(this IList<StepDataObject> list, ProjectItem activity) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        if (!item.DataItem.NamedKey.Contains("WebForm") && !item.DataItem.NamedKey.Contains("WebGrid")) {
          array.Add(ConvertToAutofill(item, activity).ToResponse());
        } else {
          array.Add(item.ToResponse(activity));
        }
      }
      return array;
    }


    private static Autofill ConvertToAutofill(StepDataObject dataObject, ProjectItem activity) {
      return new Autofill(dataObject, activity);
    }


    static internal object ToResponse(this StepDataObject dataObject) {
      return new {
        uid = dataObject.UID,
        type = dataObject.DataItem.NamedKey,
        name = dataObject.Name,
        description = dataObject.Description,
        dataObject = dataObject.DataItem.ToResponse(),
        optional = dataObject.Optional,
        legalBasis = dataObject.LegalBasis,
      };
    }

    // static internal object ToResponse(this DataStore dataStore) {
    //  return new {
    //    uid = dataStore.UID,
    //    type = dataStore.NamedKey,
    //    family = dataStore.Family,
    //    name = dataStore.Name,
    //    description = dataStore.Description,
    //    templateUrl = dataStore.Template.Replace("~", libraryBaseAddress)
    //  };
    //}


    static internal object ToResponse(this StepDataObject dataObject, ProjectItem activity) {
      return new {
        uid = dataObject.UID,
        type = dataObject.DataItem.NamedKey,
        entity = dataObject.Step.ToIdentifiableResponse(x => x.Name),
        subject = activity.ToIdentifiableResponse(x => x.Name),
        action = dataObject.Action,
        family = dataObject.DataItem.Family,
        name = dataObject.Name,
        description = dataObject.Description,
        isOptional = dataObject.Optional == "Mandatory",
        mediaFormat = dataObject.MediaFormat,
        autofillFileUrl = String.Empty,
        uploadedFileUrl = String.Empty,
        templateUrl = dataObject.DataItem.Template.Replace("~", libraryBaseAddress),
        decorator = dataObject.DataItem.Terms,
        status = dataObject.Status,
        dataObject = dataObject.DataItem.ToResponse(),
        optional = dataObject.Optional,
        legalBasis = dataObject.LegalBasis,
      };
    }


    static internal object ToResponse(this Autofill autofill) {
      var dataObject = autofill.StepDataObject;
      var activity = autofill.Activity;

      return new {
        uid = dataObject.UID,
        type = dataObject.DataItem.NamedKey,
        entity = dataObject.Step.ToIdentifiableResponse(x => x.Name),
        subject = activity.ToIdentifiableResponse(x => x.Name),
        action = dataObject.Action,
        family = dataObject.DataItem.Family,
        name = dataObject.Name,
        description = dataObject.Description,
        isOptional = dataObject.Optional == "Mandatory",
        mediaFormat = dataObject.MediaFormat,
        autofillFileUrl = autofill.AutofillFileUrl,
        uploadedFileUrl = autofill.UploadedFileUrl,
        templateUrl = dataObject.DataItem.Template.Replace("~", libraryBaseAddress),
        decorator = dataObject.DataItem.Terms,
        status = dataObject.Status,
        dataObject = dataObject.DataItem.ToResponse(),
        optional = dataObject.Optional,
        legalBasis = dataObject.LegalBasis,
      };
    }

    #endregion Response methods

  }  // class StepDataObjectModels

}  // namespace Empiria.Steps.Design.WebApi
