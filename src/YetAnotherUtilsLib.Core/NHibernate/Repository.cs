using NHibernate;

namespace YetAnotherUtilsLib.Core.NHibernate
{
    public interface IRepository<TEntity, TKey>
    {
        TEntity Get(TKey id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }

    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    {
        protected ISession CurrentSession
        {
            get { return UnitOfWork.CurrentUnitOfWork.CurrentSession; }
        }

        public TEntity Get(TKey id)
        {
            return CurrentSession.Get<TEntity>(id);
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