using System.Collections.Generic;

namespace YetAnotherUtilsLib.Core.NHibernate
{
    public interface IRepository<TEntity, TKey>
    {
        TEntity Get(TKey id);
        IList<TEntity> GetAll();
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}