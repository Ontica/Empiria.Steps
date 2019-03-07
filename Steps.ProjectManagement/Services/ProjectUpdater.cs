/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Service provider                      *
*  Type     : ProjectUpdater                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides project management activity's state update services.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Provides project management activity's state update services</summary>
  static public class ProjectUpdater {

    #region Services


    static public void Complete(ProjectItem projectItem, DateTime completedDate) {
      Assertion.AssertObject(projectItem, "projectItem");

      WhatIfResult result = ModelingServices.WhatIfCompleted(projectItem, completedDate);

      if (result.HasErrors) {
        throw result.GetException();
      }

      StoreChanges(result);
    }


    static public void Reactivate(ProjectItem projectItem) {
      Assertion.AssertObject(projectItem, "projectItem");

      WhatIfResult result = ModelingServices.WhatIfReactivated(projectItem);

      if (result.HasErrors) {
        throw result.GetException();
      }

      StoreChanges(result);
    }


    #endregion Services


    #region Private methods


    static private void StoreChanges(WhatIfResult result) {
      foreach (var change in result.StateChanges) {
        switch (change.Operation) {
          case ProjectItemOperation.Complete:
            change.ProjectItem.Complete(change.ActualEndDate);
            break;
          case ProjectItemOperation.Reactivate:
            change.ProjectItem.Reactivate();
            break;
        }
      }  // foreach
    }


    #endregion Private methods

  }  // class ProjectUpdater

}  // namespace Empiria.ProjectManagement.Services
