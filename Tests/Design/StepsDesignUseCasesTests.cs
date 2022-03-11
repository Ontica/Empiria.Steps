/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design                               Component : Test cases                              *
*  Assembly : Empiria.Steps.Tests.dll                    Pattern   : Use cases tests class                   *
*  Type     : StepsDesignUseCasesTests                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for steps design related use cases.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Xunit;

using Empiria.Steps.Design.Adapters;
using Empiria.Steps.Design.UseCases;

namespace Empiria.Steps.Tests {

  /// <summary>Test cases for steps design related use cases.</summary>
  public class StepsDesignUseCasesTests {

    #region Fields

    private readonly StepsDesignUseCases _usecases;

    #endregion Fields

    #region Initialization

    public StepsDesignUseCasesTests() {
      CommonMethods.Authenticate();

      _usecases = StepsDesignUseCases.UseCaseInteractor();
    }


    ~StepsDesignUseCasesTests() {
      _usecases.Dispose();
    }


    #endregion Initialization

    #region Facts

    [Fact]
    public void Should_Get_A_Step() {
      StepDto process = _usecases.GetStep(TestingConstants.STEP_UID);

      Assert.Equal(TestingConstants.STEP_UID, process.UID);
    }


    [Fact]
    public void Should_Search_Steps() {
      var searchCommand = new SearchStepsCommand {
        PageSize = 100,
      };

      FixedList<StepDescriptorDto> list = _usecases.SearchSteps(searchCommand);

      Assert.NotEmpty(list);

      int moreGeneralListItemsCount = list.Count;

      searchCommand.Keywords = "cnh hidrocarburos";

      list = _usecases.SearchSteps(searchCommand);

      Assert.True(list.Count <= moreGeneralListItemsCount,
                  "Search by keyword must return the same or fewer steps.");
    }

    #endregion Facts

  }  // class StepsDesignUseCasesTests

}  // namespace Empiria.Steps.Tests
