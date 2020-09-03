using OAuth2Example.DataServices;
using System.Web.Http;

namespace OAuth2Example.Auth
{
    public interface IHttpAuthService : IAuthService
    {
        IHttpActionResult LogIn(string email, string password);
    }
}
