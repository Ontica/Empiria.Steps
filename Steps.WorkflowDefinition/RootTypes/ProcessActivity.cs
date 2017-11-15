/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Workflow Definition                 *
*  Assembly : Empiria.Steps.WorkflowDefinition.dll             Pattern : Domain class                        *
*  Type     : ProcessActivity                                  License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a workflow activity.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;

namespace Empiria.Steps.WorkflowDefinition {

  /// <summary>Describes a workflow model as an a activity network.</summary>
  public class ProcessActivity : WorkflowObject {

    #region Constructors and parsers

    protected ProcessActivity() : this(WorkflowObjectType.Process) {

    }

    protected ProcessActivity(WorkflowObjectType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static internal new ProcessActivity Parse(int id) {
      return BaseObject.ParseId<ProcessActivity>(id);
    }

    static public ProcessActivity Parse(string uid) {
      return BaseObject.ParseKey<ProcessActivity>(uid);
    }

    static public new ProcessActivity Empty {
      get {
        return BaseObject.ParseEmpty<ProcessActivity>();
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("TaskType")]
    public string TaskType {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    protected override void OnSave() {
      throw new NotImplementedException();
    }

    #endregion Public methods

  } // class ProcessActivity

} // namespace Empiria.Steps.WorkflowDefinition
