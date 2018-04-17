/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Web Api Controller                  *
*  Type     : ActivityTasksController                          License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Public API to retrieve and control activities tasks (checklists).                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Steps.ProjectManagement;

namespace Empiria.Governance.WebApi {

  /// <summary>Public API to retrieve and control activities tasks (checklists).</summary>
  public class ActivityTasksController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/project-management/activities/{activityUID}/tasks")]
    public CollectionModel GetActivityTasks(string activityUID) {
      try {
        Activity activity = null;

        if (EmpiriaString.IsInteger(activityUID)) {
          activity = Activity.Parse(int.Parse(activityUID));
        } else {
          activity = Activity.Parse(activityUID);
        }

        return new CollectionModel(this.Request, activity.Tasks.ToResponse(),
                                   typeof(ProjectObject).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

  }  // class ActivityTasksController

}  // namespace Empiria.Governance.WebApi
