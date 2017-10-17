/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Enumeration                         *
*  Type     : HypertextEngine                                  License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Represents a time unit used to describe activity due terms.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Empiria.Steps.Legal;

namespace Empiria.Steps.Presentation {

  public class Hypertext {

    static public string ToAcronymHypertext(string source) {
      var template = TextTemplate.Parse("Acronym.Link");

      var documentsList = Document.GetList();

      var hypertext = source;

      foreach (var document in documentsList) {

        // dictionary work

        var dictionary = new Dictionary<string, string>(2);

        dictionary.Add("{{URL}}", document.Url);
        dictionary.Add("{{ACRONYM}}", document.Code);
        dictionary.Add("{{BUBBLE-TEXT}}", document.Name);

        var content = template.ParseContent(dictionary);

        // END dictionary work

        string textToFind = String.Format(@"\b{0}\b", document.Code);

        hypertext = Regex.Replace(hypertext, textToFind, content);
      }
      return hypertext;
    }

    static public string ToTermDefinitionHypertext(string source, Contract contract) {
      var template = TextTemplate.Parse("Term.Definition");

      var clausesList = Clause.GetList(contract);
      clausesList = clausesList.FindAll((x) => x.Section == "Cláusulas" && x.Number == "1.1");
      clausesList.Sort((x, y) => x.Title.CompareTo(y.Title));

      var hypertext = source;

      foreach (var clause in clausesList) {

        // dictionary work
        var dictionary = new Dictionary<string, string>(2);

        var clauseTitle = CleanClauseTitle(clause.Title);

        dictionary.Add("{{TERM}}", clauseTitle);
        dictionary.Add("{{DEFINITION}}", clause.Text);

        var content = template.ParseContent(dictionary);

        // END dictionary work

        var regex = new Regex(String.Format(@"(?<!<[^>]*)\b{0}\b", clauseTitle));

        hypertext = regex.Replace(hypertext, content, 10);
      }

      return hypertext;
    }

    private static string CleanClauseTitle(string title) {
      title = title.Replace("“", "");
      title = title.Replace("”", "");
      title = title.Replace("\"", "");

      return EmpiriaString.TrimAll(title);
    }
  }

  ///// <summary>Represents a time unit used to describe activity due terms.</summary>
  //public class Hypertext {

  //  public AddRules(ReplacementRulesList list) {
  //    Assertion.AssertObject(list, "list");

  //    this.rules = list;
  //  }

  //  public string Convert(string content) {
  //    string hypertext = content;

  //    foreach (var rule in this.ReplacementRules) {
  //      hypertext = rule.ApplyTo(hypertext);
  //    }

  //    return hypertext;
  //  }

  //}  // class Hypertext

}  // namespace Empiria.Steps.Presentation
