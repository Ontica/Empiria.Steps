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


    static public WhatIfResult WhatIfCreatedFromEvent(Activity activityModel,
                                                      Project project,
                                                      DateTime eventDate) {
      Assertion.AssertObject(activityModel, "activityModel");
      Assertion.AssertObject(project, "project");

      var handler = new ActivityCreator(project);

      return handler.CreateFromEvent(activityModel, eventDate);
    }


    static public WhatIfResult WhatIfCompleted(ProjectItem projectItem, DateTime completedDate) {
      Assertion.AssertObject(projectItem, "projectItem");

      var updater = new ActivityUpdater();

      return updater.OnComplete(projectItem, completedDate);

      //var whatIfResult = new WhatIfResult(projectItem, ProjectItemOperation.Complete);

      //var stateChange = new ProjectItemStateChange(projectItem, ProjectItemOperation.Complete);

      //stateChange.ActualEndDate = completedDate;

      //whatIfResult.AddStateChange(stateChange);

      ////if (projectItem.HasTemplate) {
      ////  FixedList<ProjectItem> associatedItems = GetAssociatedProjectItems(projectItem);
      ////  FixedList<ProjectItemStateChange> stateChanges = GetWhatIfClosedStateChanges(projectItem, associatedItems, completedDate);
      ////  whatIfResult.AddStateChanges(stateChanges);
      ////}

      //return whatIfResult;
    }


    //static private FixedList<ProjectItem> GetAssociatedProjectItems(ProjectItem projectItem) {
    //  if (!projectItem.HasTemplate) {
    //    return new FixedList<ProjectItem>();
    //  }
    //  ProjectItem template = projectItem.GetTemplate();

    //  FixedList<ProjectItem> templateUpdateContext = GetUpdateContext(template);
    //  FixedList<ProjectItem> projectItemUpdateContext = GetUpdateContext(projectItem);


    //}


    static private FixedList<ProjectItemStateChange> GetWhatIfClosedStateChanges(ProjectItem projectItem,
                                                                                 FixedList<ProjectItem> associatedItems,
                                                                                 DateTime completedDate) {
      throw new NotImplementedException();
    }


    //static private void CreateBranchFromTemplate(Activity activityModel) {
    //  var modelBranch = activityModel.GetBranch();

    //  foreach (var modelItem in modelBranch) {

    //    var stateChange = new ProjectItemStateChange(modelItem, ProjectItemOperation.CreateFromTemplate);

    //    stateChange.Project = this.targetProject;

    //    if (this.whatIfResult.StateChanges.Count != 0) {
    //      var parent = this.whatIfResult.StateChanges.Find(x => x.Template.Id == modelItem.Parent.Id);

    //      if (parent != null) {
    //        stateChange.Parent = parent;
    //      } else {
    //        stateChange.Parent = this.whatIfResult.StateChanges[0];
    //      }
    //    }  // if
    //    this.whatIfResult.AddStateChange(stateChange);

    //  }  // foreach

    //}

    static public WhatIfResult WhatIfReactivated(ProjectItem projectItem) {
      Assertion.AssertObject(projectItem, "projectItem");

      var whatIfResult = new WhatIfResult(projectItem, ProjectItemOperation.Reactivate);

      var stateChange = new ProjectItemStateChange(projectItem, ProjectItemOperation.Reactivate);

      whatIfResult.AddStateChange(stateChange);

      return whatIfResult;
    }

    #endregion Services

  }  // class ModelingServices

}  // namespace Empiria.ProjectManagement.Services