/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Module   : Messaging services                           Component : Presentation Layer                    *
*  Assembly : Empiria.ProjectManagement.dll                Pattern   : Utility methods                       *
*  Type     : MessagingUtilities                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Utility static methods used in messaging services.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;

namespace Empiria.ProjectManagement.Messaging {

/// <summary>Utility static methods used in messaging services.</summary>
  static internal class MessagingUtilities {

    #region Utility methods


    static internal string GetActivityRowColor(ProjectItem item) {
      if (item.Deadline <= DateTime.Today.AddDays(7)) {
        return COLORS.red;

      } else if (item.Deadline <= DateTime.Today.AddDays(14)) {
        return COLORS.amber;

      } else if (item.Deadline <= DateTime.Today.AddDays(28)) {
        return COLORS.green;

      } else {
        return COLORS.gray;
      }
    }


    static internal FixedList<Activity> GetAllUpcomingActivities(Project project) {
      FixedList<Activity> upcoming = project.GetItems()
                                               .FindAll(x => x is Activity && x.IsUpcoming &&
                                                             !((Activity) x).Responsible.EMail.Contains("talanza"))
                                               .ConvertAll(x => (Activity) x)
                                               .ToFixedList();

      upcoming.Sort((x, y) => x.Deadline.CompareTo(y.Deadline));

      return upcoming;
    }


    static internal FixedList<Activity> GetUserUpcomingActivities(Project project, Person user) {
      FixedList<Activity> upcoming = project.GetItems()
                                               .FindAll(x => x is Activity && x.IsUpcoming &&
                                                             ((Activity) x).Responsible.Equals(user))
                                               .ConvertAll(x => (Activity) x)
                                               .ToFixedList();

      upcoming.Sort((x, y) => x.Deadline.CompareTo(y.Deadline));

      return upcoming;
    }


    #endregion Utility methods


    #region Inner COLORS constants

    static private class COLORS {

      static internal readonly string green = "##00bbb4";
      static internal readonly string amber = "#f7931e";
      static internal readonly string red = "#ff0000";

      static internal readonly string gray = "#a8a8a8";
      static internal readonly string ghost_color = "#ececec";
      static internal readonly string default_color = "#3dbab3";
      static internal readonly string empty = "";

    }

    #endregion Inner COLORS constants

  }  // class MessagingUtilities

}  // namespace Empiria.ProjectManagement.Messaging
