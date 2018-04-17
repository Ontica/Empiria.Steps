/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Web Api Controller                  *
*  Type     : TasksController                                  License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Public API to retrieve and set projects data.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Steps.ProjectManagement;

namespace Empiria.Governance.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class InboxController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/inboxes/my-inbox")]
    public CollectionModel GetMyInbox([FromUri] ActivityFilter filter = null,
                                      [FromUri] ActivityOrder orderBy = ActivityOrder.Default) {
      try {

        if (filter == null) {
          filter = new ActivityFilter();
        }

        var finder = new ProjectFinder(filter);

        FixedList<ProjectObject> activities = finder.GetActivitiesList(orderBy);

        return new CollectionModel(this.Request, activities.ToInboxResponse(),
                                   typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class TasksController

}  // namespace Empiria.Governance.WebApi
