using OAuth2Example.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using OAuth2Example.DAL;
using System.Linq;

namespace OAuth2Example.DataServices
{
    internal abstract class BaseService<TModel> : IService<TModel> where TModel : BaseModel
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IRepository<TModel> repository;

        private bool disposed;

        public IValidationDictionary ValidationDictionary { get; set; }
        public IAuthService AuthService { get; set; }

        public BaseService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            repository = unitOfWork.GetRepo<TModel>();
        }

        #region Insert

        protected virtual bool OnBeforeInsert(TModel model)
        {
            return true;
        }

        public bool Insert(TModel model)
        {
            return RunOperation(model, repository.Insert, OnBeforeInsert, OnAfterInsert);
        }

        protected virtual void OnAfterInsert(TModel model)
        { 
        }

        #endregion // Insert

        #region Update

        protected virtual bool OnBeforeUpdate(TModel model)
        {
            return true;
        }

        public bool Update(TModel model)
        {
            return RunOperation(model, repository.Update, OnBeforeUpdate, OnAfterUpdate);
        }

        protected virtual void OnAfterUpdate(TModel model)
        {
        }

        #endregion // Update

        #region Delete

        protected virtual bool OnBeforeDelete(TModel model)
        {
            return true;
        }

        public bool Delete(TModel model)
        {
            return RunOperation(model, repository.Delete, OnBeforeDelete, OnAfterDelete);
        }

        protected virtual void OnAfterDelete(TModel model)
        {
        }

        #endregion // Delete

        #region Get

        public TModel Get(Expression<Func<TModel, bool>> filter = null)
        {
            return OnAfterGet(repository.Get(filter));
        }

        protected virtual TModel OnAfterGet(TModel model)
        {
            return model;
        }

        public IEnumerable<TModel> GetAll(Expression<Func<TModel, bool>> filter = null)
        {
            return OnAfterGetAll(repository.GetAll(filter));
        }

        protected virtual IEnumerable<TModel> OnAfterGetAll(IQueryable<TModel> models)
        {
            return models.AsEnumerable();
        }

        #endregion // Get

        #region Save

        public void Save()
        {
            unitOfWork.Commit();
        }

        public async Task SaveAsync()
        {
            await unitOfWork.CommitAsync();
        }

        #endregion // Save

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposed || !disposing) return;

            unitOfWork.Dispose();
            disposed = true;
        }

        #endregion // Dispose

        protected abstract bool Validate(TModel model);

        private bool RunOperation(TModel model, Action<TModel> repoAction, Func<TModel, bool> onBefore, Action<TModel> onAfter)
        {
            if (!Validate(model))
            {
                return false;
            }

            if (!onBefore(model))
            {
                return false;
            }

            repoAction(model);

            onAfter(model);

            return true;
        }

        private void CheckModelValid()
        {
            if (!ValidationDictionary.IsValid)
            {
                throw new InvalidOperationException("There are model errors, can not save");
            }
        }
    }
}
