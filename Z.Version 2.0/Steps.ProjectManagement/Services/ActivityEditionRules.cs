/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : ActivityEditionRules                             License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes a project activity.                                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

namespace Empiria.ProjectManagement {


  public class ActivityEditionRules {

    private Activity activity;

    public ActivityEditionRules(Activity activity) {
      this.activity = activity;
    }


    public bool CanChangeDeadline {
      get {
        return false;
      }
    }


    public bool CanBeClosed {
      get {
        return false;
      }
    }


    public bool CanBeDeleted {
      get {
        return false;
      }
    }


    public bool DeadlineChangeUpdatesOthers {
      get {
        return false;
      }
    }


    public bool HasOpenedPredecessors {
      get {
        return false;
      }
    }


    public bool HasOpenedSuccessors {
      get {
        return false;
      }
    }


  }  // class ActivityEditionRules


}  // namespace Empiria.ProjectManagement
