/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Project Management                           Component : Application services                  *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Information Holder                    *
*  Type     : WhatIfResult                                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Contains information about all project items which will be changed as a consequence            *
*             of performing an update operation in a given project item operation known as source.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria.ProjectManagement.Services {

  /// <summary>Contains information about all project items which will be changed as a consequence
  /// of performing an update operation in a given project item operation known as source.</summary>
  public class WhatIfResult {

    #region Fields


    private List<ProjectItemStateChange> stateChanges = new List<ProjectItemStateChange>();
    private List<Exception> exceptions = new List<Exception>();


    #endregion Fields


    #region Constructors and parsers


    internal WhatIfResult(ProjectItemStateChange source) {
      this.Source = source;

      this.AddStateChange(this.Source);
    }


    #endregion Constructors and parsers


    #region Properties


    public ProjectItemStateChange Source {
      get;
    }


    public bool HasErrors {
      get {
        return (this.exceptions.Count != 0);
      }
    }


    public FixedList<ProjectItemStateChange> StateChanges {
      get {
        return this.stateChanges.ToFixedList();
      }
    }


    #endregion Properties


    #region Methods


    internal void AddException(Exception exception) {
      this.exceptions.Add(exception);
    }


    internal void AddStateChange(ProjectItemStateChange stateChange) {
      this.stateChanges.Add(stateChange);
    }


    public Exception GetException() {
      return new Exception("This is the exception in WhatIfResult");
    }


    #endregion Methods

  }  // class WhatIfResult

}  // namespace Empiria.ProjectManagement.Services
