﻿/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Meetings                             Component : Domain services                       *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Aggregate root                        *
*  Type     : Meeting                                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Handles information about a project meeting.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.DataTypes;
using Empiria.Json;
using Empiria.StateEnums;

namespace Empiria.ProjectManagement.Meetings {

    /// <summary>Handles information about a project meeting.</summary>
    public class Meeting : BaseObject {

    #region Constructors and parsers

    protected Meeting() {
      // Required by Empiria Framework.
    }


    public Meeting(JsonObject data) {
      this.Load(data);
    }


    static public Meeting Parse(string uid) {
      return BaseObject.ParseKey<Meeting>(uid);
    }


    static public FixedList<Meeting> GetOpened(string keywords) {
      return MeetingData.GetOpenedMeetings(keywords);
    }


    static public FixedList<Meeting> Search(string keywords) {
      return MeetingData.SearchMeetings(keywords);
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("UID")]
    public string UID {
      get;
      private set;
    } = String.Empty;


    [DataField("ProjectId")]
    public Project Project {
      get;
      private set;
    }


    [DataField("ControlNo")]
    public string ControlNo {
      get;
      private set;
    } = String.Empty;


    [DataField("Title")]
    public string Title {
      get;
      private set;
    } = String.Empty;


    [DataField("Description")]
    public string Description {
      get;
      private set;
    } = String.Empty;


    [DataField("Tags")]
    public string Tags {
      get;
      private set;
    } = String.Empty;


    public string Location {
      get {
        return this.ExtensionData.Get("location",
                                      String.Empty);
      }
      set {
        this.ExtensionData.SetIfValue("location", value);
      }
    }


    [DataField("ExtData")]
    protected internal JsonObject ExtensionData {
      get;
      private set;
    } = new JsonObject();


    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.ControlNo, this.Title,
                                           this.Tags, this.Description,
                                           this.Project.Keywords);
      }
    }


    [DataField("StartTime", Default = "ExecutionServer.DateMaxValue")]
    internal DateTime StartDateTime {
      get;
      private set;
    }


    [DataField("EndTime", Default = "ExecutionServer.DateMaxValue")]
    internal DateTime EndDateTime {
      get;
      private set;
    }


    public DateTime Date {
      get {
        return this.StartDateTime.Date;
      }
      private set {
        this.StartDateTime = value.Date.Add(this.StartDateTime.TimeOfDay);
      }
    }


    public string StartTime {
      get {
        return Time.ToString(this.StartDateTime, TimeType.HoursMinutes);
      }
      private set {
        try {
          this.StartDateTime = Time.SetTimeToDate(this.Date, value);
        } catch {
          throw new ValidationException("Meeting.StartTime",
                                        $"Meeting start time has a wrong value: '{value}'.");
        }
      }
    }


    public string EndTime {
      get {
        return Time.ToString(this.EndDateTime, TimeType.HoursMinutes);
      }
      private set {
        try {
          // End date is always equals to the start date, just the time varies.
          this.EndDateTime = Time.SetTimeToDate(this.Date, value);
        } catch {
          throw new ValidationException("Meeting.EndTime",
                                        $"Meeting end time has a wrong value: '{value}'.");
        }
      }
    }


    [DataField("Status", Default = OpenCloseStatus.Opened)]
    public OpenCloseStatus Status {
      get;
      private set;
    }


    public FixedList<Contact> Participants {
      get {
        var list = new Contact[2];

        list[0] = Contact.Parse(8);
        list[1] = Contact.Parse(10);

        return new FixedList<Contact>(list);
      }
    }

    //public FixedList<Topic> Topics {
    //  get {
    //    return new FixedList<Topic>();
    //  }
    //}


    //public FixedList<Recommendation> Recommendations {
    //  get {
    //    return new FixedList<Topic>();
    //  }
    //}


    //public FixedList<Agreement> Agreements {
    //  get {
    //    return new FixedList<Agreement>();
    //  }
    //}

    #endregion Public properties

    #region Public methods

    public void Close() {
      Assertion.Assert(this.Status == OpenCloseStatus.Opened,
                       $"Meeting can't be closed because is not opened. Current status: {this.Status}");

      this.Status = OpenCloseStatus.Closed;

    }

    public void Delete() {
      Assertion.Assert(this.Status == OpenCloseStatus.Opened,
                       $"Meeting can't be deleted because is not opened. Current status: {this.Status}");

      this.Status = OpenCloseStatus.Deleted;
    }


    public void Open() {
      Assertion.Assert(this.Status == OpenCloseStatus.Closed,
                       $"Meeting can't be opened because is not closed. Current status: {this.Status}");

      this.Status = OpenCloseStatus.Opened;
    }

    protected override void OnBeforeSave() {
      if (this.IsNew) {
        this.UID = EmpiriaString.BuildRandomString(6, 36);

        this.ControlNo = MeetingData.GetNextMeetingControlNo(this.Project);
      }
    }


    protected override void OnSave() {
      MeetingData.WriteMeeting(this);
    }


    public void Update(JsonObject data) {
      Assertion.Assert(this.Status == OpenCloseStatus.Opened,
                $"I can't update the meeting because is not opened. Current status: {this.Status}");

      this.AssertIsValid(data);

      this.Load(data);
    }

    #endregion Public methods

    #region Private methods


    protected void AssertIsValid(JsonObject data) {
      Assertion.AssertObject(data, "data");

    }


    protected void Load(JsonObject data) {
      this.Project = data.Get<Project>("projectUID", this.Project);

      this.Title = data.GetClean("title", this.Title);
      this.Description = data.GetClean("description", this.Description);
      this.Tags = data.GetClean("tags", this.Tags);

      this.Date = data.Get("date", this.Date);
      this.StartTime = data.Get("startTime", this.StartTime);
      this.EndTime = data.Get("endTime", this.EndTime);

      this.Location = data.GetClean("location", this.Location);
    }

    #endregion Private methods

  }  // class Meeting

}  // namespace Empiria.ProjectManagement.Meetings
