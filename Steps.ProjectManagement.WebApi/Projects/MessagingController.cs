/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : MessagingController                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API for project management messaging services.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Threading.Tasks;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Contacts;
using Empiria.ProjectManagement.Messaging;


namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API for project management messaging services.</summary>
  public class MessagingController : WebApiController {


    #region Post methods

    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/send-all-activities-email/{sendToUID}")]
    public async Task<SingleObjectModel> SendAllActivitiesEmail([FromUri] string projectUID,
                                                                [FromUri] string sendToUID) {
      try {
        var project = Project.Parse(projectUID);

        var sendTo = (Person) Contact.Parse(sendToUID);

        var messenger = new ProjectManagementMessenger();

        await messenger.SendAllActivitiesEmail(project, sendTo);

        return new SingleObjectModel(this.Request,
                                     $"Project global status e-mail was sent to {sendTo.Alias}.");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/send-personal-activities-email/{sendToUID}")]
    public async Task<SingleObjectModel> SendPersonalActivitiesEmail([FromUri] string projectUID,
                                                                     [FromUri] string sendToUID) {
      try {
        var project = Project.Parse(projectUID);

        var sendTo = (Person) Contact.Parse(sendToUID);

        var messenger = new ProjectManagementMessenger();

        await messenger.SendPersonalActivitiesEmail(project, sendTo);

        return new SingleObjectModel(this.Request,
                                     $"Personal activities e-mail was sent to {sendTo.Alias}.");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Post methods

  }  // class MessagingController

}  // namespace Empiria.ProjectManagement.WebApi
