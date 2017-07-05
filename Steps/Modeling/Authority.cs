/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Domain Models                 *
*  Assembly : Empiria.Steps.dll                                Pattern : Domain class                        *
*  Type     : Authority                                        License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes an government authority.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;

namespace Empiria.Steps.Modeling {

  public class Authority {

    #region Constructors and parsers

    private Authority() {
      // Required by Empiria Framework.
    }

    static internal Authority Parse(JsonObject data) {
      var o = new Authority();

      o.Entity = Entity.Parse(data.Get<string>("entity/uid"));
      o.Office = Office.Parse(data.Get<string>("office/uid"));
      o.Position = Position.Parse(data.Get<string>("position/uid"));

      return o;
    }

    static public Authority Empty {
      get {
        return new Authority();
      }
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("AuthEntityId")]
    public Entity Entity {
      get;
      private set;
    } = Entity.Empty;


    [DataField("AuthOfficeId")]
    public Office Office {
      get;
      private set;
    } = Office.Empty;


    [DataField("AuthPositionId")]
    public Position Position {
      get;
      private set;
    } = Position.Empty;


    #endregion Properties

  }  // class Authority

}  // namespace Empiria.Steps.Modeling
