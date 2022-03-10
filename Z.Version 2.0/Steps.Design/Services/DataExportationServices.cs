/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Data Exportation                             Component : Service Layer                         *
*  Assembly : Empiria.Steps.dll                            Pattern   : Static services                       *
*  Type     : ProjectExporterServices                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Services that allow export data to Office files.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.ProjectManagement;
using Empiria.Steps.OfficeIntegration;

namespace Empiria.Steps.Services {

  /// <summary>Services that allow export data to Office files.</summary>
  static public class DataExportationServices {


    static public string ExportProcessToExcel(string processUID, string branchUID) {
      Assertion.AssertObject(processUID, "processUID");

      var process = Project.Parse(processUID);

      ProjectItem branch = GetProjectItem(branchUID);

      var exporter = new ExcelProcessExporter(process, branch);

      return exporter.Export();
    }


    static public string ExportProjectToExcel(string projectUID, string branchUID) {
      Assertion.AssertObject(projectUID, "projectUID");

      var project = Project.Parse(projectUID);

      ProjectItem branch = GetProjectItem(branchUID);

      var exporter = new ExcelExporter(project, branch);

      return exporter.Export();
    }

    #region Utility methods

    static private ProjectItem GetProjectItem(string branchUID) {
      if (String.IsNullOrEmpty(branchUID)) {
        return ProjectItem.Empty;
      } else {
        return ProjectItem.Parse(branchUID);
      }
    }

    #endregion Utility methods

  }  // class ProjectExporterServices

}  // namespace Empiria.Steps.Services
