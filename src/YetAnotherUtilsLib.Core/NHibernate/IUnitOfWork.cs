using System;
using NHibernate;

namespace YetAnotherUtilsLib.Core.NHibernate
{
    public interface IUnitOfWork : IDisposable
    {
        ISession CurrentSession { get; }
        ITransaction CurrentTransaction { get; }

        void Commit();
        void Rollback();
    }
}