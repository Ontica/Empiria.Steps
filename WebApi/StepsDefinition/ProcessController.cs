/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Definition                             Component : Web Api                               *
*  Assembly : Empiria.Steps.WebApi.dll                     Pattern   : Controller                            *
*  Type     : ProcessController                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web Api used to work with process definitions.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Steps.Definition.Adapters;
using Empiria.Steps.Definition.UseCases;

namespace Empiria.Steps.Definition.WebApi {

  /// <summary>Web Api used to work with process definitions.</summary>
  public class ProcessController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v3/steps/processes/{processUID:guid}")]
    public SingleObjectModel GetProcess([FromUri] string processUID) {

      using (var usecases = ProcessUseCases.UseCaseInteractor()) {
        StepDto processDto = usecases.GetProcess(processUID);

        return new SingleObjectModel(this.Request, processDto);
      }
    }


    [HttpGet]
    [Route("v3/steps/processes")]
    public CollectionModel SearchProcesses([FromUri] SearchStepsCommand searchCommand) {

      Assertion.AssertObject(searchCommand, "searchCommand");

      using (var usecases = ProcessUseCases.UseCaseInteractor()) {
        FixedList<StepShortModel> list = usecases.SearchProcesses(searchCommand);

        return new CollectionModel(this.Request, list);
      }
    }

    #endregion Web Apis

  }  // class ProcessController

}  //namespace Empiria.Steps.Definition.WebApi
