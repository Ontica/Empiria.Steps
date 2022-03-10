/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Office Integration                           Component : Integration Layer                     *
*  Assembly : Empiria.Steps.dll                            Pattern   : Static services                       *
*  Type     : ExcelProcessExporter                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Exports processes data to Excel files.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.IO;

using Empiria.Office.Providers;
using Empiria.Storage;

using Empiria.ProjectManagement;

namespace Empiria.Steps.OfficeIntegration {

  /// <summary>Exports processes data to Excel files.</summary>
  internal class ExcelProcessExporter {

    #region Constructors and parsers

    internal ExcelProcessExporter(Project process, ProjectItem branch) {
      Assertion.AssertObject(process, "process");
      Assertion.AssertObject(branch, "branch");

      this.Process = process;
      this.Branch = branch;
    }


    #endregion Constructors and parsers

    #region Properties

    internal Project Process {
      get;
    }


    internal ProjectItem Branch {
      get;
    }


    #endregion Properties

    #region Methods

    internal string Export() {
      var mediaTemplateUID = ConfigurationData.GetString("Export.Processes.To.Excel.MediaTemplate.UID");

      FileInfo newFile = MediaFileTemplateServices.CreateFileFromTemplate(mediaTemplateUID);

      FillOut(newFile);

      return MediaFileTemplateServices.GetFileUrl(newFile);
    }


    private void FillOut(FileInfo file) {
      var excel = OpenXMLSpreadsheet.Open(file.FullName);

      FixedList<ProjectItem> activities = GetActivitiesToExport();

      int i = 4;

      foreach (var activity in activities) {
        ActivityModel template = ((Activity) activity).Template;

        excel.SetCell($"A{i}", activity.Name);
        excel.SetCell($"B{i}", activity.Notes);
        excel.SetCell($"C{i}", template.LegalBasis);
        excel.SetCell($"D{i}", activity.ForeignLanguageData.Name);
        excel.SetCell($"E{i}", activity.ForeignLanguageData.Notes);
        excel.SetCell($"F{i}", activity.ForeignLanguageData.LegalBasis);
        excel.SetCell($"G{i}", activity.Theme);
        excel.SetCell($"H{i}", template.ActivityType);
        excel.SetCell($"I{i}", template.ExecutionMode);
        excel.SetCell($"J{i}", activity.Level);
        excel.SetCell($"K{i}", activity.Parent.Name);
        excel.SetCell($"L{i}", activity.UID);
        excel.SetCell($"M{i}", activity.Id);

        i++;
      }

      excel.SetCell($"A1", this.Process.Name);

      excel.Save();

      excel.Close();
     }


    private FixedList<ProjectItem> GetActivitiesToExport() {
      if (Branch.IsEmptyInstance) {
        return Process.GetItems();
      } else {
        return Process.GetBranch(this.Branch);
      }
    }

    #endregion Methods

  }  // class ExcelProcessExporter

}  // namespace Empiria.Steps.OfficeIntegration
