/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Response methods                    *
*  Type     : AuthorityResponseModel                         License : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Response static methods for authority/entity objects.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Steps.Modeling;

namespace Empiria.Steps.WebApi {

  /// <summary>Response static methods for authority/entity objects</summary>
  static internal class AuthorityResponseModel {

    static internal ICollection ToResponse(this IList<Entity> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var entity in list) {
        var item = new {
          uid = entity.UID,
          name = entity.FullName,
          shortName = entity.Nickname
        };
        array.Add(item);
      }
      return array;
    }

    static internal object ToResponse(this Authority authority) {
      return new {
        entity = new {
          uid = authority.Entity.UID,
          name = authority.Entity.FullName,
        },
        office = new {
          uid = authority.Office.UID,
          name = authority.Office.FullName,
        },
        position = new {
          uid = authority.Position.UID,
          name = authority.Position.FullName,
          phone = authority.Position.Phone
        },
        contact = new {
          uid = authority.Position.Officer.UID,
          name = authority.Position.Officer.FullName,
          email = authority.Position.Officer.EMail
        }
      };
    }

    static internal object ToResponse(this Entity entity) {
      return new {
        uid = entity.UID,
        name = entity.FullName,
        shortName = entity.Nickname,
        offices = entity.Authorities.ToResponse(),
        positions = entity.Positions.ToResponse()
      };
    }

    static internal ICollection ToResponse(this IList<Office> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var authority in list) {
        var item = new {
          uid = authority.UID,
          name = authority.FullName
        };
        array.Add(item);
      }
      return array;
    }

    static internal object ToResponse(this Office authority) {
      return new {
        uid = authority.UID,
        name = authority.FullName,
        headPosition = authority.HeadPosition.ToResponse()
      };
    }


    static internal ICollection ToResponse(this IList<Position> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var position in list) {
        var item = position.ToResponse();
        array.Add(item);
      }
      return array;
    }

    static internal object ToResponse(this Position position) {
      return new {
        uid = position.UID,
        name = position.FullName,
        phone = position.Phone,
        officer = new {
          uid = position.Officer.UID,
          name = position.Officer.FullName,
          email = position.Officer.EMail,
        }
      };
    }

  }  // class AuthorityResponseModel

}  // namespace Empiria.Steps.WebApi
