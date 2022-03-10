/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Information Holder                    *
*  Type     : ProjectProcess                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Process instance in a project.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement {

  public class ProjectProcess {

    #region Constructors and parsers

    static internal FixedList<ProjectProcess> GetList(Project forProject) {
      Assertion.AssertObject(forProject, "forProject");

      return ProjectData.GetProjectProcesses(forProject);
    }

    #endregion Constructors and parsers


    #region Properties

    public string UniqueID {
      get {
        if (SubprocessUID.Length != 0) {
          return $"{ProcessUID}-{SubprocessUID}";
        } else {
          return $"{ProcessUID}";
        }
      }
    }


    [DataField("ProcessUID")]
    public string ProcessUID {
      get;
      private set;
    } = String.Empty;


    // [DataField("SubprocessUID")]
    public string SubprocessUID {
      get;
      private set;
    } = String.Empty;


    public string Name {
      get {
        return StartActivity.Name;
      }
    }


    [DataField("StartActivityId")]
    public ProjectItem StartActivity {
      get;
      private set;
    }

    #endregion Properties


  } // class ProjectProcess

}  // namespace Empiria.ProjectManagement
