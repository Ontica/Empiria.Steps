/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                        Component : Web Api                               *
*  Assembly : Empiria.Steps.Design.WebApi.dll              Pattern   : Controller                            *
*  Type     : DataFormController                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web api for data form features.                                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using Empiria.ProjectManagement;
using Empiria.Steps.Design.DataObjects;

namespace Empiria.Steps.Reporting {

  public class SubtaskCNH : BaseObject {

    public SubtaskCNH() {

    }

    static internal SubtaskCNH Parse(int id) {
      return BaseObject.ParseId<SubtaskCNH>(id);
    }


    static public SubtaskCNH Parse(string uid) {
      return BaseObject.ParseKey<SubtaskCNH>(uid);
    }

    public static FixedList<SubtaskCNH> GetList() {
      return ReportingRepository.GetSubtaskCNH();
    }


    public static IDictionary<string, object> LoadFields(StepDataObject dataObject,
                                                         IDictionary<string, object> json,
                                                         ProjectItem activity) {
      var subtask = SubtaskCNH.Parse(Convert.ToInt32(json["subtarea"]));

      json.Add("subtaskCode", subtask.SubtaskCode);
      json.Add("subtaskName", subtask.SubtaskName);
      json.Add("subactivityName", subtask.SubactivityName);

      return json;
    }

    #region Properties


    [DataField("ActivityCode")]
    public string ActivityCode {
      get;
      set;
    } = String.Empty;


    [DataField("ActivityName")]
    public string ActivityName {
      get;
      set;
    } = String.Empty;


    [DataField("SubactivityCode")]
    public string SubactivityCode {
      get;
      set;
    } = String.Empty;


    [DataField("SubactivityName")]
    public string SubactivityName {
      get;
      set;
    } = String.Empty;


    [DataField("TaskCode")]
    public string TaskCode {
      get;
      set;
    } = String.Empty;


    [DataField("TaskName")]
    public string TaskName {
      get;
      set;
    } = String.Empty;

    [DataField("SubtaskCode")]
    public string SubtaskCode {
      get;
      set;
    } = String.Empty;


    [DataField("SubtaskName")]
    public string SubtaskName {
      get;
      set;
    } = String.Empty;


    [DataField("AccreditsWorkUnits")]
    public bool AccreditsWorkUnits {
      get;
      set;
    } = false;


    [DataField("UnitName")]
    public string UnitName {
      get;
      set;
    } = String.Empty;


    [DataField("WorkUnitsEquivalence")]
    public decimal WorkUnitsEquivalence {
      get;
      set;
    }


    [DataField("Jan2020")]
    public decimal Jan2020 {
      get;
      set;
    }

    [DataField("Feb2020")]
    public decimal Feb2020 {
      get;
      set;
    }

    [DataField("Mar2020")]
    public decimal Mar2020 {
      get;
      set;
    }

    [DataField("Apr2020")]
    public decimal Apr2020 {
      get;
      set;
    }

    [DataField("May2020")]
    public decimal May2020 {
      get;
      set;
    }


    [DataField("Jun2020")]
    public decimal Jun2020 {
      get;
      set;
    }


    [DataField("Jul2020")]
    public decimal Jul2020 {
      get;
      set;
    }


    [DataField("Aug2020")]
    public decimal Aug2020 {
      get;
      set;
    }

    [DataField("Sep2020")]
    public decimal Sep2020 {
      get;
      set;
    }

    [DataField("Oct2020")]
    public decimal Oct2020 {
      get;
      set;
    }

    [DataField("Nov2020")]
    public decimal Nov2020 {
      get;
      set;
    }

    [DataField("Dec2020")]
    public decimal Dec2020 {
      get;
      set;
    }

    [DataField("Annual2020")]
    public decimal Annual2020 {
      get;
      set;
    }

    [DataField("Annual2021")]
    public decimal Annual2021 {
      get;
      set;
    }

    [DataField("Annual2022")]
    public decimal Annual2022 {
      get;
      set;
    }

    [DataField("Annual2023")]
    public decimal Annual2023 {
      get;
      set;
    }

    #endregion Properties


  }  // class SubtaskCNH

}  // namespace Empiria.Steps.Reporting
