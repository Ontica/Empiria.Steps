/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Meetings                             Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : MeetingsParticipantsController               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web services to manage the list of participants of a project meeting.                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Contacts;
using Empiria.WebApi;

namespace Empiria.ProjectManagement.Meetings.WebApi {

  /// <summary>Web services to manage the list of participants of a project meeting.</summary>
  public class MeetingsParticipantsController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/project-management/meetings/{meetingUID}/participants/available")]
    public SingleObjectModel GetParticipants(string meetingUID) {
      try {
        var meeting = Meeting.Parse(meetingUID);

        FixedList<Contact> participants = meeting.GetAvailableParticipants();

        return new SingleObjectModel(this.Request, participants.ToResponse(),
                                     typeof(Meeting).FullName);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

    #region UPDATE methods

    [HttpPost]
    [Route("v1/project-management/meetings/{meetingUID}/participants")]
    public SingleObjectModel AddParticipant(string meetingUID,
                                            [FromBody] object body) {
      try {
        string participantUID = base.GetFromBody<string>(body, "participantUID");

        var participant = Contact.Parse(participantUID);

        var meeting = Meeting.Parse(meetingUID);

        // meeting.AddParticipant(participant);

        using (var context = StorageContext.Open()) {

          meeting.AddParticipant(participant);

          meeting.SaveAll();

          context.Commit();

          return new SingleObjectModel(this.Request, meeting.ToResponse(),
                                       typeof(Meeting).FullName);
        }

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpDelete]
    [Route("v1/project-management/meetings/{meetingUID}/participants/{participantUID}")]
    public SingleObjectModel RemoveParticipant(string meetingUID, string participantUID) {
      try {
        var participant = Contact.Parse(participantUID);

        var meeting = Meeting.Parse(meetingUID);

        using (var context = StorageContext.Open()) {

          meeting.RemoveParticipant(participant);

          meeting.SaveAll();

          context.Commit();

          return new SingleObjectModel(this.Request, meeting.ToResponse(),
                                       typeof(Meeting).FullName);
        }

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class MeetingsParticipantsController

}  // namespace Empiria.ProjectManagement.Meetings.WebApi
