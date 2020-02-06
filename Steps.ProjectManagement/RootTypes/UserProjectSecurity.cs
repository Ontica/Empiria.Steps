/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Project Management System           *
*  Assembly : Empiria.ProjectManagement.dll                    Pattern : Data Service                        *
*  Type     : UserProjectSecurity                              License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Project data read and write methods.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;


namespace Empiria.ProjectManagement {

  /// <summary>Project data read and write methods.</summary>
  static public class UserProjectSecurity {


    static public string GetUserAssigneeList() {
      Organization organization = GetUserOrganization();

      return organization.ExtendedData.Get<string>("assingeeList");
    }


    static public Organization GetUserOrganization() {
      var user = Security.EmpiriaIdentity.Current.User.AsContact();

      int organizationId = user.ExtendedData.Get<int>("organizationId");

      return Organization.Parse(organizationId);
    }


    static public string GetUserProjectList() {
      Organization organization = GetUserOrganization();

      return organization.ExtendedData.Get<string>("projectList", "-9999");
    }


    static public string GetUserTemplateList() {
      Organization organization = GetUserOrganization();

      return organization.ExtendedData.Get("templateList", "-9999");
    }


    static public FixedList<Project> GetUserProjectFixedList() {
      string[] stringArray = GetUserProjectList().Split(',');

      Project[] array = new Project[stringArray.Length];

      for (int i = 0; i < array.Length; i++) {
        array[i] = Project.Parse(int.Parse(EmpiriaString.TrimAll(stringArray[i])));
      }

      return new FixedList<Project>(array);
    }


  }  // class UserProjectSecurity


}  // namespace Empiria.ProjectManagement
