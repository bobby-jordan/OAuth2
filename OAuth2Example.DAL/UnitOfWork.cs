using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OAuth2Example.DAL.Models;
using System.Data.Entity;

namespace OAuth2Example.DAL
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        private bool disposed;

        public UnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public IRepository<TModel> GetRepo<TModel>() where TModel : BaseModel
        {
            Type modelType = typeof(TModel);

            if (!repositories.ContainsKey(modelType))
            {
                repositories.Add(modelType, new Repository<TModel>(dbContext.Set<TModel>(), dbContext));
            }

            return (IRepository<TModel>)repositories[modelType];
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposed || !disposing) return;

            dbContext.Dispose();
            disposed = true;
        }
    }
}
