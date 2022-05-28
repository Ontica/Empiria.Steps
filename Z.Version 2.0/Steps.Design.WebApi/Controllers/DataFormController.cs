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
using Empiria.Postings;

using Empiria.Steps.Design.DataObjects;
using Empiria.ProjectManagement;
using System.Linq;


namespace Empiria.Steps.Design.WebApi {

  /// <summary>Web api for data form features.</summary>
  public class DataFormController: WebApiController {

    #region Get method

    [HttpGet]
    [Route("v3/empiria-steps/data-graph")]
    public SingleObjectModel GetGraphData() {
      try {

        var graphData = ProjectItem.GetGraphData();


        return new SingleObjectModel(this.Request, graphData,
                                     typeof(DataFormField).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


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
                                   typeof(DataFormField).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v3/empiria-steps/data-objects/{dataObjectUID}/grid-data-source/{activityUID}")]
    public CollectionModel GetGridFormDataSource([FromUri] string dataObjectUID,
                                                 [FromUri] string activityUID) {
      try {

        var dataObject = StepDataObject.Parse(dataObjectUID);

        var activity = ProjectItem.Parse(activityUID);

        FixedList<Posting> list = PostingList.GetPostings(dataObject.DataItem,
                                                          activity,
                                                          dataObject.DataItem.DataType);

        var data = list.Select(x => {
          var json = x.ExtensionData.ToDictionary();

          json.Add("uid", x.UID);

          if (dataObject.DataItem.Terms == "Actividades.CNH.CustomGrid") {
            json = Reporting.SubtaskCNH.LoadFields(dataObject, json, activity);
          }

          return json;
        });

        return new CollectionModel(this.Request, data.ToArray());

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Get methods

    #region Write methods



    [HttpPost]
    [Route("v3/empiria-steps/data-objects/{dataObjectUID}/data-form/{activityUID}")]
    public SingleObjectModel SetDataFormFields([FromUri] string dataObjectUID,
                                               [FromUri] string activityUID,
                                               [FromBody] object body) {
      try {

        var dataObject = StepDataObject.Parse(dataObjectUID);

        var json = JsonObject.Parse(body);

        var eventType = json.Get<string>("type");
        var formData = JsonObject.Parse(json.Get<string>("payload/formData"));

        var activity = ProjectItem.Parse(activityUID);

        Posting posting;

        if (eventType == "created") {
          posting = new Posting(dataObject.DataItem.DataType,
                                dataObject.DataItem, activity);
          posting.ExtensionData = formData;
          posting.Save();

        } else if (eventType == "updated") {

          posting = Posting.Parse(json.Get<string>("payload/uid"));
          posting.ExtensionData = formData;
          posting.Save();

        } else if (eventType == "deleted") {

          posting = Posting.Parse(json.Get<string>("payload/uid"));
          posting.Delete();

        } else {
          throw Assertion.EnsureNoReachThisCode($"Unrecognized event {eventType}.");
        }

        return new SingleObjectModel(this.Request, dataObject.ToResponse(activity),
                                     typeof(StepDataObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Write methods

  }  // class DataFormController

}  // namespace Empiria.Steps.Design.WebApi
