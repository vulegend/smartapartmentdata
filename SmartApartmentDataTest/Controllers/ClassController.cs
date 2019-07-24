using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class ClassController : ApiController
    {
        #region Private Fields

        private readonly ISmartApartmentDataTestDataAccess _dataAccess;

        #endregion

        #region Constructor

        public ClassController(ISmartApartmentDataTestDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        #endregion

        #region POST

        [HttpPost]
        [PermissionAuthorize("Admin")]
        [Route("api/v1/addClass")]
        public HttpResponseMessage AddClass(ClassModelDto request)
        {
            try
            {
                _dataAccess.AddClass(request);
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
        [Route("api/v1/classNames")]
        public HttpResponseMessage GetClassNames()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dataAccess.GetClassNames());
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(500, e.Message));
            }
        }

        [HttpGet]
        [PermissionAuthorize("User")]
        [Route("api/v1/class/{id:int}")]
        public HttpResponseMessage GetClassById(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dataAccess.GetClassByID(id));
            }
            catch (ClassNotFoundException classNotFoundException)
            {
                SmartApartmentLogger.LogAsync(ELogType.Debug, classNotFoundException.Message, $"ErrorCode:{ClassNotFoundException.ERROR_CODE}");
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(ClassNotFoundException.ERROR_CODE, classNotFoundException.Message));
            }
            catch (Exception e)
            {
                SmartApartmentLogger.LogAsync(ELogType.Error, e.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(500, e.Message));
            }
        }

        [HttpGet]
        [PermissionAuthorize("User")]
        [Route("api/v1/class/{name}")]
        public HttpResponseMessage GetClassByName(string name)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _dataAccess.GetClassByName(name));
            }
            catch (ClassNotFoundException classNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new ErrorResponse(ClassNotFoundException.ERROR_CODE, classNotFoundException.Message));
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
        [Route("api/v1/deleteClass")]
        public HttpResponseMessage DeleteClass(DeleteClassRequest request)
        {
            try
            {
                if (request.Id.HasValue)
                    _dataAccess.DeleteClassByID(request.Id.Value);
                else if (!request.Name.Equals(string.Empty))
                    _dataAccess.DeleteClassByName(request.Name);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ClassNotFoundException classNotFoundException)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(ClassNotFoundException.ERROR_CODE, classNotFoundException.Message));
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
