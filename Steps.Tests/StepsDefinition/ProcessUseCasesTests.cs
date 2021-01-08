/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Definition                           Component : Test cases                              *
*  Assembly : Empiria.Steps.Tests.dll                    Pattern   : Use cases tests class                   *
*  Type     : ProcessUseCasesTests                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Test cases for process definition related use cases.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Xunit;

using Empiria.Steps.Definition.Adapters;
using Empiria.Steps.Definition.UseCases;

namespace Empiria.Steps.Tests.StepsDefinition {

  /// <summary>Test cases for process definition related use cases.</summary>
  public class ProcessUseCasesTests {

    #region Fields

    private readonly ProcessUseCases _usecases;

    #endregion Fields

    #region Initialization

    public ProcessUseCasesTests() {
      CommonMethods.Authenticate();

      _usecases = ProcessUseCases.UseCaseInteractor();
    }


    ~ProcessUseCasesTests() {
      _usecases.Dispose();
    }


    #endregion Initialization

    #region Facts

    [Fact]
    public void Should_Get_A_Process() {
      StepDto process = _usecases.GetProcess(TestingConstants.PROCESS_UID);

      Assert.Equal(TestingConstants.PROCESS_UID, process.UID);
    }


    [Fact]
    public void Should_Search_Processes() {
      var searchCommand = new SearchStepsCommand {
        PageSize = 100,
      };

      FixedList<StepShortModel> list = _usecases.SearchProcesses(searchCommand);

      Assert.NotEmpty(list);

      int moreGeneralListItemsCount = list.Count;

      searchCommand.Keywords = "cnh hidrocarburos";

      list = _usecases.SearchProcesses(searchCommand);

      Assert.True(list.Count <= moreGeneralListItemsCount,
                  "Search processes by keyword must return the same or fewer items.");
    }

    #endregion Facts

  }  // class ProcessUseCasesTests

}  // namespace Empiria.Steps.Tests.StepsDefinition
