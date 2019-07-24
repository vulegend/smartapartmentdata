using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject.Activation;
using SmartApartmentDataTest.Auth;
using SmartApartmentDataTest.Requests;
using SmartApartmentDataTestDataModule.Data_Access;
using SmartApartmentDataTestDataModule.DTOs;
using SmartApartmentDataTestDataModule.Exceptions;
using SmartApartmentDataTestDataModule.Models;
using SmartApartmentDataTestDataModule.Responses;
using SmartApartmentDataTestUtility.Logger;

namespace SmartApartmentDataTest.Controllers
{
    public class GuildController : ApiController
    {
        #region Private Fields

        private readonly ISmartApartmentDataTestDataAccess _dataAccess;

        #endregion

        #region Constructor

        public GuildController(ISmartApartmentDataTestDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        #endregion

        #region POST

        [HttpPost]
        [PermissionAuthorize("GuildMaster")]
        [Route("api/v1/guild/add")]
        public HttpResponseMessage AddGuild(GuildModelDto request)
        {
            try
            {
                _dataAccess.AddGuild(request);
                var getPlayer =
                    _dataAccess.GetPlayerByUserId(((UserModel)ActionContext.Request.Properties["user"]).Id);
                _dataAccess.AddGuildMember(getPlayer.Name, request.Name);
                return Request.CreateResponse(HttpStatusCode.OK, "Guild successfully added");
            }
            catch (PlayerNotFoundException playerNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(PlayerNotFoundException.ERROR_CODE, playerNotFoundException.Message));
            }
            catch (GuildNotFoundException guildNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(GuildNotFoundException.ERROR_CODE, guildNotFoundException.Message));
            }
            catch (FactionNotFoundException factionNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(FactionNotFoundException.ERROR_CODE, factionNotFoundException.Message));
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        [HttpPost]
        [PermissionAuthorize("GuildMaster")]
        [Route("api/v1/guild/members/add")]
        public HttpResponseMessage AddGuildMember(AddGuildMemberRequest request)
        {
            try
            {
                _dataAccess.AddGuildMember(request.PlayerName, request.GuildName);
                return Request.CreateResponse(HttpStatusCode.OK, "Player successfully added to the guild");
            }
            catch (PlayerNotFoundException playerNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(PlayerNotFoundException.ERROR_CODE, playerNotFoundException.Message));
            }
            catch (GuildNotFoundException guildNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(GuildNotFoundException.ERROR_CODE, guildNotFoundException.Message));
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        #endregion

        #region GET

        [HttpGet]
        [PermissionAuthorize("User")]
        [Route("api/v1/guild/members/{guildName}")]
        public HttpResponseMessage GetGuildMembers(string guildName)
        {
            try
            {
                var getGuildMembers = _dataAccess.GetGuildMembers(guildName);
                return Request.CreateResponse(HttpStatusCode.OK, getGuildMembers);
            }
            catch (GuildNotFoundException guildNotFoundException)
            {
                SmartApartmentLogger.LogAsync(ELogType.Debug, "Test");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(GuildNotFoundException.ERROR_CODE, guildNotFoundException.Message));
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        #endregion
    }
}
