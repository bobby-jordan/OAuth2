using OAuth2Example.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OAuth2Example.DataServices
{
    public interface IService<TModel> : IDisposable where TModel : BaseModel
    {
        bool Insert(TModel model);
        bool Update(TModel model);
        bool Delete(TModel model);

        TModel Get(Expression<Func<TModel, bool>> filter = null);
        IEnumerable<TModel> GetAll(Expression<Func<TModel, bool>> filter = null);

        void Save();
        Task SaveAsync();

        IValidationDictionary ValidationDictionary { get; set; }
        IAuthService AuthService { get; set; }
    }
}
