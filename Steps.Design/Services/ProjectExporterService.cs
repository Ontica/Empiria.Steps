/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Data Exportation                             Component : Service Layer                         *
*  Assembly : Empiria.Steps.dll                            Pattern   : Static services                       *
*  Type     : ProjectExporterServices                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Services that allow export activites data to Office files.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.ProjectManagement;

using Empiria.Steps.OfficeIntegration;

namespace Empiria.Steps.Services {

  /// <summary>Services that allow export activites data to Office files.</summary>
  static public class ProjectExporterServices {

    static public string ExportToExcel(string projectUID, string branchUID = "") {
      Assertion.AssertObject(projectUID, "projectUID");

      var project = Project.Parse(projectUID);

      ProjectItem branch;

      if (branchUID.Length != 0) {
        branch = ProjectItem.Parse(branchUID);
      } else {
        branch = ProjectItem.Empty;
      }

      var exporter = new ExcelExporter(project, branch);

      string fileName = exporter.Export();

      return fileName;
    }

  }  // class ProjectExporterServices

}  // namespace Empiria.Steps.Services
