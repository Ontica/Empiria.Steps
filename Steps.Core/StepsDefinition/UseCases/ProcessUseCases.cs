/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Definition                           Component : Use cases Layer                         *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Use case interactor class               *
*  Type     : ProcessUseCases                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for process definition searching and retrieving.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Services;
using Empiria.Steps.Definition.Adapters;

namespace Empiria.Steps.Definition.UseCases {

  /// <summary>Use cases for process definition searching and retrieving.</summary>
  public class ProcessUseCases : UseCase {

    #region Constructors and parsers

    protected ProcessUseCases() {
      // no-op
    }

    static public ProcessUseCases UseCaseInteractor() {
      return ProcessUseCases.CreateInstance<ProcessUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public StepDto GetProcess(string processUID) {
      Assertion.AssertObject(processUID, "processUID");

      var process = Process.Parse(processUID);

      return StepMapper.Map(process);
    }


    public FixedList<StepShortModel> SearchProcesses(SearchStepsCommand searchCommand) {
      Assertion.AssertObject(searchCommand, "searchCommand");

      var list = Process.GetList(searchCommand);

      return StepMapper.MapToShortModel(list);
    }

    #endregion Use cases

  }  // class ProcessUseCases

}  // namespace Empiria.Steps.Definition.UseCases
