using System;
using System.Collections.Generic;
using NHibernate;

namespace YetAnotherUtilsLib.Core.NHibernate
{
    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey>
    {
        protected ISession CurrentSession
        {
            get
            {
                return UnitOfWorkFactory.CurrentUnitOfWork.CurrentSession;
            }
        }

        public TEntity Get(TKey id)
        {
            return CurrentSession.Get<TEntity>(id);
        }

        public IList<TEntity> GetAll()
        { 
            return CurrentSession.CreateCriteria(typeof(TEntity))
                .List<TEntity>();
        }

        public void Insert(TEntity entity)
        {
            CurrentSession.Save(entity);
        }

        public void Update(TEntity entity)
        {
            CurrentSession.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            CurrentSession.Delete(entity);
        }
    }
}