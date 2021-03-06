﻿/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : ResourceResponses                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for resources.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.ProjectManagement.Resources.WebApi {

  /// <summary>Response static methods for resources models.</summary>
  static internal class ResourceResponseModels {

    static internal ICollection ToResponse(this IList<Resource> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var resource in list) {
        var item = new {
          uid = resource.UID,
          type = resource.ResourceType.Name,
          name = resource.Name,
          notes = resource.Notes,
        };
        array.Add(item);
      }
      return array;
    }

    //static internal ICollection ToResponse(this FixedList<TreeItem> treeItems) {
    //  ArrayList array = new ArrayList(treeItems.Count);

    //  foreach (var treeItem in treeItems) {
    //    object item;
    //    if (treeItem.Children.Count != 0) {
    //      item = ToTreeCompoundResponse(treeItem);
    //    } else {
    //      item = ToTreeLeafResponse(treeItem);
    //    }
    //    array.Add(item);
    //  }
    //  return array;
    //}

    //static internal object ToTreeCompoundResponse(this TreeItem treeItem) {
    //  Resource resource = treeItem.GetTarget<Resource>();

    //  return new {
    //    id = treeItem.Id,
    //    type = resource.ResourceType.Name,
    //    text = treeItem.Name,
    //    items = treeItem.Children.ToResponse()
    //  };
    //}

    //static internal object ToTreeLeafResponse(this TreeItem treeItem) {
    //  Resource resource = treeItem.GetTarget<Resource>();

    //  return new {
    //    id = resource.Id,
    //    type = resource.ResourceType.Name,
    //    text = resource.Name
    //  };
    //}

  }  // class ResourceResponses

}  // namespace Empiria.ProjectManagement.Resources.WebApi
