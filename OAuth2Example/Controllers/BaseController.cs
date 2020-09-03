using OAuth2Example.Auth;
using OAuth2Example.DAL.Models;
using OAuth2Example.DataServices;
using OAuth2Example.Utils;
using System;
using System.Web.Http;

namespace OAuth2Example.Controllers
{
    public abstract class BaseController<TModel> : ApiController where TModel : BaseModel
    {
        private readonly IService<TModel> service;
        private readonly IHttpAuthService authService;

        private bool disposed;

        protected IService<TModel> Service => service;
        protected IHttpAuthService AuthService => authService;

        public BaseController(IService<TModel> service, IHttpAuthService authService)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));

            this.service.ValidationDictionary = new ModelStateWrapper(ModelState);
            this.service.AuthService = authService;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposed || disposing) return;

            service.Dispose();
            disposed = true;
        }

        protected StatusCodeWithJsonResult StatusCodeWithJson<TResult>(int statusCode, TResult obj)
        {
            return new StatusCodeWithJsonResult(statusCode, obj, Request);
        }
    }
}
