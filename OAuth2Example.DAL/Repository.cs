using OAuth2Example.DAL.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;

namespace OAuth2Example.DAL
{
    internal class Repository<TModel> : IRepository<TModel> where TModel : BaseModel
    {
        private readonly DbSet<TModel> dbSet;
        private readonly DbContext context;

        public Repository(DbSet<TModel> dbSet, DbContext context)
        {
            this.dbSet = dbSet ?? throw new ArgumentNullException(nameof(dbSet));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Insert(TModel model)
        {
            dbSet.Add(model);
        }

        public void Update(TModel model)
        {
            context.Entry(model).State = EntityState.Modified;
        }

        public void Delete(TModel model)
        {
            dbSet.Remove(model);
        }

        public TModel Get(Expression<Func<TModel, bool>> filter)
        {
            return dbSet.FirstOrDefault(filter);
        }

        public async Task<TModel> GetAsync(Expression<Func<TModel, bool>> filter)
        {
            return await dbSet.FirstOrDefaultAsync(filter);
        }

        public IQueryable<TModel> GetAll(Expression<Func<TModel, bool>> filter = null)
        {
            return filter == null ? dbSet : dbSet.Where(filter);
        }
    }
}
