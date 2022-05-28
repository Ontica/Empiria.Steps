/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Service provider                      *
*  Type     : DataRetrievalServices                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides project management data retrieval services.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Provides project management data retrieval services.</summary>
  static public class DataRetrievalServices {


    static public FixedList<ProjectItem> GetWorkOnActivities(ActivityFilter filter,
                                                             ActivityOrderBy orderBy) {
      Assertion.Require(filter, "filter");

      var finder = new ProjectFinder(filter);

      FixedList<ProjectItem> activities = finder.GetWorkOnActivities(orderBy);

      return activities;
    }


  }  // class DataRetrievalServices

}  // namespace Empiria.ProjectManagement.Services
