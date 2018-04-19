/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Meetings                             Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : MeetingsResponseModel                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for help desk tickets and their related entities.                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Contacts;

namespace Empiria.ProjectManagement.Meetings.WebApi {

  /// <summary>Response static methods for help desk tickets and their related entities.</summary>
  static internal class MeetingsResponseModel {

    static internal ICollection ToResponse(this IList<Meeting> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var meeting in list) {
        var item = meeting.ToResponse();

        array.Add(item);
      }
      return array;
    }


    static internal object ToResponse(this Meeting meeting) {
      return new {
        uid = meeting.UID,
        controlNo = meeting.ControlNo,
        title = meeting.Title,
        description = meeting.Description,
        project = new {
          uid = meeting.Project.UID,
          name = meeting.Project.Name
        },
        date = meeting.Date.ToResponse(),
        startTime = meeting.StartTime,
        endTime = meeting.EndTime,
        location = meeting.Location,
        status = meeting.Status,

        participants = meeting.Participants.ToResponse(),

        topics = new string[0],

        recommendations = new string[0],

        agreements = new string[0]
      };

    }

    static internal ICollection ToResponse(this IList<Contact> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var contact in list) {
        var item = new {
          uid = contact.UID,
          shortName = contact.Alias,
          email = contact.EMail
        };
        array.Add(item);
      }

      return array;
    }

    static internal string ToResponse(this DateTime dateTime) {
      if (ExecutionServer.DateMinValue == dateTime ||
          ExecutionServer.DateMaxValue == dateTime) {
        return String.Empty;
      } else {
        return dateTime.ToString("yyyy-MM-dd");
      }
    }

  }  // class MeetingsResponseModel

}  // namespace Empiria.ProjectManagement.Meetings.WebApi
