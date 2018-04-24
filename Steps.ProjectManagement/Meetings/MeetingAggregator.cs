/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Meetings                             Component : Domain services                       *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Aggregator                            *
*  Type     : MeetingAggregator                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Handles information about a project meeting.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Data;

namespace Empiria.ProjectManagement.Meetings {

  internal class MeetingAggregator {

    #region Fields

    private Meeting meeting;

    private Lazy<List<Contact>> _participants;

    #endregion Fields

    #region Constructors and parsers

    internal MeetingAggregator(Meeting meeting) {
      Assertion.AssertObject(meeting, "meeting");

      this.meeting = meeting;
      _participants = new Lazy<List<Contact>>(() => MeetingData.GetMeetingParticipants(this.meeting));

    }

    #endregion Constructors and parsers


    #region Participants methods

    internal void AddParticipant(Contact participant) {
      MeetingData.AddParticipant(this.meeting, participant);

      _participants.Value.Add(participant);
    }


    internal FixedList<Contact> GetParticipants() {
      return this._participants.Value.ToFixedList();
    }


    internal void RemoveParticipant(Contact participant) {
      MeetingData.RemoveParticipant(this.meeting, participant);

      _participants.Value.Remove(participant);
    }


    #endregion Participants methods

  }  // class MeetingAggregator

}  //namespace Empiria.ProjectManagement.Meetings
