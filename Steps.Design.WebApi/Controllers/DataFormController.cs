/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                        Component : Web Api                               *
*  Assembly : Empiria.Steps.Design.WebApi.dll              Pattern   : Controller                            *
*  Type     : DataFormController                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web api for data form features.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

using Empiria.Data.DataObjects;

using Empiria.Steps.Design.DataObjects;

namespace Empiria.Steps.Design.WebApi {

  /// <summary>Web api for data form features.</summary>
  public class DataFormController: WebApiController {

    #region Get method

    [HttpGet]
    [Route("v3/empiria-steps/data-objects/{dataObjectUID}/data-form")]
    public SingleObjectModel GetDataFormFields([FromUri] string dataObjectUID) {
      try {

        var dataObject = StepDataObject.Parse(dataObjectUID);

        var fields = dataObject.DataItem.GetFormFields();

        var values = dataObject.GetFormFields();

        foreach (DataFormField field in fields) {
          if (values.ContainsKey(field.Key)) {
            field.Value = Convert.ToString(values[field.Key]);
          }
        }

        return new SingleObjectModel(this.Request, fields,
                                   typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


     #endregion Get methods

    #region Write methods


    [HttpPost]
    [Route("v3/empiria-steps/data-objects/{dataObjectUID}/data-form")]
    public SingleObjectModel SetDataFormFields([FromUri] string dataObjectUID, [FromBody] object body) {
      try {

        var dataObject = StepDataObject.Parse(dataObjectUID);

        var formData = base.GetFromBody<string>(body, "formData");

        dataObject.SaveFormData(JsonObject.Parse(formData));

        return new SingleObjectModel(this.Request, dataObject.ToResponse(),
                                     typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Write methods

  }  // class DataFormController

}  // namespace Empiria.Steps.Design.WebApi
