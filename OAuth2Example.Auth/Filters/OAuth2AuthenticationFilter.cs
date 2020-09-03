using Autofac.Integration.WebApi;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using OAuth2Example.DataServices;
using OAuth2Example.DAL.Models;
using System.Web.Http;
using System.Net.Http;

namespace OAuth2Example.Auth.Filters
{
    public class OAuth2AuthenticationFilter : IAutofacActionFilter
    {
        private readonly OAuth2AuthService authService;
        private readonly IService<AuthToken> tokenService;

        public OAuth2AuthenticationFilter(IHttpAuthService authService, IService<AuthToken> tokenService)
        {
            this.authService = (OAuth2AuthService)authService ?? throw new ArgumentNullException(nameof(authService));
            this.tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        public async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            authService.Controller = (ApiController)actionContext.ControllerContext.Controller;

            string token = actionContext.Request.Headers.Authorization?.Parameter;
            if (string.IsNullOrEmpty(token))
            {
                CheckUnauthorized(actionContext);
            }

            DateTime now = DateTime.UtcNow;

            AuthToken authToken = tokenService.Get(t => t.Token == token && t.CreatedAt <= now && t.ExpiresAt >= now);

            if (authToken != null)
            {
                authService.User = authToken.User;
                authToken.ExpiresAt = now.AddDays(1);

                tokenService.Update(authToken);
                await tokenService.SaveAsync();
            }

            CheckUnauthorized(actionContext);
        }

        private void CheckUnauthorized(HttpActionContext actionContext)
        {
            bool hasLogin = actionContext.ActionDescriptor.GetCustomAttributes<Attributes.LoginAttribute>(true).Count != 0;

            if (hasLogin)
            {
                if (authService.User == null)
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }

        public Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.Run(() => { });
        }
    }
}
