/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Procedure                                        License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Provides services that gets process definition models.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using Empiria.Json;

namespace Empiria.Steps.Modeling {

  public class Procedure : BaseObject {

    #region Constructors and parsers

    private Procedure() {
      // Required by Empiria Framework.
    }

    public Procedure(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Load(data);
    }

    static public Procedure Parse(string uid) {
      return BaseObject.ParseKey<Procedure>(uid);
    }

    static public FixedList<Procedure> GetList(string filter = "") {
      return ProcedureData.GetProcedureList(filter);
    }

    #endregion Constructors and parsers

    #region Public properties

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

    [DataField("ShortName")]
    public string ShortName {
      get;
      private set;
    }

    [DataField("Code")]
    public string Code {
      get;
      private set;
    }

    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }

    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Code, this.Name, this.Theme, this.Stage,
                                           this.Authority.Entity.Keywords,
                                           this.LegalInfo.LegalBasis);
      }
    }

    [DataField("URL")]
    public string URL {
      get;
      private set;
    }


    [DataField("Stage")]
    public string Stage {
      get;
      private set;
    }


    [DataField("Category")]
    public string Category {
      get;
      private set;
    }


    [DataField("Theme")]
    public string Theme {
      get;
      private set;
    }


    [DataObject]
    public Authority Authority {
      get;
      private set;
    } = Authority.Empty;


    [DataObject]
    public LegalInfo LegalInfo {
      get;
      private set;
    } = LegalInfo.Empty;


    [DataObject]
    public FilingCondition FilingCondition {
      get;
      private set;
    } = FilingCondition.Empty;


    [DataObject]
    public FilingDocuments FilingDocuments {
      get;
      private set;
    } = FilingDocuments.Empty;


    [DataObject]
    public FilingFee FilingFee {
      get;
      private set;
    } = FilingFee.Empty;


    [DataField("StatusNotes")]
    public string StatusNotes {
      get;
      private set;
    }


    [DataField("ProcedureStatus", Default = GeneralObjectStatus.Pending)]
    public GeneralObjectStatus Status {
      get;
      private set;
    }


    [DataField("MSExcelNo")]
    public int MSExcelNo {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    protected override void OnBeforeSave() {
      if (this.IsNew) {
        this.UID = EmpiriaString.BuildRandomString(32);
      }
    }

    protected override void OnSave() {
      ProcedureData.WriteProcedure(this);
    }

    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Load(data);
    }

    #endregion Public methods

    #region Private methods

    private void Load(JsonObject data) {
      this.Name = data.Get<string>("name", this.Name);
      this.Notes = data.Get<string>("notes", this.Notes);
      this.URL = data.Get<string>("url", this.URL);

      this.Stage = data.Get<string>("stage", this.Stage);
      this.Category = data.Get<string>("category", this.Category);
      this.Theme = data.Get<string>("theme", this.Theme);

      this.Authority = Authority.Parse(data.Slice("authority"));
      this.LegalInfo = LegalInfo.Parse(data.Slice("legalInfo"));
      this.FilingCondition = FilingCondition.Parse(data.Slice("filingCondition"));
      this.FilingDocuments = FilingDocuments.Parse(data.Slice("filingDocuments"));
      this.FilingFee = FilingFee.Parse(data.Slice("filingFee"));

    }

    #endregion Private methods

  }  // class Procedure

}  // namespace Empiria.Steps.Modeling
