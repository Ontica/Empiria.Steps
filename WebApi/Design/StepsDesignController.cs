/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design                                 Component : Web Api                               *
*  Assembly : Empiria.Steps.WebApi.dll                     Pattern   : Controller                            *
*  Type     : StepsDesignController                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Web Api used to work with steps design definitions.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Steps.Design.Adapters;
using Empiria.Steps.Design.UseCases;

namespace Empiria.Steps.Design.WebApi {

  /// <summary>Web Api used to work with steps design definitions.</summary>
  public class StepsDesignController : WebApiController {

    #region Web Apis

    [HttpGet]
    [Route("v3/steps/design/{stepUID:guid}")]
    public SingleObjectModel GetStep([FromUri] string stepUID) {

      using (var usecases = StepsDesignUseCases.UseCaseInteractor()) {
        StepDto stepDto = usecases.GetStep(stepUID);

        return new SingleObjectModel(this.Request, stepDto);
      }
    }


    [HttpPost]
    [Route("v3/steps/design")]
    public CollectionModel SearchSteps([FromBody] SearchStepsCommand searchCommand) {

      base.RequireBody(searchCommand);

      using (var usecases = StepsDesignUseCases.UseCaseInteractor()) {
        FixedList<StepDescriptorDto> list = usecases.SearchSteps(searchCommand);

        return new CollectionModel(this.Request, list);
      }
    }

    #endregion Web Apis

  }  // class StepsDesignController

}  //namespace Empiria.Steps.Design.WebApi
