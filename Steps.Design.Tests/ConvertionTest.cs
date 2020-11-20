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
      string[] uids = { "166871d7-5cd7-40c7-aa47-618db5e643b8",
                        "ea805b13-a259-41c2-ae26-86232f45ac4d",
                        "442537c8-5395-4c25-b3bd-f054d2855e8d"};

      foreach (var projectUID in uids) {
        var project = Project.Parse(projectUID);

        Converter.Convert(project);
      }
    }

  }  // class ConvertionTest

}  //namespace Empiria.Steps.Design.Tests
