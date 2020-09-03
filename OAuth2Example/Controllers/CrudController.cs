using OAuth2Example.DAL.Models;
using OAuth2Example.DataServices;
using OAuth2Example.BindModels;
using System;
using System.Web.Http;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using System.Net;
using OAuth2Example.Auth;
using System.Web.OData;

namespace OAuth2Example.Controllers
{
    public abstract class CrudController<TModel, TBindModel> : BaseController<TModel>
        where TModel : BaseModel
        where TBindModel : BaseBindModel
    {
        public CrudController(IService<TModel> service, IHttpAuthService authService) : base(service, authService)
        {
        }

        [HttpGet]
        [Route("")]
        public virtual IHttpActionResult Get()
        {
            try
            {
                return Ok(Service.GetAll().Select(m => Mapper.Map<TModel, TBindModel>(m)));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public virtual IHttpActionResult Get(int id)
        {
            try
            {
                TModel result = Service.Get(m => m.Id == id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(Mapper.Map<TModel, TBindModel>(result));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route]
        public virtual async Task<IHttpActionResult> Post(TBindModel bindModel)
        {
            try
            {
                if (bindModel == null)
                {
                    return BadRequest();
                }

                TModel model = Mapper.Map<TBindModel, TModel>(bindModel);

                if (!Service.Insert(model))
                {
                    return CreateResultFromModelState();
                }

                try
                {
                    await Service.SaveAsync();
                }
                catch(System.Data.Entity.Validation.DbEntityValidationException e)
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

                string domain = Request.RequestUri.Scheme + System.Uri.SchemeDelimiter + Request.RequestUri.Host +
                            (Request.RequestUri.IsDefaultPort ? "" : ":" + Request.RequestUri.Port) + Request.RequestUri.AbsolutePath + "/" + model.Id;

                return Created(domain, Mapper.Map<TModel, TBindModel>(model));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public virtual async Task<IHttpActionResult> Patch(int id, Delta<TBindModel> bindModel)
        {
            try
            {
                if (bindModel == null)
                {
                    return BadRequest();
                }

                TModel model = Service.Get(m => m.Id == id);

                if (model == null)
                {
                    return NotFound();
                }

                TBindModel temp = Mapper.Map<TModel, TBindModel>(model);
                bindModel.Patch(temp);
                Mapper.Map(temp, model);

                model.Id = id;

                if (!Service.Update(model))
                {
                    return CreateResultFromModelState();
                }

                await Service.SaveAsync();

                return Ok();
            }
            catch (AutoMapperMappingException)
            {
                return BadRequest();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                TModel model = Service.Get(m => m.Id == id);

                if (model == null)
                {
                    NotFound();
                }

                Service.Delete(model);

                await Service.SaveAsync();

                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        protected IHttpActionResult CreateResultFromModelState()
        {
            if (!ModelState.IsValid)
                return StatusCodeWithJson(422, new { errors = ModelState.Select(kvp => new { key = kvp.Key, values = kvp.Value.Errors.Select(e => e.ErrorMessage) }) });

            return StatusCode((HttpStatusCode)422);
        }
    }
}
