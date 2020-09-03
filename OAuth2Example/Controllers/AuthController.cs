using OAuth2Example.BindModels;
using OAuth2Example.DAL.Models;
using System.Net;
using System.Web.Http;
using OAuth2Example.Auth;
using OAuth2Example.DataServices;
using System.Threading.Tasks;
using OAuth2Example.Auth.Attributes;
using System.Web.OData;
using Swashbuckle.Swagger.Annotations;
using System.Collections;
using System.Collections.Generic;

namespace OAuth2Example.Controllers
{
    [Route("api/1.0/auth")]
    public class AuthController : BaseController<AuthToken>
    {
        public AuthController(IService<AuthToken> service, IHttpAuthService authService) : base(service, authService)
        {
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <remarks>
        /// Example return:
        /// 
        ///     {
        ///         "access_token": A1B2C3D4F5A6B7C8,
        ///         "expires_in": 600
        ///     }
        /// 
        /// </remarks>
        /// <param name="bindModel">The user info</param>
        /// <response code="200">The user was logged in successfully</response>
        /// <response code="400">The user info is missing, invalid, wrong, or the grant type is not password</response>
        [HttpPost]
        public IHttpActionResult Post(AuthBindModel bindModel)
        {
            if (bindModel == null || bindModel.grant_type != "password")
                return StatusCodeWithJson(400, new { error = "bad_request" });

            return AuthService.LogIn(bindModel.username, bindModel.password);
        }
    }
}
