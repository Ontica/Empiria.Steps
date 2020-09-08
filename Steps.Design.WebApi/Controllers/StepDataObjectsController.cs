﻿/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                        Component : Web Api                               *
*  Assembly : Empiria.Steps.Design.WebApi.dll              Pattern   : Controller                            *
*  Type     : StepDataObjectsController                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web api to interact with StepDataObject instances.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.ProjectManagement;

using Empiria.Data.DataObjects;

using Empiria.Steps.Design.DataObjects;

namespace Empiria.Steps.Design.WebApi {

  /// <summary>Web api to interact with StepDataObject instances.</summary>
  public class StepDataObjectsController : WebApiController {

    #region Get method

    [HttpGet]
    [Route("v3/empiria-steps/activities/{activityUID}/data-objects")]
    public CollectionModel GetActivityDataObjects([FromUri] string activityUID) {
      try {
        var activity = ProjectItem.Parse(activityUID);

        FixedList<StepDataObject> dataObjects = StepDataObject.GetListForAction(activity);

        return new CollectionModel(this.Request, dataObjects.ToResponse(activity),
                                   typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v3/empiria-steps/steps/{stepUID}/data-objects")]
    public CollectionModel GetStepDataObjects([FromUri] string stepUID) {
      try {
        var step = ProjectItem.Parse(stepUID);

        FixedList<StepDataObject> dataObjects = StepDataObject.GetListFor(step);

        return new CollectionModel(this.Request, dataObjects.ToResponse(),
                                   typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v3/empiria-steps/data-objects/data-sources")]
    public CollectionModel GetStepDataStores() {
      try {
        FixedList<DataStore> dataStores = DataStore.GetList();

        return new CollectionModel(this.Request, dataStores.ToResponse(),
                                   typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion Get methods

    #region Write methods

    [HttpPost]
    [Route("v3/empiria-steps/steps/{stepUID}/data-objects")]
    public SingleObjectModel LinkStepWithDataSource([FromUri] string stepUID, [FromBody] object body) {
      try {
        var step = ProjectItem.Parse(stepUID);

        var dataSource = base.GetFromBody<DataStore>(body, "dataSourceUID");

        StepDataObject dataObject = new StepDataObject(step, dataSource);

        dataObject.Save();

        return new SingleObjectModel(this.Request, dataObject.ToResponse(),
                                   typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpDelete]
    [Route("v3/empiria-steps/data-objects/{dataObjectUID}")]
    public NoDataModel RemoveDataObjectLink([FromUri] string dataObjectUID) {
      try {
        var dataObject = StepDataObject.Parse(dataObjectUID);

        dataObject.Delete();

        return new NoDataModel(this.Request);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion Write methods

  }  // class StepDataObjectsController

}  // namespace Empiria.Steps.Design.WebApi
