/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Knowledge Services                   Component : Data Integration                        *
*  Assembly : Empiria.Steps.Design.dll                   Pattern   : Data Services                           *
*  Type     : ReportingRepository                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data repository with methods used to read and write steps data objects.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Empiria.Data;
using Empiria.Json;
using Empiria.Steps.Design.DataObjects;

namespace Empiria.Steps.Reporting {

  /// <summary>Data repository with methods used to read and write steps data objects.</summary>
  static internal class ReportingRepository {

    #region Methods

    static internal FixedList<JsonObject> GetCNHActividad() {
      return GetJsonsFromPostings("CNH.Actividad");
    }


    static internal FixedList<JsonObject> GetCNIHFluido() {
      return GetJsonsFromPostings("CNIH.Fluido");
    }


    static internal FixedList<JsonObject> GetCNIHOperacionDiaria() {
      return GetJsonsFromPostings("CNIH.OperacionDiaria");
    }


    static internal FixedList<JsonObject> GetSasisopaTasksList(StepDataObject dataObject) {
      return GetJsonsFromPostings("Sasisopa.Task");
    }


    static internal FixedList<SubtaskCNH> GetSubtaskCNH() {
      var sql = $"SELECT * FROM RPTSubtasks";

      var op = DataOperation.Parse(sql);

      return DataReader.GetFixedList<SubtaskCNH>(op);
    }


    static private FixedList<JsonObject> GetJsonsFromPostings(string postingType) {
      var sql = $"SELECT ExtData FROM EXFPostings " +
                $"WHERE PostingType = '{postingType}' AND PostingStatus <> 'X'";

      var op = DataOperation.Parse(sql);

      var strList = DataReader.GetFieldValues<string>(op, "ExtData");

      List<JsonObject> jsons = new List<JsonObject>(strList.Count);

      foreach (var item in strList) {
        var json = JsonObject.Parse(item);
        jsons.Add(json);
      }

      return jsons.ToFixedList();
    }


    #endregion Methods

  }  // class ReportingRepository

}  // namespace Empiria.Steps.Knowledge
