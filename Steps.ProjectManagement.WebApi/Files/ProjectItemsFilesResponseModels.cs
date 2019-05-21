/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web Api                               *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Response methods                      *
*  Type     : ProjectItemsFilesResponseModels              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for project items files.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Postings;
using Empiria.Postings.Media;


namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for project items files.</summary>
  static internal class ProjectItemsFilesResponseModels {


    #region Response methods


    static internal ICollection ToProjectItemFileResponse(this IList<Posting> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var item in list) {
        var itemResponse = item.ToProjectItemFileResponse();

        array.Add(itemResponse);
      }
      return array;
    }


    static internal object ToProjectItemFileResponse(this Posting posting) {
      ProjectItem projectItem = posting.GetNodeObjectItem<ProjectItem>();
      MediaFile mediaFile = posting.GetPostedItem<MediaFile>();

      return new {
        uid = posting.UID,
        postingTime = posting.PostingTime,
        postedBy = posting.PostedBy.ToShortResponse(),

        projectItem = projectItem.ToResponse(),

        mediaFile = mediaFile.ToResponse(),
      };
    }


    #endregion Response methods


  }  // class ProjectItemsFilesResponseModels

}  // namespace Empiria.ProjectManagement.WebApi
