/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.ProjectManagement.dll              Pattern : Data Service                        *
*  Type     : ProjectContactsData                              License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Project contacts data read and write methods.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;
using Empiria.Data;

namespace Empiria.Steps.ProjectManagement {

  /// <summary>Project contacts data read and write methods.</summary>
  static internal class ProjectContactsData {

    static internal FixedList<Contact> GetProjectResponsibles(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (61, 62, 63, 64, 65) " +
                   $"ORDER BY Nickname";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }


    static internal FixedList<Contact> GetProjectRequesters(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (65, 66, 67, 68) " +
                   $"ORDER BY Nickname";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }


    static internal FixedList<Contact> GetProjectTaskManagers(Project project) {
      string sql = $"SELECT * FROM Contacts " +
                   $"WHERE ContactId IN (67, 69, 70) " +
                   $"ORDER BY Nickname";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList(op, (x) => BaseObject.ParseList<Contact>(x))
                       .ToFixedList();
    }

  }  // class ProjectContactsData

}  // namespace Empiria.Steps.ProjectManagement
