/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : ProjectModel                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Slice of a workflow process that serves as an activity model to build a project instance.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;

using Empiria.Workflow.Definition;

namespace Empiria.ProjectManagement {

  /// <summary>Slice of a workflow process that serves as an activity
  /// model to build a project instance.</summary>
  public class ProjectModel {

    #region Fields

    private Lazy<FixedList<ProcessActivity>> stepsList = null;

    #endregion Fields

    #region Constructors and parsers

    private ProjectModel(Process baseProcess) {
      this.BaseProcess = baseProcess;
      stepsList = new Lazy<FixedList<ProcessActivity>>(() => ProjectModelData.GetSteps(this));
    }


    static public ProjectModel Parse(Process baseProcess) {
      Assertion.AssertObject(baseProcess, "baseProcess");

      return new ProjectModel(baseProcess);
    }

    static public FixedList<ProjectModel> GetActivitiesModelsList() {
      var ownerOrManager = Contact.Parse(51);

      var list = ProjectModelData.GetActivitiesModels(ownerOrManager);

      list.Sort((x, y) => x.BaseProcess.Name.CompareTo(y.BaseProcess.Name));

      return list;
    }


    static public FixedList<ProjectModel> GetEventsModelsList() {
      var ownerOrManager = Contact.Parse(51);

      var list = ProjectModelData.GetEventsModels(ownerOrManager);

      list.Sort((x,y) => x.BaseProcess.Name.CompareTo(y.BaseProcess.Name));

      return list;
    }

    #endregion Constructors and parsers

    #region Public properties

    public Process BaseProcess {
      get;
    }

    public FixedList<ProcessActivity> Steps {
      get {
        return stepsList.Value;
      }
    }

    #endregion Public properties

    #region Methods

    public Project CreateInstance(Project baseProject, JsonObject data) {
      Assertion.AssertObject(baseProject, "baseProject");
      Assertion.AssertObject(data, "data");

      data.Add("workflowObjectId", BaseProcess.Id);

      ProjectItem baseItem = baseProject.AddActivity(data);

      baseItem.SetDates(DateTime.Today, DateTime.Today);

      DateTime startDate = DateTime.Today;

      int daysCount = 0;
      foreach (var step in this.Steps) {
        var baseSummary = (Summary) baseItem;
        var stepAsJson = this.ConvertStepToJson(baseSummary, step, daysCount);

        baseProject.AddActivity(stepAsJson);

        daysCount += step.EstimatedDuration.Value;
      }

      baseItem.SetDates(startDate, startDate.AddDays(daysCount));

      return baseProject;
    }


    private JsonObject ConvertStepToJson(Summary parent, ProcessActivity step,
                                         int daysAccumulator) {
      var json = new JsonObject();

      json.Add("name", step.Name);

      json.Add("startDate", parent.StartDate.AddDays(daysAccumulator));

      json.Add("dueDate", parent.StartDate.AddDays(step.EstimatedDuration.Value + daysAccumulator));

      json.Add("estimatedDuration", step.EstimatedDuration.ToString());

      json.Add("resourceUID", parent.Resource.UID);

      json.Add("requestedByUID", parent.RequestedBy.UID);
      json.Add("requestedTime", parent.RequestedTime);
      json.Add("responsibleUID", parent.Responsible.UID);

      json.Add("workflowObjectId", step.Id);
      json.Add("parentId", parent.Id);

      return json;
    }

    #endregion Methods

  } // class ProjectModel

} // namespace Empiria.ProjectManagement
