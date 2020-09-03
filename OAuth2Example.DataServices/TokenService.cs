using OAuth2Example.DAL.Models;
using OAuth2Example.DAL;

namespace OAuth2Example.DataServices
{
    internal class TokenService : BaseService<AuthToken>
    {
        public TokenService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override bool Validate(AuthToken model)
        {
            return true;
        }
    }
}
