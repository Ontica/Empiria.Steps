using System;
using System.IO;

using Empiria.Json;

using Empiria.Office.Providers;

using Empiria.Steps.Reporting;


namespace Empiria.Steps.Design.DataObjects {

  ///<summary>WARNING: Last minute demo technical debt isolated methods.</summary>
  static internal class AutofillExecution {

    #region Public methods

    //static internal void ExecuteAutofill(this Autofill autofill,
    //                                 string fullPath, bool uploaded) {
    //  if (autofill.StepDataObject.MediaFormat == "Excel") {
    //    autofill.ExecuteExcelAutofill(fullPath, uploaded);
    //  } else if (autofill.StepDataObject.MediaFormat == "PDF") {
    //    autofill.ExecutePDFAutofill(fullPath, uploaded);
    //  } else {
    //    Assertion.AssertNoReachThisCode($"Unrecognized autofill handler for media format {autofill.StepDataObject.MediaFormat}.");
    //  }
    //}


    //static internal void ExecutePDFAutofill2(this Autofill autofill,
    //                                       string templatePath, string fullPath) {
    //  try {
    //    var defaults = autofill.StepDataObject.DataItem.GetPDFFormFields();

    //    var pdfFields = PdfFieldsReader.GetFields(templatePath);

    //    foreach (var field in pdfFields) {
    //      if (defaults.Contains(field.Key)) {
    //        field.Value = defaults.Get<string>(field.Key);
    //      }
    //    }

    //    Stream stream = File.OpenRead(templatePath);

    //    var autofilled = PdfFieldsWriter.WriteFields(stream, pdfFields);

    //    byte[] bytes = autofilled.ToArray();

    //    stream.Close();

    //    using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write)) {
    //      fs.Write(bytes, 0, (int) bytes.Length);
    //    }

    //  } catch (Exception e) {
    //    throw e;
    //  }
    //}

    #endregion Public methods


    #region Private methods

    static private void ExecuteExcelAutofill(this Autofill autofill, string fullPath, bool uploaded) {
      if (uploaded) {
        return;
      }

      if (autofill.StepDataObject.DataItem.Label == "Reporte.Semestral.SASISOPA.Excel") {
        autofill.FillOutSasisopa(fullPath);
        return;
      }

      if (autofill.StepDataObject.DataItem.Label == "Reporte.Mensual.Actividades.CNH.Excel") {
        autofill.FillOutMensualActividadesCNH(fullPath);
        return;
      }

      if (autofill.StepDataObject.DataItem.Label == "Reporte.Mensual.Costos.CNH.Excel") {
        autofill.FillOutCostosCNH(fullPath);
        return;
      }

      if (autofill.StepDataObject.DataItem.Label == "Reporte.Bitacora.Operaciones.Excel") {
        autofill.FillOutBitacoraOperaciones(fullPath);
        return;
      }

      if (autofill.StepDataObject.DataItem.Label == "Reporte.Muestras.Fluidos.Excel") {
        autofill.FillOutMuestraFluidos(fullPath);
        return;
      }

      if (autofill.StepDataObject.DataItem.Label == "Extraer.Datos.Archivo.SIPAC") {
        autofill.GetDataArchivoSIPAC(fullPath);
        return;
      }
    }


    //static private void ExecutePDFAutofill(this Autofill autofill,
    //                                            string fullPath, bool uploaded) {
    //  var defaults = autofill.StepDataObject.DataItem.GetPDFFormFields();

    //  try {
    //    var pdfFields = PdfFieldsReader.GetFields(fullPath);

    //    JsonObject json = new JsonObject();

    //    foreach (var field in pdfFields) {
    //      if (uploaded) {
    //        json.AddIfValue(field.Key, field.Value);
    //      } else {
    //        json.Add(field.Key, field.Value);
    //      }
    //    }

    //    autofill.AutofillFields = json;
    //    autofill.Save();

    //  } catch (Exception e) {
    //    throw e;
    //  }
    //}


    static private void FillOutMensualActividadesCNH(this Autofill autofill,
                                                     string fullPath) {
      try {
        var excel = OpenXMLSpreadsheet.Open(fullPath);

        var tasks = ReportingRepository.GetCNHActividad();

        int i = 12;

        foreach (var task in tasks) {
          excel.SetCell($"T{i}", "Vista Oil & Gas Holding II, S.A. de C.V.");
          excel.SetCell($"U{i}", "Vernet-1001EXP");
          excel.SetCell($"V{i}", "CNH-R02-L03-CS-01/2017");
          //excel.SetCell($"M{i}", random.Next(0, 12));
          //excel.SetCell($"N{i}", random.Next(5, 10));
          //excel.SetCell($"O{i}", random.Next(1, 10));
          //excel.SetCell($"P{i}", random.Next(4, 9));
          //excel.SetCell($"Q{i}", random.Next(0, 50));
          //excel.SetCell($"R{i}", random.Next(0, 30));
          //excel.SetCell($"S{i}", random.Next(10, 50));
          //excel.SetCell($"T{i}", random.Next(20, 40));
          //excel.SetCell($"U{i}", random.Next(5, 40));
          //excel.SetCell($"V{i}", random.Next(0, 50));
        }

        excel.Save();

        excel.Close();

      } catch (Exception e) {
        throw e;
      }
    }


