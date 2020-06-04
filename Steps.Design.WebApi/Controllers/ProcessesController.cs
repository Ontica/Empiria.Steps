/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                        Component : Web Api                               *
*  Assembly : Empiria.Steps.Design.WebApi.dll              Pattern   : Controller                            *
*  Type     : ProcessesController                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API used to retrieve processes design data.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

namespace Empiria.Steps.Design.WebApi {

  /// <summary>Public API used to retrieve processes design data.</summary>
  public class ProcessesController : WebApiController {

    #region Get methods

    [HttpGet]
    [Route("v3/steps/design/processes")]
    public CollectionModel GetProcessDesignList() {
      try {
        var list = Process.GetList<Process>("Accessibility = 'Public'");

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Process).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Get methods

  }  // class ProcessesController

}  // namespace Empiria.Steps.Design.WebApi
