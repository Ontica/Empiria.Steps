/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : InboxController                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web services used to interact with task inboxes.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Web services used to interact with task inboxes.</summary>
  public class InboxController : WebApiController {

    #region Get methods

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


    [HttpGet]
    [Route("v1/project-management/projects/as-work-list")]
    public CollectionModel GetProjectActivitiesAsWorklist([FromUri] ActivityFilter filter = null,
                                                          [FromUri] ActivityOrder orderBy = ActivityOrder.Default) {
      try {

        if (filter == null) {
          filter = new ActivityFilter();
        }

        var finder = new ProjectFinder(filter);

        FixedList<ProjectItem> activities = finder.GetActivitiesList(orderBy);

        return new CollectionModel(this.Request, activities.ToResponse(),
                                   typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Get methods

  }  // class InboxController

}  // namespace Empiria.ProjectManagement.WebApi