    static private void FillOutSasisopa(this Autofill autofill,
                                        string fullPath) {
      try {
        var excel = OpenXMLSpreadsheet.Open(fullPath);

        var tasks = ReportingRepository.GetSasisopaTasksList(autofill.StepDataObject);

        int i = 4;

        foreach (var task in tasks) {
          excel.SetCell($"A{i}", task.Get<string>("anexo", String.Empty));
          excel.SetCell($"B{i}", task.Get<string>("elemento", String.Empty));
          excel.SetCell($"C{i}", task.Get<string>("actividad", String.Empty));
          excel.SetCell($"D{i}", task.Get<string>("descripcion", String.Empty));
          excel.SetCell($"E{i}", task.Get<string>("fechasInicio", String.Empty));
          excel.SetCell($"F{i}", task.Get<string>("fechasTermino", String.Empty));
          excel.SetCell($"G{i}", task.Get<string>("periodicidad", String.Empty));
          excel.SetCell($"H{i}", task.Get<string>("responsable", String.Empty));
          excel.SetCell($"I{i}", task.Get<string>("evidencias", String.Empty));
          excel.SetCell($"J{i}", task.Get<string>("documentosAnexos", String.Empty));
          excel.SetCell($"K{i}", task.Get<string>("avance", String.Empty));
          excel.SetCell($"L{i}", task.Get<string>("observaciones", String.Empty));
          excel.SetCell($"M{i}", task.Get<string>("siguientesAcciones", String.Empty));

          i++;
        }

        excel.Save();

        excel.Close();

      } catch (Exception e) {
        throw e;
      }
    }


    static private void FillOutBitacoraOperaciones(this Autofill autofill,
                                                   string fullPath) {
      try {
        var excel = OpenXMLSpreadsheet.Open(fullPath);

        var tasks = ReportingRepository.GetCNIHOperacionDiaria();

        int i = 5;

        foreach (var task in tasks) {
          excel.SetCell($"A{i}", "Vista Oil & Gas Holding II, S.A. de C.V.");
          excel.SetCell($"B{i}", "Vernet-1001EXP");
          excel.SetCell($"C{i}", "590480190000200");
          excel.SetCell($"D{i}", "CNH-R02-L03-CS-01/2017");
          excel.SetCell($"E{i}", task.Get<string>("diasEnOperacion", String.Empty).ToUpper());
          excel.SetCell($"F{i}", task.Get<string>("proximoMovimiento", String.Empty).ToUpper());
          excel.SetCell($"G{i}", task.Get<string>("tipoTerminacion", String.Empty).ToUpper());
          excel.SetCell($"H{i}", task.Get<string>("registroHidrocarburos", String.Empty).ToUpper());
          excel.SetCell($"I{i}", task.Get<string>("fechaReporte", String.Empty).ToUpper());
          excel.SetCell($"J{i}", task.Get<string>("tiempoDias", String.Empty).ToUpper());
          excel.SetCell($"K{i}", task.Get<string>("metrosDesarrollados", String.Empty).ToUpper());
          excel.SetCell($"L{i}", task.Get<string>("metrosVerticales", String.Empty).ToUpper());
          excel.SetCell($"M{i}", task.Get<string>("pesoAplicadoBarrena", String.Empty).ToUpper());
          excel.SetCell($"N{i}", task.Get<string>("velocidadMR", String.Empty).ToUpper());
          excel.SetCell($"O{i}", task.Get<string>("presionBomba", String.Empty).ToUpper());
          excel.SetCell($"P{i}", task.Get<string>("torque", String.Empty).ToUpper());
          excel.SetCell($"Q{i}", task.Get<string>("pesoSarta", String.Empty).ToUpper());

         i++;
        }

        excel.Save();

        excel.Close();

      } catch (Exception e) {
        throw e;
      }
    }


    static private void FillOutCostosCNH(this Autofill autofill,
                                         string fullPath) {

    }


    static private void FillOutMuestraFluidos(this Autofill autofill,
                                              string fullPath) {
      try {
        var excel = OpenXMLSpreadsheet.Open(fullPath);

        var tasks = ReportingRepository.GetCNIHFluido();

        int i = 4;

        foreach (var task in tasks) {
          excel.SetCell($"A{i}", "Vista Oil & Gas Holding II, S.A. de C.V.");
          excel.SetCell($"B{i}", "Vernet-1001EXP");
          excel.SetCell($"C{i}", "590480190000200");
          excel.SetCell($"D{i}", "CNH-R02-L03-CS-01/2017");
          excel.SetCell($"E{i}", task.Get<string>("archivoLaboratorio", String.Empty).ToUpper());
          excel.SetCell($"F{i}", task.Get<string>("tipoMuestras", String.Empty).ToUpper());
          excel.SetCell($"G{i}", task.Get<string>("fechaMuestreo", String.Empty).ToUpper());
          excel.SetCell($"H{i}", task.Get<string>("fechaEntrega", String.Empty).ToUpper());
          excel.SetCell($"I{i}", task.Get<string>("caja", String.Empty).ToUpper());
          excel.SetCell($"J{i}", task.Get<string>("intervaloProbado", String.Empty).ToUpper());
          excel.SetCell($"K{i}", task.Get<string>("cantidadMuestra", String.Empty).ToUpper());
          excel.SetCell($"P{i}", task.Get<string>("condicionesTomaMuestra", String.Empty).ToUpper());
          excel.SetCell($"R{i}", task.Get<string>("observaciones", String.Empty).ToUpper());

          i++;
        }

        excel.Save();

        excel.Close();

      } catch (Exception e) {
        throw e;
      }
    }


    static private void GetDataArchivoSIPAC(this Autofill autofill,
                                            string fullPath) {
    }

    #endregion Private methods

  }  // class TechnicalDebtMethods

}  // namespace Empiria.Steps.Design
