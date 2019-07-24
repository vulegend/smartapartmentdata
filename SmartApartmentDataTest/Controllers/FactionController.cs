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
    public class FactionController : ApiController
    {
        #region Private Fields

        private readonly ISmartApartmentDataTestDataAccess _dataAccess;

        #endregion

        #region Constructor

        public FactionController(ISmartApartmentDataTestDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        #endregion

        #region POST

        [HttpPost]
        [PermissionAuthorize("Admin")]
        [Route("api/v1/addFaction")]
        public HttpResponseMessage AddFaction(FactionModelDto request)
        {
            try
            {
                _dataAccess.AddFaction(request);
            }
            catch (Exception exception)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, exception.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(500, "Internal server error. Please try again later"));
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        #endregion

        #region GET

        [HttpGet]
        [AllowAnonymous]
        [Route("api/v1/factionNames")]
        public HttpResponseMessage GetFactionNames()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dataAccess.GetFactionNames());
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(500, e.Message));
            }
        }

        [HttpGet]
        [PermissionAuthorize("User")]
        [Route("api/v1/faction/{id:int}")]
        public HttpResponseMessage GetFactionById(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dataAccess.GetFactionByID(id));
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(500, e.Message));
            }
        }

        [HttpGet]
        [PermissionAuthorize("User")]
        [Route("api/v1/faction/{name}")]
        public HttpResponseMessage GetFactionByName(string name)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dataAccess.GetFactionByName(name));
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
        [Route("api/v1/deleteFaction")]
        public HttpResponseMessage DeleteFaction(DeleteFactionRequest request)
        {
            try
            {
                if (request.Id.HasValue)
                    _dataAccess.DeleteFactionByID(request.Id.Value);
                else if (!request.Name.Equals(string.Empty))
                    _dataAccess.DeleteFactionByName(request.Name);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (FactionNotFoundException factionNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(FactionNotFoundException.ERROR_CODE, factionNotFoundException.Message));
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
