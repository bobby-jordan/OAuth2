using System;
using System.Web.Http;
using OAuth2Example.DAL.Models;
using OAuth2Example.DataServices;
using System.Web.Http.Results;
using OAuth2Example.DataServices.Crypto;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace OAuth2Example.Auth
{
    internal class OAuth2AuthService : IHttpAuthService
    {
        private readonly IService<User> userService;
        private readonly IService<AuthToken> tokenService;

        public User User { get; set; }
        internal ApiController Controller { get; set; }

        public OAuth2AuthService(IService<User> userService, IService<AuthToken> tokenService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public IHttpActionResult LogIn(string email, string password)
        {
            User user = userService.Get(u => u.Email == email);
            if (user == null || !PasswordHasher.VerifyPassword(password, user.Salt, user.Password))
                return new BadRequestErrorMessageResult(JsonConvert.SerializeObject(new { error = "bad_request" }), Controller);

            DateTime now = DateTime.UtcNow;
            DateTime expires = now.AddDays(1);

            AuthToken token = tokenService.Get(t => t.User.Id == user.Id && t.ExpiresAt > now);

            if(token != null)
            {
                token.ExpiresAt = expires;

                tokenService.Update(token);
                tokenService.Save();

                return MakeOkResult(new { access_token = token.Token, expires_in = (expires - now).TotalSeconds });
            }

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[16];
                rng.GetBytes(buffer);

                string tokenString = BitConverter.ToString(buffer).Replace("-", "");

                token = new AuthToken
                {
                    User = user,
                    Token = tokenString,
                    CreatedAt = now,
                    ExpiresAt = expires
                };

                tokenService.Insert(token);
                tokenService.Save();

                // Hack to make OkNegotiatedContent infer the type argument using the dynamic object
                return MakeOkResult(new { access_token = tokenString, expires_in = (expires - now).TotalSeconds });
            }
        }

        private OkNegotiatedContentResult<T> MakeOkResult<T>(T content)
        {
            return new OkNegotiatedContentResult<T>(content, Controller);
        }
    }
}
