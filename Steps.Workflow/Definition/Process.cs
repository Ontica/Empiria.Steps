/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Workflow Definition                 *
*  Assembly : Empiria.Workflow.dll                             Pattern : Domain class                        *
*  Type     : Process                                          License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a workflow model as an a activity network.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Workflow.Definition {

  /// <summary>Describes a workflow model as an a activity network.</summary>
  public class Process : WorkflowObject {

    #region Fields

    //private Lazy<List<ProcessActivity>> transitionsTable = null;

    #endregion Fields

    #region Constructors and parsers

    protected Process() : this(WorkflowObjectType.Process) {

    }

    protected Process(WorkflowObjectType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static internal new Process Parse(int id) {
      return BaseObject.ParseId<Process>(id);
    }

    static public Process Parse(string uid) {
      return BaseObject.ParseKey<Process>(uid);
    }

    static public new Process Empty {
      get {
        return BaseObject.ParseEmpty<Process>();
      }
    }

    static public FixedList<Process> GetList(string filter = "") {
      var list = WorkflowDefinitionData.GetProcesses(filter);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list.ToFixedList();
    }

    protected override void OnInitialize() {
      // transitionsTable = new Lazy<List<ProcessActivity>>(() => ProjectModelData.GetProcessActivitiesList(this));
    }

    #endregion Constructors and parsers

    #region Public properties


    public BpmnDiagram BpmnDiagram {
      get {
        return BpmnDiagram.Parse(base.FlowObjectID);
      }
    }


    #endregion Public properties

    #region Public methods

    protected override void OnSave() {
      WorkflowDefinitionData.WriteProcess(this);
    }

    #endregion Public methods

  } // class Process

} // namespace Empiria.Workflow.Definition
