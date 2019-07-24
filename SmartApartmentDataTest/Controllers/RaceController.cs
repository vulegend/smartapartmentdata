using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartApartmentDataTest.Auth;
using SmartApartmentDataTest.Requests;
using SmartApartmentDataTestDataModule.Data_Access;
using SmartApartmentDataTestDataModule.DTOs;
using SmartApartmentDataTestDataModule.Exceptions;
using SmartApartmentDataTestDataModule.Responses;
using SmartApartmentDataTestUtility.Logger;

namespace SmartApartmentDataTest.Controllers
{
    public class RaceController : ApiController
    {
        #region Private Fields

        private readonly ISmartApartmentDataTestDataAccess _dataAccess;

        #endregion

        #region Constructor

        public RaceController(ISmartApartmentDataTestDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        #endregion

        #region POST

        [HttpPost]
        [PermissionAuthorize("Admin")]
        [Route("api/v1/addRace")]
        public HttpResponseMessage AddRace(RaceModelDto request)
        {
            try
            {
                _dataAccess.AddRace(request);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception exception)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, exception.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        #endregion

        #region GET

        [HttpGet]
        [AllowAnonymous]
        [Route("api/v1/raceNames")]
        public HttpResponseMessage GetRaceNames()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dataAccess.GetRaceNames());
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(500, e.Message));
            }
        }

        [HttpGet]
        [PermissionAuthorize("User")]
        [Route("api/v1/race/{id:int}")]
        public HttpResponseMessage GetRaceById(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dataAccess.GetRaceByID(id));
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(500, e.Message));
            }
        }

        [HttpGet]
        [PermissionAuthorize("User")]
        [Route("api/v1/race/{name}")]
        public HttpResponseMessage GetRaceByName(string name)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dataAccess.GetRaceByName(name));
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(500, e.Message));
            }
        }

        #endregion

        #region DELETE



        [HttpDelete]
        [PermissionAuthorize("Admin")]
        [Route("api/v1/deleteRace")]
        public HttpResponseMessage DeleteRace(DeleteRaceRequest request)
        {
            try
            {
                if (request.Id.HasValue)
                    _dataAccess.DeleteRaceByID(request.Id.Value);
                else if (!request.Name.Equals(string.Empty))
                    _dataAccess.DeleteFactionByName(request.Name);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (RaceNotFoundException raceNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(RaceNotFoundException.ERROR_CODE, raceNotFoundException.Message));
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(500, "Internal server error. Please try again later"));
            }
        }

        #endregion
    }
}
