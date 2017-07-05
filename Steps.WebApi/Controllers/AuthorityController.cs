/* Empiria Steps *********************************************************************************************
*                                                                                                            *
*  Solution : Empiria Steps                                    System  : Steps Web API                       *
*  Assembly : Empiria.Steps.WebApi.dll                         Pattern : Web Api Controller                  *
*  Type     : AuthorityController                              License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Gets and sets authority and entities data.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Steps.Modeling;

namespace Empiria.Steps.WebApi {

  /// <summary>Gets and sets authority and entities data.</summary>
  public class AuthorityController : WebApiController {

    #region Public APIs

    [HttpGet, AllowAnonymous]
    [Route("v1/modeling/entities")]
    public CollectionModel GetEntitiesList([FromUri] string filter = "") {
      try {
        var list = Entity.GetList(filter ?? String.Empty);

        return new CollectionModel(this.Request, BuildResponse(list), typeof(Entity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet, AllowAnonymous]
    [Route("v1/modeling/entities/{entityUID}")]
    public SingleObjectModel GetEntity([FromUri] string entityUID) {
      try {
        base.RequireResource(entityUID, "entityUID");

        var entity = Entity.Parse(entityUID);

        return new SingleObjectModel(this.Request, BuildResponse(entity), typeof(Entity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet, AllowAnonymous]
    [Route("v1/modeling/offices/{officeUID}")]
    public SingleObjectModel GetOffice([FromUri] string officeUID) {
      try {
        base.RequireResource(officeUID, "officeUID");

        var authority = Office.Parse(officeUID);

        return new SingleObjectModel(this.Request, BuildResponse(authority), typeof(Office).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

    #region Private methods

    private ICollection BuildResponse(FixedList<Entity> list) {
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

    private object BuildResponse(Entity entity) {
      return new {
        uid = entity.UID,
        name = entity.FullName,
        shortName = entity.Nickname,
        offices = BuildResponse(entity.Authorities),
        positions = BuildResponse(entity.Positions)
      };
    }

    private ICollection BuildResponse(FixedList<Office> list) {
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

    private object BuildResponse(Office authority) {
      return new {
        uid = authority.UID,
        name = authority.FullName,
        headPosition = BuildResponse(authority.HeadPosition)
      };
    }

    private ICollection BuildResponse(FixedList<Position> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var position in list) {
        var item = BuildResponse(position);
        array.Add(item);
      }
      return array;
    }

    private object BuildResponse(Position position) {
      return new {
        uid = position.UID,
        name = position.FullName,
        phoneNumber = position.PhoneNumber,
        officer = new {
          uid = position.Officer.UID,
          name = position.Officer.FullName,
          email = position.Officer.EMail,
        }
      };
    }

    #endregion Private methods

  }  // class AuthorityController

}  // namespace Empiria.Steps.WebApi
