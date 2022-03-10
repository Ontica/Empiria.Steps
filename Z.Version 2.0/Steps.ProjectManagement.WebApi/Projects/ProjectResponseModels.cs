/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : ProjectModels                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for project entities.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for project entities.</summary>
  static internal class ProjectResponseModels {

    #region Response methods


    static internal ICollection ToResponse(this IList<Project> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var project in list) {
        var itemResponse = project.ToResponse();

        array.Add(itemResponse);
      }

      return array;
    }


    static internal object ToResponse(this Project project) {
      return new {
        uid = project.UID,
        name = project.Name,
        notes = project.Notes,
        status = project.Status
      };
    }


    #endregion Response methods


  }  // class ProjectResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
