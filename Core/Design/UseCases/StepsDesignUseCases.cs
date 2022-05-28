/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design                               Component : Use cases Layer                         *
*  Assembly : Empiria.Steps.Core.dll                     Pattern   : Use case interactor class               *
*  Type     : StepsDesignUseCases                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases for steps designs.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Services;
using Empiria.Steps.Design.Adapters;

namespace Empiria.Steps.Design.UseCases {

  /// <summary>Use cases for process definition searching and retrieving.</summary>
  public class StepsDesignUseCases : UseCase {

    #region Constructors and parsers

    protected StepsDesignUseCases() {
      // no-op
    }

    static public StepsDesignUseCases UseCaseInteractor() {
      return StepsDesignUseCases.CreateInstance<StepsDesignUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public StepDto GetStep(string stepUID) {
      Assertion.Require(stepUID, "stepUID");

      var step = Step.Parse(stepUID);

      return StepMapper.Map(step);
    }


    public FixedList<StepDescriptorDto> SearchSteps(SearchStepsCommand searchCommand) {
      Assertion.Require(searchCommand, "searchCommand");

      var list = Step.GetList(searchCommand);

      return StepMapper.MapToShortModel(list);
    }

    #endregion Use cases

  }  // class StepsDesignUseCases

}  // namespace Empiria.Steps.Design.UseCases
