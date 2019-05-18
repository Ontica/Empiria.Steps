/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web services interface                *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ActivityFilesController                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API to retrieve and set project activities files.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Postings;
using Empiria.Postings.Media;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API to retrieve and set project activities files.</summary>
  public class ActivityFilesController : WebApiController {

    #region GET methods


    [HttpGet]
    [Route("v1/project-management/activities/{activityUID}/files")]
    public CollectionModel GetActivityFiles(string activityUID) {
      try {
        var activity = ProjectItem.Parse(activityUID);

        FixedList<MediaFile> list = Posting.GetList<MediaFile>(activity, "ProjectItem.MediaFile");

        return new CollectionModel(this.Request, list.ToResponse(),
                                  typeof(MediaFile).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion GET methods


    #region UPDATE methods


    [HttpPost]
    [Route("v1/project-management/activities/{activityUID}/files")]
    public SingleObjectModel UploadActivityFile(string activityUID) {
      try {
        var request = HttpContext.Current.Request;

        var activity = ProjectItem.Parse(activityUID);

        Posting posting = MediaPostingServices.CreateMediaPost(request, activity, "ProjectItem.MediaFile");

        MediaFile mediaFile = posting.GetPostedItem<MediaFile>();

        return new SingleObjectModel(this.Request, mediaFile.ToResponse(),
                                     typeof(MediaFile).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpDelete]
    [Route("v1/project-management/activities/{activityUID}/files/{activityFileUID}")]
    public NoDataModel RemoveActivityFile(string activityUID, string activityFileUID) {
      try {
        var activity = ProjectItem.Parse(activityUID);

        var posting = Posting.Parse(activityFileUID);

        Assertion.Assert(posting.NodeObjectUID == activity.UID,
            $"Activity {activity.Name} does not have the file {posting.PostedItemUID}.");

        posting.Delete();

        return new NoDataModel(this.Request);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/project-management/activities/{activityUID}/files/{activityFileUID}")]
    public SingleObjectModel UpdateActivityFile(string activityUID, string activityFileUID) {
      try {
        var request = HttpContext.Current.Request;

        var activity = ProjectItem.Parse(activityUID);

        var posting = Posting.Parse(activityFileUID);

        Assertion.Assert(posting.NodeObjectUID == activity.UID,
            $"Activity {activity.Name} does not have the file {posting.PostedItemUID}.");

        MediaPostingServices.UpdateMediaPost(request, posting);

        MediaFile mediaFile = posting.GetPostedItem<MediaFile>();

        return new SingleObjectModel(this.Request, mediaFile.ToResponse(),
                                     typeof(MediaFile).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion UPDATE methods

  }  // class ActivityFilesController

}  // namespace Empiria.ProjectManagement.WebApi
