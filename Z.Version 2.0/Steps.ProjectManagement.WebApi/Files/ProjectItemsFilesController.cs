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
using Empiria.Storage;

namespace Empiria.ProjectManagement.WebApi {

  /// <summary>Public API to retrieve and set project items files.</summary>
  public class ProjectItemsFilesController : WebApiController {

    #region GET methods


    [HttpGet]
    [Route("v1/project-management/files")]
    public CollectionModel GetAllProjectFiles() {
      try {
        FixedList<Posting> list = PostingList.GetPostings("ProjectItem.MediaFile");

        var projects = UserProjectSecurity.GetUserProjectFixedList();

        list = list.FindAll(x => projects.Exists(y => y.Id == x.GetNodeObjectItem<ProjectItem>().Project.Id));

        return new CollectionModel(this.Request, list.ToProjectItemFileResponse(),
                                  typeof(FormerMediaFile).FullName);

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
                                  typeof(FormerMediaFile).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/project-management/project-items/{projectItemUID}/files")]
    public CollectionModel GetProjectItemFiles([FromUri] string projectItemUID) {
      try {
        var projectItem = ProjectItem.Parse(projectItemUID);

        FixedList<FormerMediaFile> list = PostingList.GetPostedItems<FormerMediaFile>(projectItem, "ProjectItem.MediaFile");

        return new CollectionModel(this.Request, list.ToResponse(),
                                  typeof(FormerMediaFile).FullName);

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

        Posting posting = MediaFilePostingServices.CreateMediaFilePosting(request, projectItem, "ProjectItem.MediaFile");

        FormerMediaFile mediaFile = posting.GetPostedItem<FormerMediaFile>();

        return new SingleObjectModel(this.Request, mediaFile.ToResponse(),
                                     typeof(FormerMediaFile).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpDelete]
    [Route("v1/project-management/project-items/{projectItemUID}/files/{fileUID}")]
    public NoDataModel RemoveProjectItemFile(string projectItemUID, string fileUID) {
      try {
        var projectItem = ProjectItem.Parse(projectItemUID);

        FixedList<Posting> list = PostingList.GetPostings(projectItem, "ProjectItem.MediaFile");

        var posting = list.Find(x => x.PostedItemUID == fileUID);

        Assertion.Require(posting.NodeObjectUID == projectItem.UID,
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

        Assertion.Require(posting.NodeObjectUID == projectItem.UID,
            $"ProjectItem {projectItem.Name} does not have the file {posting.PostedItemUID}.");

        MediaFilePostingServices.UpdateMediaFilePosting(request, posting);

        FormerMediaFile mediaFile = posting.GetPostedItem<FormerMediaFile>();

        return new SingleObjectModel(this.Request, mediaFile.ToResponse(),
                                     typeof(FormerMediaFile).FullName);

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
