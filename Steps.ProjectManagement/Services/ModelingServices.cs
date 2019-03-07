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

      var whatIfResult = new WhatIfResult(projectItem, ProjectItemOperation.Complete);

      var stateChange = new ProjectItemStateChange(projectItem, ProjectItemOperation.Complete);

      stateChange.ActualEndDate = completedDate;

      whatIfResult.AddStateChange(stateChange);

      return whatIfResult;
    }


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