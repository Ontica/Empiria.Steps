/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : GanttController                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web services used to interact with the gantt component.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Web services used to interact with the gantt component.</summary>
  public class GanttController : WebApiController {

    #region Get methods

    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/as-gantt")]
    public CollectionModel GetProjectActivitiesAsGantt(string projectUID,
                                                       [FromUri] ActivityFilter filter = null,
                                                       [FromUri] ActivityOrder orderBy = ActivityOrder.Default) {
      try {

        if (filter == null) {
          filter = new ActivityFilter();
        }

        var project = Project.Parse(projectUID);

        var fullActivitiesList = project.GetActivities();

        return new CollectionModel(this.Request, fullActivitiesList.ToGanttResponse(),
                                   typeof(ProjectItem).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Get methods

  }  // class GanttController

}  // namespace Empiria.ProjectManagement.WebApi
