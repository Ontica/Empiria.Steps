/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Domain class                        *
*  Type     : ProjectItem                                      License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Abstract class that describes a project item, like an activity, a task or summary.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Empiria.ProjectManagement {

  public class GraphData {

    public GraphData(string project) {
      this.Project = project;
      //this.Person = person;
    }

    internal static FixedList<GraphData> Load(FixedList<ProjectItem> activities) {
      Dictionary<string, GraphData> dictionary = new Dictionary<string, GraphData>(50);

      foreach (var item in activities) {
        if (!(item is Activity)) {
          continue;
        }

        var activity = (Activity) item;

        var key = $"{activity.Project.Name}";

        if (!dictionary.ContainsKey(key)) {
          dictionary.Add(key, new GraphData(activity.Project.Name));
        }
        dictionary[key].Accumulate(activity);
      }

      return new FixedList<GraphData>(dictionary.Values.ToList());

    }

    private void Accumulate(Activity item) {
      this.TotalTasks++;
      if (this.FirstDate > item.Deadline) {
        this.FirstDate = item.Deadline;
      }
      if (item.Deadline != ExecutionServer.DateMaxValue) {
        if (this.LastDate < item.Deadline) {
          this.LastDate = item.Deadline;
        }
      }
    }


    public string Project {
      get;
      set;
    }


    public string Authority {
      get;
      set;
    }


    public string Person {
      get;
      set;
    }


    public int TotalTasks {
      get;
      set;
    }


    public int TotalDays {
      get {
        return Convert.ToInt32(this.LastDate.Subtract(this.FirstDate).TotalDays);
      }
    }


    public DateTime FirstDate {
      get;
      set;
    } = DateTime.Today;


    public DateTime LastDate {
      get;
      set;
    } = DateTime.Today;


  }

}