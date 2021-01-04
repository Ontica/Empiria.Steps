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

    public StepDto GetProcess(string processUID) {
      Assertion.AssertObject(processUID, "processUID");

      var process = Process.Parse(processUID);

      return StepMapper.Map(process);
    }

    public FixedList<StepListItemDto> SearchProcesses(SearchStepsCommand searchCommand) {
      throw new NotImplementedException();
    }

    #endregion Constructors and parsers

  }  // class ProcessUseCases

}  // namespace Empiria.Steps.Definition.UseCases
