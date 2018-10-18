/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : TemplatesController                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API to retrieve and set projects templates data.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API to retrieve and set projects data.</summary>
  public class TemplatesController : WebApiController {

    #region Get methods

    [HttpGet]
    [Route("v1/project-management/templates")]
    public CollectionModel GetTemplatesList() {
      try {
        var list = Project.GetTemplatesList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Project).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/templates/events")]
    public CollectionModel GetEventsList() {
      try {
        var list = Project.GetEventsList();

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Project).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Get methods

    #region Update methods

    [HttpPost]
    [Route("v1/project-management/projects/{projectUID}/create-from-event")]
    public SingleObjectModel CreateFromEvent(string projectUID,
                                            [FromBody] object body) {
      try {
        base.RequireBody(body);
        var bodyAsJson = JsonObject.Parse(body);

        var project = Project.Parse(projectUID);

        var model = bodyAsJson.Get<Activity>("eventUID");
        var templateProject = model.Project;

        var targetDate = bodyAsJson.Get<DateTime>("targetDate", DateTime.Today);

        var modelBranch = model.GetBranch();

        List<Activity> createdActivities = new List<Activity>();

        DateTime? dueDate = targetDate;

        foreach (var modelItem in modelBranch) {
          dueDate = CalculateNewDueDate(modelItem, targetDate);

          var json = new JsonObject();

          json.Add("name", modelItem.Name);
          json.Add("notes", modelItem.Notes);

          if (createdActivities.Count == 0) {
            json.Add("targetDate", targetDate);
          }

          if (dueDate.HasValue) {
            json.Add("dueDate", dueDate);
          } else if (createdActivities.Count == 0) {
            json.Add("dueDate", targetDate);
          }

          if (createdActivities.Count != 0) {
            json.Add("workflowObjectId", modelItem.Id);
          }

          var activity = project.AddActivity(json);

          if (createdActivities.Count != 0) {
            activity.SetAndSaveParent(createdActivities[0]);
          }

          createdActivities.Add(activity);
        }


        var dependencies = templateProject.GetItems()
                                          .FindAll( x => ((Activity) x).Template.DueOnControllerId == model.Id
                                                          && !createdActivities.Exists(y => y.WorkflowObjectId == x.Id));

        foreach (var dependency in dependencies) {
          dueDate = CalculateNewDueDate(dependency, targetDate);

          var json = new JsonObject();

          json.Add("name", dependency.Name);
          json.Add("notes", dependency.Notes);

          if (createdActivities.Count == 0) {
            json.Add("targetDate", targetDate);
          }

          if (dueDate.HasValue) {
            json.Add("dueDate", dueDate);
          } else if (createdActivities.Count == 0) {
            json.Add("dueDate", targetDate);
          }

          json.Add("workflowObjectId", dependency.Id);

          var activity = project.AddActivity(json);

          if (createdActivities.Count != 0) {
            activity.SetAndSaveParent(createdActivities[0]);
          }

          createdActivities.Add(activity);

        }

        return new SingleObjectModel(this.Request, model.ToResponse(),
                                     typeof(Activity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion Update methods

    private DateTime? CalculateNewDueDate(ProjectItem template, DateTime baseDate) {
      if (!(template is Activity)) {
        return null;
      }
      var dueOnTerm = ((Activity) template).Template.DueOnTerm;

      if (String.IsNullOrWhiteSpace(dueOnTerm)) {
        return null;
      }

      var term = int.Parse(dueOnTerm);

      switch (((Activity) template).Template.DueOnTermUnit) {
        case "BusinessDays":
          return baseDate.AddDays(Math.Round(term * 7.0 / 5.0, 0));

        case "CalendarDays":
          return baseDate.AddDays(term);

        case "Hours":
          return baseDate.AddHours(term);

        case "Months":
          return baseDate.AddMonths(term);

        case "Years":
          return baseDate.AddYears(term);

        default:
          return null;
      }


    }

  }  // class TemplatesController

}  // namespace Empiria.ProjectManagement.WebApi
