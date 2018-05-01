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

    #region Collection responses

    static internal ICollection ToResponse(this IList<Project> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var project in list) {
        var item = new {
          uid = project.UID,
          name = project.Name,
          notes = project.Notes,
          ownerUID = project.Owner.UID,
          managerUID = project.Manager.UID,
          status = project.Status
        };
        array.Add(item);
      }
      return array;
    }


    static internal ICollection ToResponse(this IList<ProjectItem> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        if (item is Activity) {
          array.Add(((Activity) item).ToResponse());

        } else if (item is Summary) {
          array.Add(((Summary) item).ToResponse());

        }
      }
      return array;
    }


    #endregion Collection responses

    #region Entities responses

    static internal object ToResponse(this Project project) {
      return new {
        uid = project.UID,
        name = project.Name,
        notes = project.Notes,
        ownerUID = project.Owner.UID,
        managerUID = project.Manager.UID,
        status = project.Status
      };
    }

    #endregion Entities responses

  }  // class ProjectResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
