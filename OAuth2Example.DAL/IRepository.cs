using OAuth2Example.DAL.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OAuth2Example.DAL
{
    public interface IRepository<TModel> where TModel : BaseModel
    {
        void Insert(TModel model);
        void Update(TModel model);
        void Delete(TModel model);

        TModel Get(Expression<Func<TModel, bool>> filter = null);
        Task<TModel> GetAsync(Expression<Func<TModel, bool>> filter = null);
        IQueryable<TModel> GetAll(Expression<Func<TModel, bool>> filter = null);
    }
}
