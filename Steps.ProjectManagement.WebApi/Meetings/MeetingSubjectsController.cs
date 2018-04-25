///* Empiria Steps *********************************************************************************************
//*                                                                                                            *
//*  Module   : Project Meetings                             Component : Web Api                               *
//*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
//*  Type     : MeetingSubjectsController                    License   : Please read LICENSE.txt file          *
//*                                                                                                            *
//*  Summary  : Set of web services used to interact with the list of subjects of a project meeting.           *
//*                                                                                                            *
//************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
//using System;
//using System.Web.Http;

//using Empiria.WebApi;

//namespace Empiria.ProjectManagement.Meetings.WebApi {

//  /// <summary>Set of web services used to interact with the list of subjects of a project meeting.</summary>
//  public class MeetingSubjectsController : WebApiController {

//    #region GET methods

//    [HttpGet]
//    [Route("v1/project-management/meetings/{meetingUID}/subjects/available")]
//    public SingleObjectModel GetAvailableSubjects(string meetingUID) {
//      try {
//        var meeting = Meeting.Parse(meetingUID);

//        FixedList<MeetingSubject> subjects = meeting.GetAvailableSubjects();

//        return new SingleObjectModel(this.Request, subjects.ToResponse(),
//                                     typeof(Meeting).FullName);
//      } catch (Exception e) {
//        throw base.CreateHttpException(e);
//      }
//    }

//    #endregion GET methods

//    #region UPDATE methods

//    [HttpPost]
//    [Route("v1/project-management/meetings/{meetingUID}/subjects")]
//    public SingleObjectModel AddSubject(string meetingUID,
//                                        [FromBody] object body) {
//      try {
//        string subjectUID = base.GetFromBody<string>(body, "meetingSubjectUID");

//        var subject = MeetingSubject.Parse(subjectUID);

//        var meeting = Meeting.Parse(meetingUID);

//        meeting.AddSubject(subject);

//        meeting.Save();

//        return new SingleObjectModel(this.Request, meeting.ToResponse(),
//                                     typeof(Meeting).FullName);
//      } catch (Exception e) {
//        throw base.CreateHttpException(e);
//      }
//    }


//    [HttpDelete]
//    [Route("v1/project-management/meetings/{meetingUID}/participants/{subjectUID}")]
//    public SingleObjectModel RemoveSubject(string meetingUID, string subjectUID) {
//      try {
//        var subject = MeetingSubject.Parse(subjectUID);

//        var meeting = Meeting.Parse(meetingUID);

//        meeting.RemoveSubject(subject);

//        meeting.Save();

//        return new SingleObjectModel(this.Request, meeting.ToResponse(),
//                                     typeof(Meeting).FullName);
//      } catch (Exception e) {
//        throw base.CreateHttpException(e);
//      }
//    }

//    #endregion UPDATE methods

//  }  // class MeetingSubjectsController

//}  // namespace Empiria.ProjectManagement.Meetings.WebApi
