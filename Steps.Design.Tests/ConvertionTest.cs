/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Steps Design Services                      Component : Unit and Integration Tests              *
*  Assembly : Empiria.Steps.Design.Tests.dll             Pattern   : Tests Class                             *
*  Type     : ConvertionTest                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Tests convertion services.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Empiria.ProjectManagement;
using Empiria.Steps.Design.Integration;

namespace Empiria.Steps.Design.Tests {

  /// <summary>Tests convertion services</summary>
  [TestClass]
  public class ConvertionTest {

/*                      "ab329289-ed96-4565-85f5-1145c79537ad",
                        "be9fd6bf-c4b4-4ff2-ae14-d8ce5624de47",
                        "c8ae585c-36dc-4834-827e-fa3767009e3d"

*/    [TestMethod]
    public void Convert() {
      string[] uids = { "163286aa-5ddf-4206-85d0-5888c977e4de",
                        "ab329289-ed96-4565-85f5-1145c79537ad",
                        "be9fd6bf-c4b4-4ff2-ae14-d8ce5624de47",
                        "50036465-c8fe-465b-b8f0-e4fe2d3e679a",
                        "f81744a3-0fc3-46bd-a728-90ece8187abc",
                        "c8ae585c-36dc-4834-827e-fa3767009e3d",
                        "5234852e-0cfd-4d9d-8ec5-4252afd9ff7d"};

      foreach (var projectUID in uids) {
        var project = Project.Parse(projectUID);

        Converter.Convert(project);
      }
    }

  }  // class ConvertionTest

}  //namespace Empiria.Steps.Design.Tests
