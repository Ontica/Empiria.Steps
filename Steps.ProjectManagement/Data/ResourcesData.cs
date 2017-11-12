/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.Steps.ProjectManagement.dll              Pattern : Data Service                        *
*  Type     : ResourcesData                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Resource read and write data methods.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Data;
using Empiria.Contacts;

namespace Empiria.Steps.Resources {

  /// <summary>Resource read and write data methods.</summary>
  static internal class ResourcesData {

    static internal List<Resource> GetResourcesList(Contact contact) {
      string sql = $"SELECT * FROM Aggregations " +
                   $"WHERE AggregationName = 'ResourceTree' AND " +
                   $"AggregationItemKind = 'Object' AND " +
                   $"OwnerId IN ({contact.Id}, -1) AND Status <> 'X' " +
                   $"ORDER BY Position";

      var op = DataOperation.Parse(sql);

      return DataReader.GetList<Resource>(op, (x) => BaseObject.ParseList<Resource>(x));
    }

  }  // class ResourcesData

}  // namespace Empiria.Steps.Resources
