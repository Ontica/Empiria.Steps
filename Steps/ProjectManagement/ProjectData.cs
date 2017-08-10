/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Data Service                        *
*  Type     : ProjectData                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Project's data read and write methods.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Data;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Project's data read and write methods.</summary>
  static internal class ProjectData {

    static internal List<ProjectItem> GetProjectActivities(Project project) {
      string sql = $"SELECT * FROM BPMProjectItems " +
                   $"WHERE ProjectId = {project.Id} AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<ProjectItem>(op, (x) => BaseObject.ParseList<ProjectItem>(x));
    }

  }  // class ProjectData

}  // namespace Empiria.Steps.ProjectManagement
