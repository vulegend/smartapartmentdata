using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartApartmentDataTest.Auth;
using SmartApartmentDataTestDataModule.Data_Access;
using SmartApartmentDataTestDataModule.DTOs;
using SmartApartmentDataTestDataModule.Exceptions;
using SmartApartmentDataTestDataModule.Models;
using SmartApartmentDataTestDataModule.Responses;
using SmartApartmentDataTestUtility.Logger;

namespace SmartApartmentDataTest.Controllers
{
    public class PlayerController : ApiController
    {
        #region Private Fields

        private readonly ISmartApartmentDataTestDataAccess _dataAccess;

        #endregion

        #region Constructor

        public PlayerController(ISmartApartmentDataTestDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        #endregion

        #region POST

        [HttpPost]
        [PermissionAuthorize("User")]
        [Route("api/v1/player/add")]
        public HttpResponseMessage AddPlayer(PlayerModelDto request)
        {
            try
            {
                _dataAccess.AddPlayer(request, ((UserModel)ActionContext.Request.Properties["user"]).Id);
                return Request.CreateResponse(HttpStatusCode.OK, "Player successfully added");
            }
            catch (ClassNotFoundException classNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(ClassNotFoundException.ERROR_CODE, classNotFoundException.Message));
            }
            catch (FactionNotFoundException factionNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(FactionNotFoundException.ERROR_CODE, factionNotFoundException.Message));
            }
            catch (RaceNotFoundException raceNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(RaceNotFoundException.ERROR_CODE, raceNotFoundException.Message));
            }
            catch (PlayerAlreadyExistsException playerAlreadyExistsException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(PlayerAlreadyExistsException.ERROR_CODE, playerAlreadyExistsException.Message));
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
        [AllowAnonymous]
        [Route("api/v1/player/{name}")]
        public HttpResponseMessage GetPlayerByName(string name)
        {
            try
            {
                var playerModel = _dataAccess.GetPlayerByName(name);
                return Request.CreateResponse(HttpStatusCode.OK, playerModel);
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
