/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Integration Types                       *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Service provider                        *
*  Type     : Converter                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Converts models from Steps version 2.0 to version 3.0.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.DataTypes.Time;
using Empiria.Json;

using Empiria.ProjectManagement;

namespace Empiria.Steps.Design.Integration {

  /// <summary>Converts models from Steps version 2.0 to version 3.0.</summary>
  public class Converter {

    #region Fields

    private readonly Project _project;

    #endregion Fields

    #region Constructors and parsers

    public Converter(Project project) {
      _project = project;
    }

    #endregion Constructors and parsers

    #region Public methods

    static public FixedList<Process> Convert(Project project) {
      var converter = new Converter(project);

      var projectRootItems = converter.GetProjectRootItems();
      List<Process> rootProcesses = new List<Process>();

      foreach (var projectRoot in projectRootItems) {
        Process process = converter.CreateProcessFromProjectItem(projectRoot);

        rootProcesses.Add(process);
      }

      return rootProcesses.ToFixedList();
    }


    #endregion Public methods

    #region Private methods

    private StepRelation CreateDependencyAsStepRelation(ProjectItem projectItem,
                                                    ActivityModel formerModel) {
      var data = new StepRelationDataHolder();

      data.Accessibility = "Private";
      data.DrivenMode = "Manual";
      data.DueOnRule = ConvertDueOnRule(formerModel);

      if (EmpiriaString.IsBoolean(formerModel.DueOnRuleAppliesForAllContracts) &&
          !EmpiriaString.ToBoolean(formerModel.DueOnRuleAppliesForAllContracts)) {
        data.ExecutionContext = ConvertExecutionContext(projectItem);
      }

      data.ExtensionData = ConvertRelationExtensionData(formerModel);

      data.FlowControl = "Sequence";
      data.RelationKind = "Dependency";
      data.SourceId = projectItem.Id;
      data.TargetId = formerModel.DueOnControllerId;
      data.WorkSequenceKind = "FinishToFinish";

      data.Save();

      return StepRelation.Parse(data.UID);
    }


    private Process CreateProcessFromProjectItem(ProjectItem baseProjectItem) {
      Process process = CreatePublicProcessContainer(baseProjectItem);
      FixedList<ProjectItem> branch = baseProjectItem.GetBranch();

      for (int i = 1; i < branch.Count; i++) {
        var projectItem = branch[i];

        var step = CreateProcessStep(process, projectItem, i - 1);

        // process.Add(step);
      }

      return process;
     }


    private Step CreateProcessStep(Process process, ProjectItem projectItem, int position) {
      StepDataHolder data = MapToStepDataHolder(GetStepType(projectItem),
                                                projectItem, ((Activity) projectItem).Template);
      data.BaseStepId = process.Id;
      data.ParentStepId = projectItem.Parent.Id;

      data.StepPosition = position;

      data.Save();

      return Step.Parse(data.UID);
    }


    private Process CreatePublicProcessContainer(ProjectItem projectItem) {
      StepDataHolder data = MapToStepDataHolder(StepType.Process, projectItem,
                                                ((Activity) projectItem).Template);
      data.Accessibility = "Public";

      data.Save();

      return Process.Parse(data.UID);
    }


    private StepType GetStepType(ProjectItem item) {
        var template = ((Activity) item).Template;

      switch (template.ActivityType) {

        case "Event":
          return StepType.Event;

        case "Section":
          return StepType.Section;

        case "Subprocess":
          return StepType.Process;

        case "Internal":
        case "External":
        case "":
          return StepType.Task;
        default:
          throw Assertion.AssertNoReachThisCode();
      }
    }


    private StepDataHolder MapToStepDataHolder(StepType stepType,
                                               ProjectItem projectItem,
                                               ActivityModel formerModel) {
      var data = new StepDataHolder();

      data.Accessibility = "Private";
      data.DrivenMode = "Manual";

      data.EstimatedDuration = ConvertEstimatedDuration(formerModel);

      data.ExecutionContext = ConvertExecutionContext(projectItem);

      data.ExtensionData = ConvertExtensionData(formerModel);

      data.IsOptional = !formerModel.IsMandatory;

      data.Notes = projectItem.Notes;

      data.OldProjectObjectId = projectItem.Id;

      if (formerModel.DueOnControllerId > 0 && EmpiriaString.IsInteger(formerModel.DueOnTerm)) {

        StepRelation relation = CreateDependencyAsStepRelation(projectItem, formerModel);

        var dueOn = Activity.Parse(formerModel.DueOnControllerId);
        data.DataModels = $"{formerModel.DueOnTerm} {formerModel.DueOnTermUnit} {formerModel.DueOnCondition} " +
                          $"{dueOn.Name} ({dueOn.Template.ActivityType}) [{formerModel.DueOnControllerId}]";
      }

      if (formerModel.IsPeriodic) {
        data.PeriodicityRule = formerModel.PeriodicRule.ToJson();
      }

      data.ProcedureEntityId = formerModel.EntityId;
      data.ProcedureId = formerModel.ProcedureId;

      data.StepKind = formerModel.ActivityType;

      data.StepName = projectItem.Name.Trim();
      data.StepPosition = 0;
      data.StepType = stepType;

      data.Tags = projectItem.Tag;
      data.Themes = projectItem.Theme;

      return data;
    }


    #endregion Private methods

    #region Utility methods

    private JsonObject ConvertDueOnRule(ActivityModel formerModel) {
      var json = new JsonObject();

      json.Add("term", formerModel.DueOnTerm);
      json.Add("termUnit", formerModel.DueOnTermUnit);
      json.Add("condition", formerModel.DueOnCondition);

      return json;
    }


    private Duration ConvertEstimatedDuration(ActivityModel formerModel) {
      if (formerModel.Duration.Length != 0 && EmpiriaString.IsInteger(formerModel.Duration) &&
          EmpiriaString.ToInteger(formerModel.Duration) > 0 &&
          formerModel.DurationUnit.Length != 0) {

        string unit = formerModel.DurationUnit == "Undefined" || formerModel.DurationUnit == "NA" ?
                      "BusinessDays" : formerModel.DurationUnit;

        return Duration.Parse($"{formerModel.Duration} {unit}");

      } else {
        return Duration.Empty;

      }
    }


    private string ConvertExecutionContext(ProjectItem projectItem) {
      return EmpiriaString.TrimAll(projectItem.Project.Tags.Items[0]);
    }


    private JsonObject ConvertExtensionData(ActivityModel formerModel) {
      var json = new JsonObject();

      if (formerModel.LegalBasis.Trim().Length != 0) {
        json.Add("LegalBasis", formerModel.LegalBasis.Trim());
      }

      return json;
    }


    private JsonObject ConvertRelationExtensionData(ActivityModel formerModel) {
      var json = new JsonObject();

      if (formerModel.ContractClause.Trim().Length != 0 &&
          formerModel.ContractClause != "No aplicable") {
        json.Add("ContractClause", formerModel.ContractClause.Trim());
      }

      return json;
    }


    private FixedList<ProjectItem> GetProjectRootItems() {
      return _project.GetItems().FindAll(x => x.Level == 1);
    }

    #endregion Utility methods

  }  // class Converter

}  // namespace Empiria.Steps.Design.Integration
