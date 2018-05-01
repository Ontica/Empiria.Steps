/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : Summary                                          License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project activity.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Json;

using Empiria.ProjectManagement.Resources;

namespace Empiria.ProjectManagement {

  /// <summary>Describes a project activity.</summary>
  public class Summary : ProjectItem {

    #region Fields

    private Lazy<List<ProjectItem>> itemsList = null;

    #endregion Fields

    #region Constructors and parsers

    protected Summary() : this(ProjectItemType.SummaryType) {

    }


    protected Summary(ProjectItemType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected internal Summary(Project project, ProjectItem parent, JsonObject data) :
                               base(ProjectItemType.SummaryType, project, parent, data) {

      this.AssertIsValid(data);
      this.Load(data);
    }

    static public Activity Parse(string uid) {
      return BaseObject.ParseKey<Activity>(uid);
    }

    static internal new Activity Parse(int id) {
      return BaseObject.ParseId<Activity>(id);
    }

    static public new Activity Empty {
      get {
        return BaseObject.ParseEmpty<Activity>();
      }
    }

    protected override void OnInitialize() {
      itemsList = new Lazy<List<ProjectItem>>(() => ProjectData.GetAllActivities(this));
    }

    #endregion Constructors and parsers

    #region Public properties


    [DataField("ResourceId")]
    public Resource Resource {
      get;
      private set;
    } = Resource.Empty;


    [DataField("ResponsibleId")]
    public Contact Responsible {
      get;
      private set;
    }


    [DataField("RequestedTime")]
    public DateTime RequestedTime {
      get;
      private set;
    } = ExecutionServer.DateMinValue;


    public Contact RequestedBy {
      get;
      private set;
    } = Contact.Empty;


    #endregion Public properties

    #region Properties related to the activity structure


    public FixedList<ProjectItem> Subactivities {
      get {
        return itemsList.Value.ToFixedList();
      }
    }


    public new ProjectItem Parent {
      get {
        return base.Parent;
      }
    }

    public int Level {
      get {
        if (this.Parent.ProjectObjectType == ProjectItemType.SummaryType) {
          return ((Summary) this.Parent).Level + 1;
        } else {
          return 1;
        }
      }
    }


    #endregion Properties related to the activity structure

    #region Public methods

    protected override void AssertIsValid(JsonObject data) {
      base.AssertIsValid(data);
    }

    protected override void Load(JsonObject data) {
      base.Load(data);

      this.Resource = Resource.Parse(data.Get("resourceUID", "Empty"));
      this.Responsible = Contact.Parse(data.Get("responsibleUID", "Empty"));
      this.RequestedTime = data.Get<DateTime>("requestedTime", this.RequestedTime);
      this.RequestedBy = Contact.Parse(data.Get("requestedByUID", "Empty"));
    }


    protected override void OnSave() {
      ProjectData.WriteSummary(this);
    }

    #endregion Public methods

  } // class Summary

} // namespace Empiria.ProjectManagement
