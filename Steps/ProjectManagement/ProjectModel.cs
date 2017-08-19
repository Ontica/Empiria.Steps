/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : ProjectModel                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Slice of a workflow process that serves as an activity model to build a project instance.      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Json;

using Empiria.Steps.WorkflowDefinition;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Slice of a workflow process that serves as an activity
  /// model to build a project instance.</summary>
  public class ProjectModel {

    #region Fields

    private Lazy<FixedList<ProcessActivity>> stepsList = null;

    #endregion Fields

    #region Constructors and parsers

    private ProjectModel(Process baseProcess) {
      this.BaseProcess = baseProcess;
      stepsList = new Lazy<FixedList<ProcessActivity>>(() => ProjectModelData.GetSteps(this));
    }


    static public ProjectModel Parse(Process baseProcess) {
      Assertion.AssertObject(baseProcess, "baseProcess");

      return new ProjectModel(baseProcess);
    }

    static public FixedList<ProjectModel> GetActivitiesModelsList() {
      var ownerOrManager = Contact.Parse(51);

      return ProjectModelData.GetActivitiesModels(ownerOrManager);
    }


    static public FixedList<ProjectModel> GetEventsModelsList() {
      var ownerOrManager = Contact.Parse(51);

      return ProjectModelData.GetEventsModels(ownerOrManager);
    }

    #endregion Constructors and parsers

    #region Public properties

    public Process BaseProcess {
      get;
    }

    public FixedList<ProcessActivity> Steps {
      get {
        return stepsList.Value;
      }
    }

    #endregion Public properties

    #region Methods

    public Project CreateInstance(Project baseProject, JsonObject data) {
      return baseProject;
    }

    #endregion Methods

  } // class ProjectModel

} // namespace Empiria.Steps.ProjectManagement
