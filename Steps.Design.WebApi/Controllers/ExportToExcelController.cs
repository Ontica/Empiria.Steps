/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Data Export Services                         Component : Web Api                               *
*  Assembly : Empiria.Steps.WebApi.dll                     Pattern   : Controller                            *
*  Type     : ExportToExcelController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web api methods that export steps and activites data to Excel files.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Steps.Services;

namespace Empiria.Steps.WebApi {

  /// <summary>Web api methods that export steps and activites data to Excel files.</summary>
  public class ExportToExcelController : WebApiController {

    #region Export methods

    [HttpGet]
    [Route("v3/empiria-steps/projects/{projectUID}/export-to-excel/{branchUID?}")]
    public SingleObjectModel ExportProjectToExcelFile([FromUri] string projectUID,
                                                      [FromUri] string branchUID = "") {
      try {
        base.RequireResource(projectUID, "projectUID");

        string fileName = ProjectExporterServices.ExportToExcel(projectUID, branchUID);

        return new SingleObjectModel(this.Request, fileName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Export methods

  }  // class ExportToExcelController

}  // namespace Empiria.Steps.WebApi
