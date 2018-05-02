/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : ProjectModelResponseModels                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for project models.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.ProjectManagement.Modeling.WebApi {

  /// <summary>Response static methods for project models.</summary>
  static internal class ProjectModelResponseModels {

    #region Responses

    static internal ICollection ToResponse(this IList<ProjectModel> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var model in list) {
        var process = model.BaseProcess;

        var item = new {
          uid = process.UID,
          type = process.WorkflowObjectType.Name,
          name = process.Name,
          notes = process.Notes,
          ownerUID = process.Owner.UID,
          //                         resourceTypeId = process.ResourceType.Id,
          links = process.Links,
          steps = new string[0]     //model.Steps.ToResponse()
        };
        array.Add(item);
      }
      return array;
    }


    #endregion Responses

  }  // class ProjectModelResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
