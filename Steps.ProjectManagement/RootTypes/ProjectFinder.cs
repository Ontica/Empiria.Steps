/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.ProjectManagement.dll              Pattern : Finder                              *
*  Type     : ProjectFinder                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
w  Summary  : Performs search operations over projects, activities, and other project items.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Data;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Performs search operations over projects, activities, and other project items.</summary>
  public class ProjectFinder {

    #region Constructors and parsers

    public ProjectFinder(ActivityFilter filter) {
      if (filter == null) {
        filter = new ActivityFilter();
      }
      this.Filter = filter;
    }

    #endregion Constructors and parsers

    #region Public properties

    public ActivityFilter Filter {
      get;
    }

    #endregion Public properties

    #region Public methods

    public FixedList<ProjectObject> GetActivitiesList(ActivityOrder orderBy = ActivityOrder.Default) {
      var list = ProjectData.GetNoSummaryActivities(this.FilterAsSql());

      list = this.ApplyNoSqlFilter(list);
      list = this.ApplySort(list, orderBy);

      return list.ToFixedList();
    }

    public FixedList<ProjectObject> GetActivitiesTree(ActivityOrder orderBy = ActivityOrder.Default) {
      throw new NotImplementedException();

      //var project = ProjectData.GetNoSummaryActivities(this.Filter);

      //var list = project.GetActivities(this.Filter, orderBy)
      //                  .FindAll((x) => x.ProjectObjectType != ProjectObjectType.SummaryType);

      //return list;
    }

    #endregion Public methods

    #region Private methods

    private List<ProjectObject> ApplySort(List<ProjectObject> list, ActivityOrder orderBy) {
      switch (orderBy) {
        case ActivityOrder.DueDate:

          return (from item in list
                  orderby item.DueDate, item.Name
                  select item)
                  .ToList();


        case ActivityOrder.Responsible:

          var list1 = from item in list
                      where ((Activity) item).Responsible.Id != -1
                      orderby ((Activity) item).Responsible.Nickname, item.DueDate, item.Name
                      select item;

          var list2 = from item in list
                      where ((Activity) item).Responsible.Id == -1
                      orderby item.DueDate, item.Name
                      select item;

          return list1.Concat(list2).ToList();


        case ActivityOrder.TargetDate:

          return (from item in list
                  orderby item.TargetDate, item.Name
                  select item)
                  .ToList();


        case ActivityOrder.ActivityName:

          return (from item in list
                  orderby item.Name
                  select item)
                  .ToList();


        case ActivityOrder.Default:
        default:

          return (from item in list
                  orderby item.Ordering, item.Id
                  select item)
                 .ToList();
      }
    }

    private List<ProjectObject> ApplyNoSqlFilter(List<ProjectObject> list) {
      return list;
    }

    private string FilterAsSql() {
      return GeneralDataOperations.BuildSqlAndFilter(this.Filter.GetProjectsSqlFilter(),
                                                     this.Filter.GetResponsiblesSqlFilter(),
                                                     this.Filter.GetTagsSqlFilter(),
                                                     this.Filter.GetKeywordsFilter());
    }

    #endregion Private methods

  } // class ProjectFinder

} // namespace Empiria.Steps.ProjectManagement
