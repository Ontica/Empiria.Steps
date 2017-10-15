/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Requirement                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Represents a procedure requirement.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Json;

namespace Empiria.Steps.Modeling {

  /// <summary>Represents a procedure requirement.</summary>
  public class Requirement : BaseObject {

    #region Constructors and parsers

    protected Requirement() {
      // Required by Empiria Framework.
    }


    static internal Requirement Parse(int id) {
      return BaseObject.ParseId<Requirement>(id);
    }


    static public Requirement Parse(string uid) {
      return BaseObject.ParseKey<Requirement>(uid);
    }


    static internal List<Requirement> GetList(Procedure procedure) {
      string filter = $"ProcedureId = {procedure.Id}";
      string orderBy = "RequirementType, ItemOrder, RequirementId";

      return BaseObject.GetList<Requirement>(filter, orderBy);
    }

    #endregion Constructors and parsers

    #region Public properties

    //[DataField("UID")]
    public string UID {
      get;
      private set;
    } = String.Empty;


    [DataField("ProcedureId")]
    internal int ProcedureId {
      get;
      private set;
    }


    [DataField("RequirementType")]
    public string RequirementType {
      get;
      private set;
    }


    [DataField("ItemOrder")]
    internal int ItemOrder {
      get;
      private set;
    }


    [DataField("RequirementName")]
    public string Name {
      get;
      private set;
    }


    [DataField("Observations")]
    public string Observations {
      get;
      private set;
    }


    [DataField("AppliesTo")]
    public string AppliesTo {
      get;
      private set;
    }


    [DataField("AdditionalConditions")]
    public string AdditionalConditions {
      get;
      private set;
    }


    [DataField("FillingCopies")]
    public string FillingCopies {
      get;
      private set;
    }


    [DataField("RequirementUrl")]
    public string SourceUrl {
      get;
      private set;
    }


    [DataField("FillingSampleUrl")]
    public string SampleUrl {
      get;
      private set;
    }


    public string InstructionsUrl {
      get;
      private set;
    } = String.Empty;


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }


    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Name);
      }
    }

    #endregion Public properties

    #region Public methods

    protected override void OnBeforeSave() {
      //if (this.UID.Length == 0) {
      //  this.UID = EmpiriaString.BuildRandomString(6, 24);
      //}
    }


    protected override void OnSave() {
      // ContractsData.WriteClause(this);
    }


    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.AssertIsValid(data);

      this.Load(data);

      this.Save();
    }

    #endregion Public methods

    #region Private methods

    private void AssertIsValid(JsonObject data) {
      Assertion.AssertObject(data, "data");

    }


    private void Load(JsonObject data) {

    }

    #endregion Private methods

  }  // class Requirement

}  // namespace Empiria.Steps.Legal
