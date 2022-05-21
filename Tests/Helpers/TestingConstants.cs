/* Empiria Land **********************************************************************************************
*                                                                                                            *
*  Module   : Empiria Steps Tests                        Component : Test Helpers                            *
*  Assembly : Empiria.Steps.Tests.dll                    Pattern   : Testing constants                       *
*  Type     : TestingConstants                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides Empiria Steps testing constants.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Steps.Tests {

  /// <summary>Provides Empiria Land testing constants.</summary>
  static public class TestingConstants {

    static public string STEP_UID => ConfigurationData.Get<string>("Testing.StepUID");

  }  // class TestingConstants

}  // namespace Empiria.Steps.Tests
