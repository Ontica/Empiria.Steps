/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Service provider                      *
*  Type     : ModelingServices                             License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides project management services using project templating rules.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Provides project management services using project templating rules.</summary>
  static public class ModelingServices {

    #region Services


    public static ProjectProcess GetProcess(Project project, string processUniqueID) {
      Assertion.AssertObject(project, "project");
      Assertion.AssertObject(processUniqueID, "processUniqueID");

      ProjectProcess process = ProcessesCheckList(project).Find(x => x.UniqueID == processUniqueID);

      return process;
    }


    static public FixedList<ProjectProcess> ProcessesCheckList(Project project) {
      Assertion.AssertObject(project, "project");

      FixedList<ProjectProcess> list = ProjectProcess.GetList(project);

      list.Sort((x, y) => x.StartActivity.Position.CompareTo(y.StartActivity.Position));

      return list;
    }


    static public WhatIfResult TryGetNextPeriodic(ProjectItem projectItem, DateTime completedDate) {
      if (projectItem.TemplateId == 0) {
        return null;
      }

      var template = projectItem.GetTemplate().Template;

      if (!template.IsPeriodic) {
        return null;
      }

      var activityCreator = new ActivityCreator(projectItem.Project);

      DateTime? newPeriod = UtilityMethods.CalculateNextPeriodicDate(template, projectItem.Deadline);

      if (!newPeriod.HasValue) {
        return null;
      }

      return activityCreator.CreateFromEvent(projectItem.GetTemplate(), newPeriod.Value);
    }


    static public WhatIfResult WhatIfCreatedFromEvent(Activity activityModel,
                                                      Project project,
                                                      DateTime eventDate) {
      Assertion.AssertObject(activityModel, "activityModel");
      Assertion.AssertObject(project, "project");

      var handler = new ActivityCreator(project);

      return handler.CreateFromEvent(activityModel, eventDate);
    }


    static public WhatIfResult WhatIfCompleted(ProjectItem projectItem, DateTime completedDate, bool addNewPeriodics) {
      Assertion.AssertObject(projectItem, "projectItem");

      var updater = new ActivityUpdater();

      return updater.OnComplete(projectItem, completedDate, addNewPeriodics);
    }


    static public WhatIfResult WhatIfReactivated(ProjectItem projectItem) {
      Assertion.AssertObject(projectItem, "projectItem");

      var whatIfResult = new WhatIfResult(projectItem, ProjectItemOperation.Reactivate);

      var stateChange = new ProjectItemStateChange(projectItem, ProjectItemOperation.Reactivate);

      whatIfResult.AddStateChange(stateChange);

      return whatIfResult;
    }


    static public WhatIfResult WhatIfUpdatedWithLastProcessChanges(ProjectItem projectItem) {
      Assertion.AssertObject(projectItem, "projectItem");

      var updater = new ProcessUpdater(projectItem);

      return updater.OnUpdateProcess();
    }


    #endregion Services

  }  // class ModelingServices

}  // namespace Empiria.ProjectManagement.Services
