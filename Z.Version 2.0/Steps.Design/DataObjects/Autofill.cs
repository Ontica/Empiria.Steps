using System;
using System.IO;
using System.IO.Compression;
using System.Web;

using Empiria.Json;
using Empiria.Postings;
using Empiria.Storage;

using Empiria.ProjectManagement;

namespace Empiria.Steps.Design.DataObjects {

  ///<summary>WARNING: Last minute demo technical debt isolated methods.</summary>
  public class Autofill {

    public Autofill(StepDataObject stepDataObject,
                    ProjectItem activity) {
      this.StepDataObject = stepDataObject;
      this.Activity = activity;
      this.Posting = this.LoadOrCreatePosting();
    }


    #region Properties

    public StepDataObject StepDataObject {
      get;
    }


    public ProjectItem Activity {
      get;
    }


    public Posting Posting {
      get;
    }


    public string AutofillFileUrl {
      get {
        return this.Posting.ExtensionData.Get<string>("autofillFileUrl", String.Empty);
      }
      private set {
        this.Posting.ExtensionData.Set("autofillFileUrl", value);
      }
    }


    public JsonObject AutofillFields {
      get {
        return this.Posting.ExtensionData.Slice("autofillFields", false);
      }
      internal set {
        this.Posting.ExtensionData.Set("autofillFields", value);
      }
    }


    public string UploadedFileUrl {
      get {
        return this.Posting.ExtensionData.Get<string>("uploadedFileUrl", String.Empty);
      }
      private set {
        this.Posting.ExtensionData.Set("uploadedFileUrl", value);
      }
    }


    #endregion Properties


    #region Methods

    private void GenerateZipPozos() {
      MediaStorage storage = MediaStorage.Default;

      var zipFileName = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss--") + "carpetas.pozos.cnih.zip";
      string fullZipPath = Path.Combine(storage.Path, zipFileName);

      var sourceZip = this.StepDataObject.DataItem.TemplateFileInfo;

      ZipFile.CreateFromDirectory(sourceZip.FullName, fullZipPath);

      //using (var zip = ZipFile.Open(sourceZip.FullName, ZipArchiveMode.Update)) {

      //}


      this.AutofillFileUrl = $"{MediaStorage.Default.Url}/{fullZipPath}";

      this.Save();

      return;
    }


    public void GenerateMediaFile() {
      if (this.StepDataObject.DataItem.Terms == "Generar.Zip.Folder.Pozos.Muestras.Fluidos") {
        GenerateZipPozos();
        return;
      }

      return;

      //MediaStorage storage = MediaStorage.Default;

      //var template = this.StepDataObject.DataItem.TemplateFileInfo;

      //var fileName = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss--") + template.Name;

      //string fullPath = Path.Combine(storage.Path, fileName);

      //if (this.StepDataObject.MediaFormat == "PDF" && this.StepDataObject.DataItem.GetPDFFormFields().HasItems) {
      //  this.ExecutePDFAutofill2(template.FullName, fullPath);

      //  this.AutofillFileUrl = $"{MediaStorage.Default.Url}/{fileName}";

      //  this.Save();

      //  return;
      //}

      //File.Copy(template.FullName, fullPath, true);

      //this.AutofillFileUrl = $"{MediaStorage.Default.Url}/{fileName}";

      //this.ExecuteAutofill(fullPath, false);

      //this.Save();
    }


    public void RemoveFile() {
      this.UploadedFileUrl = String.Empty;

      this.Save();
    }


    public void RemoveMediaFile() {
      this.AutofillFileUrl = String.Empty;

      this.Save();
    }


    public void UploadFile(HttpPostedFile file) {
      MediaStorage storage = MediaStorage.Default;

      var template = this.StepDataObject.DataItem.TemplateFileInfo;

      var uploadFileName = DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss") + "--Final--" + template.Name;

      string fullPath = Path.Combine(storage.Path, uploadFileName);

      file.SaveAs(fullPath);

      this.UploadedFileUrl = $"{MediaStorage.Default.Url}/{uploadFileName}";

      // this.ExecuteAutofill(fullPath, true);

      this.Save();
    }


    #endregion Public methods


    #region Private methods

    private Posting LoadOrCreatePosting() {
      const string postingType = "Steps.Autofill.FileData";

      var currentPostings = PostingList.GetPostings(this.StepDataObject,
                                                    this.Activity, postingType);

      if (currentPostings.Count == 1) {
        return currentPostings[0];
      }

      if (currentPostings.Count > 1) {
        throw Assertion.EnsureNoReachThisCode("Multiple file postings per activity are not yet implemented.");
      }


      var posting = new Posting(postingType, this.StepDataObject, this.Activity);

      posting.Save();

      return posting;
    }


    internal void Save() {
      this.Posting.Save();
    }


    #endregion Private methods

  }  // class TechnicalDebtMethods

}  // namespace Empiria.Steps.Design
