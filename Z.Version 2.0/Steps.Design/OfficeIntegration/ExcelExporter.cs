/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Office Integration                           Component : Integration Layer                     *
*  Assembly : Empiria.Steps.dll                            Pattern   : Static services                       *
*  Type     : ExcelExporter                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Services that allow export activites data to Office files.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.IO;

using Empiria.Office.Providers;
using Empiria.Storage;

using Empiria.ProjectManagement;

namespace Empiria.Steps.OfficeIntegration {

  internal class ExcelExporter {

    #region Constructors and parsers

    internal ExcelExporter(Project project, ProjectItem branch) {
      Assertion.Require(project, "project");
      Assertion.Require(branch, "branch");

      this.Project = project;
      this.Branch = branch;
    }


    #endregion Constructors and parsers

    #region Properties

    internal Project Project {
      get;
    }


    internal ProjectItem Branch {
      get;
    }


    #endregion Properties

    #region Methods

    internal string Export() {
      var mediaTemplateUID = ConfigurationData.GetString("Export.Activities.To.Excel.MediaTemplate.UID");

      FileInfo newFile = MediaFileTemplateServices.CreateFileFromTemplate(mediaTemplateUID);

      FillOut(newFile);

      return MediaFileTemplateServices.GetFileUrl(newFile);
    }


    private void FillOut(FileInfo file) {
      var excel = OpenXMLSpreadsheet.Open(file.FullName);

      FixedList<ProjectItem> activities = GetActivitiesToExport();

      int i = 4;

      foreach (var activity in activities) {
        ActivityModel template = activity.HasTemplate ?
                                          activity.GetTemplate().Template : ActivityModel.Empty;

        excel.SetCell($"A{i}", activity.Name);
        excel.SetCell($"B{i}", activity.Notes);
        excel.SetCell($"C{i}", template.LegalBasis);
        excel.SetCell($"D{i}", activity.ForeignLanguageData.Name);
        excel.SetCell($"E{i}", activity.ForeignLanguageData.Notes);
        excel.SetCell($"F{i}", activity.ForeignLanguageData.LegalBasis);
        excel.SetCell($"G{i}", activity.Theme);
        excel.SetCell($"H{i}", activity.Tag);
        excel.SetCell($"I{i}", activity.Status.ToString());
        excel.SetCell($"J{i}", GetAssignee(activity));
        excel.SetCell($"K{i}", ToString(activity.Deadline));
        excel.SetCell($"L{i}", ToString(activity.PlannedEndDate));
        excel.SetCell($"M{i}", ToString(activity.ActualEndDate));
        excel.SetCell($"N{i}", template.ActivityType);
        excel.SetCell($"O{i}", template.ExecutionMode);
        excel.SetCell($"P{i}", activity.UID);

        i++;
      }

      excel.SetCell($"A1", this.Project.Name);

      excel.Save();

      excel.Close();
     }


    private string GetAssignee(ProjectItem projectItem) {
      if (!(projectItem is Activity)) {
        return String.Empty;
      }

      Activity activity = (Activity) projectItem;

      if (!activity.Responsible.IsEmptyInstance) {
        return activity.Responsible.Alias;
      } else {
        return String.Empty;
      }
    }


    private FixedList<ProjectItem> GetActivitiesToExport() {
      if (Branch.IsEmptyInstance) {
        return Project.GetItems();
      } else {
        return Project.GetBranch(this.Branch);
      }
    }


    private string ToString(DateTime date) {
      if (date == ExecutionServer.DateMaxValue || date == ExecutionServer.DateMinValue) {
        return String.Empty;
      } else {
        return date.ToString("yyyy-MM-dd");
      }
    }

    #endregion Methods

  }  // class ExcelExporter

}  // namespace Empiria.Steps.OfficeIntegration
