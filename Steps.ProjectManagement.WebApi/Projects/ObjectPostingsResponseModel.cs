/* Empiria Postings ******************************************************************************************
*                                                                                                            *
*  Module   : Empiria Postings                             Component : Web services interface                *
*  Assembly : Empiria.Postings.WebApi.dll                  Pattern   : Response methods                      *
*  Type     : ObjectPostingsResponseModel                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for object postings.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Postings;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Response static methods for object postings.</summary>
  static internal class ObjectPostingsResponseModel {

    static internal ICollection ToResponse(this IList<ObjectPosting> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var posting in list) {
        var item = posting.ToResponse();

        array.Add(item);
      }
      return array;
    }


    static internal object ToResponse(this ObjectPosting posting) {
      return new {
        uid = posting.UID,
        objectUID = posting.ObjectUID,
        body = posting.Body,
        title = posting.Title,
        fileUrl = posting.FileUrl,
        authors = posting.Authors,
        tags = posting.Tags,
        sendTo = posting.SendTo.ToObject(),
        date = posting.Date,
        accessMode = posting.AccessMode,
        owner = posting.Owner.Nickname,
        status = posting.Status
      };
    }

  }  // class ObjectPostingsResponseModel

}  // namespace Empiria.Postings.WebApi
