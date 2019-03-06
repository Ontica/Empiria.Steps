/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Service provider                      *
*  Type     : ModelingServices                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides project management services using project templating rules.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Provides project management services using project templating rules.</summary>
  static public class ModelingServices {


    static public FixedList<ProjectItem> CreateActivitiesFromModel(Activity activityModel,
                                                                   Project project,
                                                                   DateTime eventDate) {
      Assertion.AssertObject(activityModel, "activityModel");
      Assertion.AssertObject(project, "project");

      var handler = new ActivityCreator(project);

      handler.CreateFromEvent(activityModel, eventDate);

      return project.GetItems();
    }


  }  // class ModelingServices

}  // namespace Empiria.ProjectManagement.Services
