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

using Empiria.Steps.Design.DataObjects;

namespace Empiria.Steps.Reporting {

  public class SasisopaTask : BaseObject {

    #region Constructors and parsers

    public SasisopaTask() {

    }

    static internal SasisopaTask Parse(int id) {
      return BaseObject.ParseId<SasisopaTask>(id);
    }


    static public SasisopaTask Parse(string uid) {
      return BaseObject.ParseKey<SasisopaTask>(uid);
    }


    //static public FixedList<SasisopaTask> GetList(StepDataObject dataObject) {
    //  return ReportingRepository.GetSasisopaTasksList(dataObject);
    //}


    #endregion Constructors and parsers

    #region Properties

    [DataField("Anexo")]
    public string Anexo {
      get;
      set;
    } = String.Empty;


    [DataField("Elemento")]
    public string Elemento {
      get;
      set;
    } = String.Empty;


    [DataField("Actividad")]
    public string Actividad {
      get;
      set;
    } = String.Empty;


    [DataField("Descripcion")]
    public string Descripcion {
      get;
      set;
    } = String.Empty;


    [DataField("FechasInicio")]
    public string FechasInicio {
      get;
      set;
    } = String.Empty;


    [DataField("FechasTermino")]
    public string FechasTermino {
      get;
      set;
    } = String.Empty;


    [DataField("Periodicidad")]
    public string Periodicidad {
      get;
      set;
    } = String.Empty;


    [DataField("Responsable")]
    public string Responsable {
      get;
      set;
    } = String.Empty;


    [DataField("Evidencias")]
    public string Evidencias {
      get;
      set;
    } = String.Empty;


    [DataField("DocumentosAnexos")]
    public string DocumentosAnexos {
      get;
      set;
    } = String.Empty;


    [DataField("Avance")]
    public string Avance {
      get;
      set;
    } = String.Empty;


    [DataField("Observaciones")]
    public string Observaciones {
      get;
      set;
    } = String.Empty;


    [DataField("SiguientesAcciones")]
    public string SiguientesAcciones {
      get;
      set;
    } = String.Empty;


    #endregion Properties

  }  // class SasisopaTask

} // namespace Empiria.Steps.Reporting
