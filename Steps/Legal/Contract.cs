/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Legal Domain                  *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Contract                                         License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Characterizes a contract (legal document) with a set of clauses and annexes.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Json;
using Empiria.Ontology;

namespace Empiria.Steps.Legal {

  /// <summary>Characterizes a contract (legal document) with a set of clauses and annexes.</summary>
  [PartitionedType(typeof(LegalDocumentType))]
  public class Contract : BaseObject {

    #region Fields

    private Lazy<List<Clause>> clausesList = null;

    #endregion Fields

    #region Constructors and parsers

    protected Contract(LegalDocumentType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static public Contract Parse(string uid) {
      return BaseObject.ParseKey<Contract>(uid);
    }


    static public FixedList<Contract> GetList(LegalDocumentType powertype) {
      string filter = $"ObjectTypeId = {powertype.Id}";

      return Contract.GetList<Contract>(filter)
                     .ToFixedList();
    }


    protected override void OnInitialize() {
      clausesList = new Lazy<List<Clause>>(() => Clause.GetList(this));
    }

    #endregion Constructors and parsers

    #region Public properties

    public LegalDocumentType LegalDocumentType {
      get {
        return (LegalDocumentType) base.GetEmpiriaType();
      }
    }


    [DataField("ObjectKey")]
    public string UID {
      get;
      private set;
    }


    [DataField("ObjectName")]
    public string Name {
      get;
      private set;
    }


    [DataField("ObjectExtData.url")]
    public string Url {
      get;
      private set;
    }


    public FixedList<Clause> Clauses {
      get {
        return clausesList.Value.ToFixedList();
      }
    }

    #endregion Public properties

    #region Public methods

    public Clause AddClause(JsonObject data) {
      var clause = new Clause(this, data);

      clause.Save();

      clausesList.Value.Add(clause);

      return clause;
    }


    public Clause GetClause(string clauseUID) {
      Clause item = clausesList.Value.Find((x) => x.UID == clauseUID);

      Assertion.AssertObject(item, $"A clause with uid = '{clauseUID}' " +
                                   $"was not found in contract with uid = '{this.UID}'");

      return item;
    }

    #endregion Public methods

  }  // class Contract

}  // namespace Empiria.Steps.Legal
