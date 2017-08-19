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
using System.Collections.Generic;

using Empiria.Contacts;

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


    internal static ProjectModel Parse(Process baseProcess) {
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

    #endregion Public properties

    #region Project structure

    public FixedList<ProcessActivity> Steps {
      get {
        return stepsList.Value;
      }
    }

    #endregion Project structure

  } // class ProjectModel

} // namespace Empiria.Steps.ProjectManagement
