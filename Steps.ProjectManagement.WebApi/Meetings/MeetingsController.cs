/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Meetings                             Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : MeetingsController                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web API controller for project meetings and their related entities.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

namespace Empiria.ProjectManagement.Meetings.WebApi {

  /// <summary>Web API controller for project meetings and their related entities.</summary>
  public class MeetingsController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/project-management/meetings/{meetingUID}")]
    public SingleObjectModel GetMeeting(string meetingUID = "") {
      try {
        var meeting = Meeting.Parse(meetingUID);

        return new SingleObjectModel(this.Request, meeting.ToResponse(),
                                     typeof(Meeting).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/meetings/opened")]
    public CollectionModel GetUpcomingMeetings([FromUri] string keywords = "") {
      try {

        FixedList<Meeting> meetingList = Meeting.GetOpened(keywords);

        return new CollectionModel(this.Request, meetingList.ToResponse(),
                                   typeof(Meeting).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/meetings")]
    public CollectionModel SearchMeetings([FromUri] string keywords = "") {
      try {

        FixedList<Meeting> list= Meeting.Search(keywords);

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Meeting).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

    #region UPDATE methods

    [HttpPost]
    [Route("v1/project-management/meetings")]
    public SingleObjectModel CreateTicket([FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var meeting = new Meeting(bodyAsJson);

        meeting.Save();

        return new SingleObjectModel(this.Request, meeting.ToResponse(),
                                     typeof(Meeting).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/project-management/meetings/{meetingUID}/close")]
    public SingleObjectModel CloseMeeting(string meetingUID) {
      try {

        var meeting = Meeting.Parse(meetingUID);

        meeting.Close();

        meeting.Save();

        return new SingleObjectModel(this.Request, meeting.ToResponse(),
                                     typeof(Meeting).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/project-management/meetings/{meetingUID}")]
    public SingleObjectModel UpdateTicket(string meetingUID, [FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var meeting = Meeting.Parse(meetingUID);

        meeting.Update(bodyAsJson);

        meeting.Save();

        return new SingleObjectModel(this.Request, meeting.ToResponse(),
                                     typeof(Meeting).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpDelete]
    [Route("v1/project-management/meetings/{meetingUID}")]
    public NoDataModel DeleteTicket(string meetingUID) {
      try {

        var meeting = Meeting.Parse(meetingUID);

        meeting.Delete();

        meeting.Save();

        return new NoDataModel(this.Request);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class MeetingsController

}  // namespace Empiria.ProjectManagement.Meetings.WebApi
