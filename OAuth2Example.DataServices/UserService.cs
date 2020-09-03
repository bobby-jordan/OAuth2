using OAuth2Example.DAL.Models;
using OAuth2Example.DAL;
using OAuth2Example.DataServices.Crypto;

namespace OAuth2Example.DataServices
{
    internal class UserService : BaseService<User>
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override bool Validate(User model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                ValidationDictionary.AddError("email", "missing");
            }
            else if (model.Email.Length < 3 || model.Email.Length > 30)
            {
                ValidationDictionary.AddError("email", "invalid length");
            }
            else if (repository.Get(u => u.Id != model.Id && u.Email == model.Email) != null)
            {
                ValidationDictionary.AddError("email", "the given email is already taken");
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                ValidationDictionary.AddError("password", "missing");
            }

            return ValidationDictionary.IsValid;
        }

        protected override bool OnBeforeInsert(User model)
        {
            HashPassword(model);

            return true;
        }

        protected override bool OnBeforeUpdate(User model)
        {
            if (string.IsNullOrEmpty(model.Salt))
            {
                HashPassword(model);
            }

            return true;
        }

        private void HashPassword(User user)
        {
            string salt = "";
            user.Password = PasswordHasher.HashPassword(user.Password, ref salt);
            user.Salt = salt;
        }
    }
}
