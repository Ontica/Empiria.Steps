/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : CataloguesController                             License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Get and set apis for very basic entities or value objects.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Web.Http;

using Empiria.DataTypes;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.Modeling;

namespace Empiria.Steps.WebApi {

  /// <summary>Get and set apis for very basic entities or value objects.</summary>
  public class CataloguesController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/catalogues/procedure-starts-when")]
    public CollectionModel GetProcedureStartsWhenList() {
      try {
        var list = Procedure.StartsWhenList;

        return new CollectionModel(this.Request, list, typeof(StartsWhen).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/catalogues/procedure-themes")]
    public CollectionModel GetProcedureThemesList() {
      try {
        var list = Procedure.ThemesList;

        return new CollectionModel(this.Request, list, "Steps.Procedure.Theme");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/catalogues/term-time-units")]
    public CollectionModel GetTermTimeUnitsList() {
      try {
        var list = Procedure.TermTimeUnitsList;

        return new CollectionModel(this.Request, list, typeof(TermTimeUnit).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class CataloguesController

}  // namespace Empiria.Steps.WebApi
