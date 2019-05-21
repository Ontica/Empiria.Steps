/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Web services interface                *
*  Assembly : Empiria.ProjectManagement.WebApi.dll         Pattern   : Controller                            *
*  Type     : ProjectItemsFilesController                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Public API to retrieve and set project items files.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web;
using System.Web.Http;

using Empiria.Data;
using Empiria.WebApi;

using Empiria.Postings;
using Empiria.Postings.Media;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API to retrieve and set project items files.</summary>
  public class ProjectItemsFilesController : WebApiController {

    #region GET methods


    [HttpGet]
    [Route("v1/project-management/files")]
    public CollectionModel GetAllProjectFiles() {
      try {
        FixedList<Posting> list = PostingList.GetPostings("ProjectItem.MediaFile");

        return new CollectionModel(this.Request, list.ToProjectItemFileResponse(),
                                  typeof(MediaFile).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/projects/{projectUID}/files")]
    public CollectionModel GetProjectFiles(string projectUID) {
      try {
        var project = Project.Parse(projectUID);

        FixedList<Posting> list = GetProjectFilesPostingsList(project);

        return new CollectionModel(this.Request, list.ToProjectItemFileResponse(),
                                  typeof(MediaFile).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/project-items/{projectItemUID}/files")]
    public CollectionModel GetProjectItemFiles(string projectItemUID) {
      try {
        var projectItem = ProjectItem.Parse(projectItemUID);

        FixedList<MediaFile> list = PostingList.GetPostedItems<MediaFile>(projectItem, "ProjectItem.MediaFile");

        return new CollectionModel(this.Request, list.ToResponse(),
                                  typeof(MediaFile).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion GET methods


    #region UPDATE methods


    [HttpPost]
    [Route("v1/project-management/project-items/{projectItemUID}/files")]
    public SingleObjectModel UploadProjectItemFile(string projectItemUID) {
      try {
        var request = HttpContext.Current.Request;

        var projectItem = ProjectItem.Parse(projectItemUID);

        Posting posting = MediaPostingServices.CreateMediaPost(request, projectItem, "ProjectItem.MediaFile");

        MediaFile mediaFile = posting.GetPostedItem<MediaFile>();

        return new SingleObjectModel(this.Request, mediaFile.ToResponse(),
                                     typeof(MediaFile).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpDelete]
    [Route("v1/project-management/project-items/{projectItemUID}/files/{fileUID}")]
    public NoDataModel RemoveProjectItemFile(string projectItemUID, string fileUID) {
      try {
        var projectItem = ProjectItem.Parse(projectItemUID);

        var posting = Posting.Parse(fileUID);

        Assertion.Assert(posting.NodeObjectUID == projectItem.UID,
            $"ProjectItem {projectItem.Name} does not have the file {posting.PostedItemUID}.");

        posting.Delete();

        return new NoDataModel(this.Request);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/project-management/project-items/{projectItemUID}/files/{fileUID}")]
    public SingleObjectModel UpdateProjectItemFile(string projectItemUID, string fileUID) {
      try {
        var request = HttpContext.Current.Request;

        var projectItem = ProjectItem.Parse(projectItemUID);

        var posting = Posting.Parse(fileUID);

        Assertion.Assert(posting.NodeObjectUID == projectItem.UID,
            $"ProjectItem {projectItem.Name} does not have the file {posting.PostedItemUID}.");

        MediaPostingServices.UpdateMediaPost(request, posting);

        MediaFile mediaFile = posting.GetPostedItem<MediaFile>();

        return new SingleObjectModel(this.Request, mediaFile.ToResponse(),
                                     typeof(MediaFile).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion UPDATE methods


    #region Private methods

    static private FixedList<Posting> GetProjectFilesPostingsList(Project project) {
      var op = DataOperation.Parse("@qryPMProjectFilesPostings", project.Id, "ProjectItem.MediaFile");

      return DataReader.GetFixedList<Posting>(op);
    }

    #endregion Private methods

  }  // class ProjectItemsFilesController

}  // namespace Empiria.ProjectManagement.WebApi
