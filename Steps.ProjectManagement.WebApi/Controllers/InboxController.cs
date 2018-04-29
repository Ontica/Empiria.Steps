/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : InboxController                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web services to interact with task inboxes.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Web services to interact with task inboxes.</summary>
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

        FixedList<ProjectItem> activities = finder.GetActivitiesList(orderBy);

        return new CollectionModel(this.Request, activities.ToInboxResponse(),
                                   typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class InboxController

}  // namespace Empiria.ProjectManagement.WebApi
