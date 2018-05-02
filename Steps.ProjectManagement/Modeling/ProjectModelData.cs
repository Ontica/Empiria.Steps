/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Data Service                        *
*  Type     : ProjectModelData                                 License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Data read and write methods for workflow objects.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Contacts;
using Empiria.Data;

using Empiria.Workflow.Definition;

namespace Empiria.ProjectManagement.Modeling {

  /// <summary>Data read and write methods for workflow objects.</summary>
  static internal class ProjectModelData {


    static internal FixedList<ProjectModel> GetActivitiesModels(Contact owner) {
      return ProjectModelData.GetProjectModels(owner, "ActivityModel");
    }


    static internal FixedList<ProjectModel> GetEventsModels(Contact owner) {
      return ProjectModelData.GetProjectModels(owner, "EventModel");
    }


    static internal FixedList<ProjectModel> GetProjectModels(Contact owner, string categories = "") {
      string sql = $"SELECT * FROM WFWorkflowObjects " +
                   $"WHERE WorkflowObjectTypeId = {WorkflowObjectType.Process.Id} AND " +
                   $"Categories LIKE '%{categories}%' AND Status <> 'X'";

      var op = DataOperation.Parse(sql);

      var baseProcessesList = DataReader.GetList(op, (x) => BaseObject.ParseList<Process>(x));

      var models = new List<ProjectModel>(baseProcessesList.Count);

      foreach (var baseProcess in baseProcessesList) {
        var projectModel = ProjectModel.Parse(baseProcess);

        models.Add(projectModel);
      }
      return models.ToFixedList();
    }


    static internal FixedList<ProcessActivity> GetSteps(ProjectModel projectModel) {
      string sql = $"SELECT * FROM WFWorkflowObjects " +
                   $"WHERE ParentId = {projectModel.BaseProcess.Id} AND " +
                   $"WorkflowObjectTypeId = {WorkflowObjectType.Activity.Id} AND " +
                   $"Status <> 'X'";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<ProcessActivity>(x))
                       .ToFixedList();
    }

  }  // class ProjectModelData

}  // namespace Empiria.ProjectManagement
