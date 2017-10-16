/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : DocumentsController                              License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : API to get and set documents data.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.Legal;

namespace Empiria.Steps.WebApi {

  /// <summary>API to get and set documents data.</summary>
  public class DocumentsController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/documents")]
    public CollectionModel GetDocuments([FromUri] string type = "",
                                        [FromUri] string keywords = "") {
      try {

        var documents = Document.GetList(type, keywords);

        return new CollectionModel(this.Request, documents.ToResponse(),
                                   typeof(Document).FullName);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

    #region UPDATE methods

    [HttpPost]
    [Route("v1/documents/update-all")]
    public void UpdateAllDocuments() {
      try {
        Document.UpdateAll();

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class DocumentsController

}  // namespace Empiria.Steps.WebApi
