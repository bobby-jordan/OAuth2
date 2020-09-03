using OAuth2Example.DAL.Models;

namespace OAuth2Example.DataServices
{
    public interface IAuthService
    {
        User User { get; set; }
    }
}
