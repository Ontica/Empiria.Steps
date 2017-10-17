/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : TextTemplate                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Parses content using a text template.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

namespace Empiria.Steps.Presentation {

  /// <summary>Parses content using a text template.</summary>
  public class TextTemplate : BaseObject {

    #region Constructors and parsers

    private TextTemplate() {
      // Required by Empiria Framework.
    }

    static internal TextTemplate Parse(int id) {
      return BaseObject.ParseId<TextTemplate>(id);
    }

    static public TextTemplate Parse(string namedKey) {
      return BaseObject.ParseKey<TextTemplate>(namedKey);
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("ObjectKey")]
    public string Key {
      get;
      private set;
    }


    [DataField("ObjectName")]
    public string Description {
      get;
      private set;
    }


    [DataField("ObjectExtData")]
    public string TemplateText {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    public string ParseContent(Dictionary<string, string> replacementRules) {
      var content = this.TemplateText;

      foreach (var rule in replacementRules) {
        content = content.Replace(rule.Key, rule.Value);
      }
      return content;
    }

    #endregion Public methods

  }  // class TextTemplate

}  // // namespace Empiria.Steps.Presentation
