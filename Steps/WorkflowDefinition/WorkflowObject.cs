/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : WorkflowObject                                   License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Describes a project as a set of well defined activities.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.DataTypes;
using Empiria.Json;
using Empiria.Ontology;

using Empiria.Steps.Modeling;

namespace Empiria.Steps.WorkflowDefinition {

  /// <summary>Describes a project as a set of well defined activities.</summary>
  [PartitionedType(typeof(WorkflowObjectType))]
  public class WorkflowObject : BaseObject {

    #region Constructors and parsers

    protected WorkflowObject(WorkflowObjectType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    protected internal WorkflowObject(WorkflowObjectType type,
                                      WorkflowObject parent, JsonObject data) : base(type) {
      Assertion.AssertObject(parent, "parent");
      Assertion.AssertObject(data, "data");

      this.Parent = parent;
      this.AssertIsValid(data);

      this.Load(data);
    }

    static public WorkflowObject Parse(int id) {
      return BaseObject.ParseId<WorkflowObject>(id);
    }

    static public WorkflowObject Empty {
      get {
        return BaseObject.ParseEmpty<WorkflowObject>();
      }
    }

    protected override void OnLoadObjectData(System.Data.DataRow row) {
      if ((int) row["WorkflowObjectId"] != -1) {
        this.Parent = WorkflowObject.Parse((int) row["ParentId"]);
      } else {
        this.Parent = this;
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    public WorkflowObjectType WorkflowObjectType {
      get {
        return (WorkflowObjectType) base.GetEmpiriaType();
      }
    }


    [DataField("UID")]
    public string UID {
      get;
      private set;
    }


    [DataField("Name")]
    public string Name {
      get;
      private set;
    }


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }


    [DataField("InnerTag")]
    public string InnerTag {
      get;
      private set;
    }


    [DataField("Categories")]
    public string Categories {
      get;
      private set;
    }


    [DataField("ExtData")]
    protected internal JsonObject ExtensionData {
      get;
      private set;
    }


    [DataField("ProcedureId")]
    public Procedure Procedure {
      get;
      private set;
    } = Procedure.Empty;


    [DataField("FlowObjectID")]
    public string FlowObjectID {
      get;
      private set;
    }


    [DataField("OwnerId")]
    public Contact Owner {
      get;
      private set;
    }


    protected WorkflowObject Parent {
      get;
      private set;
    }


    [DataField("Status", Default = ObjectStatus.Active)]
    public ObjectStatus Status {
      get;
      private set;
    }

    public FixedList<object> Links {
      get {
        return this.ExtensionData.GetList<object>("links", false)
                                 .ToFixedList();
      }
    }

    #endregion Public properties

    #region Private methods

    protected virtual void AssertIsValid(JsonObject data) {
      Assertion.AssertObject(data, "data");

    }

    protected virtual void Load(JsonObject data) {
      this.Name = data.GetClean("name", this.Name);
      this.Notes = data.GetClean("notes", this.Notes);
    }

    protected override void OnBeforeSave() {
      if (this.IsNew) {
        this.UID = EmpiriaString.BuildRandomString(6, 24);
      }
    }

    protected override void OnSave() {
      throw Assertion.AssertNoReachThisCode();
    }

    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Load(data);
    }

    #endregion Private methods

  } // class WorkflowObject

} // namespace Empiria.Steps.WorkflowDefinition
