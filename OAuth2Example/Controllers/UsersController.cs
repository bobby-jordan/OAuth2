using OAuth2Example.BindModels;
using OAuth2Example.DAL.Models;
using System;
using System.Linq;
using System.Web.Http;
using OAuth2Example.DataServices;
using OAuth2Example.Auth;
using OAuth2Example.Auth.Attributes;
using System.Threading.Tasks;
using AutoMapper;
using System.Web.OData;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;

namespace OAuth2Example.Controllers
{
    [Route("api/1.0/users")]
    [RoutePrefix("api/1.0/users")]
    public class UsersController : CrudController<User, UserBindModel>
    {
        public UsersController(IService<User> service, IHttpAuthService authService) : base(service, authService)
        {
        }

        /// <summary>
        /// Returns a list of all users
        /// </summary>
        [Login]
        [SwaggerResponse(200, Type = typeof(IEnumerable<UserBindModel>))]
        public override IHttpActionResult Get()
        {
            return base.Get();
        }

        /// <summary>
        /// Returns information for a given user
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <response code="200">The request completed successfully</response>
        /// <response code="404">The user was not found</response>
        /// <response code="401">You are not authorized to use this endpoint</response>
        [Login]
        [SwaggerResponse(200, Type = typeof(UserBindModel))]
        public override IHttpActionResult Get(int id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <response code="200">The user was successfully deleted</response>
        /// <response code="404">The user was not found</response>
        /// <response code="401">You are not authorized to use this endpoint</response>
        [Login]
        public override Task<IHttpActionResult> Delete(int id)
        {
            return base.Delete(id);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="bindModel">Contains the user information</param>
        /// <response code="201">The user was created successfully. URL to info is in the Location header</response>
        /// <response code="400">The user data is invalid or missing</response>
        /// <response code="422">There was an error validation user data. See response for details</response>
        [SwaggerResponse(201, Type = typeof(UserBindModel))]
        public override Task<IHttpActionResult> Post(UserBindModel bindModel)
        {
            return base.Post(bindModel);
        }

        /// <summary>
        /// Edits a user
        /// </summary>
        /// <param name="id">The user ID</param>
        /// <param name="bindModel">Contains the new user data</param>
        /// <response code="200">The user was edited successfully</response>
        /// <response code="400">The user data is invalid or missing</response>
        /// <response code="422">There was an error validation user data. See response for details</response>
        [Login]
        public override async Task<IHttpActionResult> Patch(int id, Delta<UserBindModel> bindModel)
        {
            if (bindModel == null)
            {
                return BadRequest();
            }

            try
            {
                User user = Service.Get(u => u.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                string newPassword = "";

                if (bindModel.TryGetPropertyValue("new_password", out object newPass))
                {
                    newPassword = newPass as string;
                }

                UserBindModel temp = Mapper.Map<User, UserBindModel>(user);
                bindModel.Patch(temp);
                Mapper.Map(temp, user, opts => opts.BeforeMap((bm, m) => bm.password = m.Password));
                user.Id = id;

                if(!string.IsNullOrEmpty(newPassword))
                {
                    user.Password = newPassword;
                    user.Salt = null;
                }

                if (!Service.Update(user))
                {
                    return CreateResultFromModelState();
                }

                try
                {
                    await Service.SaveAsync();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    return StatusCodeWithJson(422, new
                    {
                        errors = e.EntityValidationErrors.First().ValidationErrors.GroupBy(err => err.PropertyName.ToLowerInvariant()).Select(err =>
                            new
                            {
                                key = err.Key,
                                values = err.Select(error => error.ErrorMessage)
                            }
                        )
                    });
                }

                return Ok();
            }
            catch(UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
