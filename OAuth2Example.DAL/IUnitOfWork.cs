using OAuth2Example.DAL.Models;
using System;
using System.Threading.Tasks;

namespace OAuth2Example.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TModel> GetRepo<TModel>() where TModel : BaseModel;
        void Commit();
        Task CommitAsync();
    }
}
