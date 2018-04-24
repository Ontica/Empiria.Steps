/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Meetings                             Component : Domain services                       *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Data Services                         *
*  Type     : MeetingData                                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Data read and write methods for project meetings.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using Empiria.Contacts;
using Empiria.Data;

namespace Empiria.ProjectManagement.Meetings {

  /// <summary>Data read and write methods for project meetings.</summary>
  static internal class MeetingData {

    #region Public methods

    static internal string GetNextMeetingControlNo(Project project) {
      var op = DataOperation.Parse("getPMNextMeetingControlForProject", project.Id);

      var lastTicket = DataReader.GetScalar<string>(op, "SHL-0000");

      return EmpiriaString.IncrementCounter(lastTicket, "SHL-");
    }


    static internal FixedList<Meeting> GetOpenedMeetings(string keywords) {
      string filter = GetMeetingsFilter(keywords, "Status = 'O'");

      return BaseObject.GetList<Meeting>(filter, "StartTime")
                       .ToFixedList();
    }

    static internal FixedList<Meeting> SearchMeetings(string keywords) {
      string filter = GetMeetingsFilter(keywords);

      return BaseObject.GetList<Meeting>(filter, "StartTime")
                       .ToFixedList();
    }


    static internal void WriteMeeting(Meeting o) {
      var op = DataOperation.Parse("writePMMeeting",
                                   o.Id, o.UID, o.Project.Id, o.ControlNo,
                                   o.Title, o.Description, o.Tags,
                                   o.ExtensionData.ToString(), o.Keywords,
                                   o.StartDateTime, o.EndDateTime, (char) o.Status);

      DataWriter.Execute(op);
    }


    #endregion Public methods

    #region Participants methods

    static internal List<Contact> GetMeetingParticipants(Meeting meeting) {
      var objectTypeInfo = meeting.GetEmpiriaType();
      var linkName = "Meeting->Participants";

      var association = objectTypeInfo.Associations[linkName];

      return new List<Contact>(association.GetLinks<Contact>(meeting));
    }


    internal static void AddParticipant(Meeting meeting, Contact participant) {
      var objectTypeInfo = meeting.GetEmpiriaType();
      //var linkName = "Meeting->Participants";

      //var association = objectTypeInfo.Associations[linkName];

      // association.AddLink(meeting, participant);
      var op = DataOperation.Parse("do", meeting.UID);

      DataWriter.ExecuteWhenRootSaved(op, meeting);
    }

    internal static void RemoveParticipant(Meeting meeting, Contact participant) {
      var objectTypeInfo = meeting.GetEmpiriaType();
      //var linkName = "Meeting->Participants";

      //var association = objectTypeInfo.Associations[linkName];

      // association.RemoveLink(participant);
      var op = DataOperation.Parse("do", meeting.UID);

      DataWriter.ExecuteWhenRootSaved(op, meeting);
    }


    #endregion Participants methods

    #region Private methods

    static private string GetMeetingsFilter(string keywords, string statusFilter = "") {
      string filter = String.Empty;

      if (!String.IsNullOrWhiteSpace(keywords)) {
        filter = SearchExpression.ParseAndLike("Keywords", EmpiriaString.BuildKeywords(keywords));
      }

      if (statusFilter.Length != 0) {
        filter += filter.Length != 0 ? " AND " : String.Empty;
        filter += statusFilter;
      }

      return filter;
    }

    #endregion Private methods

  }  // class MeetingData

}  // namespace Empiria.ProjectManagement.Meetings
