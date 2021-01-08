/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Definition                           Component : Interface adapters                      *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Command payload                         *
*  Type     : SearchStepsCommand                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Command payload used for steps definition searching.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.Steps.Definition.Adapters {

  /// <summary>Command payload used for steps definition searching.</summary>
  public class SearchStepsCommand {

    public string Keywords {
      get; set;
    } = string.Empty;


    public string OrderBy {
      get; set;
    } = string.Empty;


    public int PageSize {
      get; set;
    } = 100;


    public int Page {
      get; set;
    } = 1;

  }  // class SearchStepsCommand

}  // namespace Empiria.Steps.Definition.Adapters
