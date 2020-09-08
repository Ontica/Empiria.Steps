/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                        Component : Web Api                               *
*  Assembly : Empiria.Steps.Design.WebApi.dll              Pattern   : Controller                            *
*  Type     : AutofillController                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web api methods for the autofill functionality.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Steps.Design.DataObjects;
using Empiria.ProjectManagement;

namespace Empiria.Steps.Design.WebApi {

  /// <summary>Web api methods for the autofill functionality.</summary>
  public class AutofillController : WebApiController {

    #region Write methods

    [HttpDelete]
    [Route("v3/empiria-steps/data-objects/{dataObjectUID}/autofill/{activityUID}")]
    public SingleObjectModel DiscardAutofillFile([FromUri] string dataObjectUID,
                                                 [FromUri] string activityUID) {
      try {
        var dataObject = StepDataObject.Parse(dataObjectUID);
        var activity = ProjectItem.Parse(activityUID);

        var autofill = new Autofill(dataObject, activity);

        autofill.RemoveMediaFile();

        return new SingleObjectModel(this.Request, autofill.ToResponse(),
                                     typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v3/empiria-steps/data-objects/{dataObjectUID}/autofill/{activityUID}")]
    public SingleObjectModel GenerateAutofillFile([FromUri] string dataObjectUID,
                                                  [FromUri] string activityUID) {
      try {
        var dataObject = StepDataObject.Parse(dataObjectUID);
        var activity = ProjectItem.Parse(activityUID);

        var autofill = new Autofill(dataObject, activity);

        autofill.GenerateMediaFile();

        return new SingleObjectModel(this.Request, autofill.ToResponse(),
                                     typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpDelete]
    [Route("v3/empiria-steps/data-objects/{dataObjectUID}/upload-file/{activityUID}")]
    public SingleObjectModel RemoveUploadedFile([FromUri] string dataObjectUID,
                                                [FromUri] string activityUID) {
      try {
        var dataObject = StepDataObject.Parse(dataObjectUID);
        var activity = ProjectItem.Parse(activityUID);

        var autofill = new Autofill(dataObject, activity);

        autofill.RemoveFile();

        return new SingleObjectModel(this.Request, autofill.ToResponse(),
                                     typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v3/empiria-steps/data-objects/{dataObjectUID}/upload-file/{activityUID}")]
    public SingleObjectModel UploadFile([FromUri] string dataObjectUID,
                                        [FromUri] string activityUID) {
      try {
        var request = HttpContext.Current.Request;

        var dataObject = StepDataObject.Parse(dataObjectUID);
        var activity = ProjectItem.Parse(activityUID);

        var autofill = new Autofill(dataObject, activity);

        autofill.UploadFile(request.Files[0]);

        return new SingleObjectModel(this.Request, autofill.ToResponse(),
                                     typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion Write methods

  }  // class AutofillController

}  // namespace Empiria.Steps.Design.WebApi
