using System;
using NHibernate;
using YetAnotherUtilsLib.Core.Common;
using YetAnotherUtilsLib.Core.Container;

namespace YetAnotherUtilsLib.Core.NHibernate
{
    public interface IUnitOfWork : IDisposable
    {
        ISession CurrentSession { get; }

        void Commit();
        void Rollback();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private const string UNIT_OF_WORK_KEY = "UnitOfWork.CurrentUnitOfWork";

        public static IUnitOfWork CurrentUnitOfWork
        {
            get { return WebSafeCallContext.GetData(UNIT_OF_WORK_KEY) as IUnitOfWork; }
            set { WebSafeCallContext.SetData(UNIT_OF_WORK_KEY, value); }
        }

        public ISession CurrentSession { get; protected set; }
        public ITransaction CurrentTransaction { get; protected set; }

        public static IUnitOfWork Start()
        {
            var factory = IoC.Resolve<INHibernateSessionFactoryBuilder>();

            var session = factory.BuildSessionFactory().OpenSession();
            session.FlushMode = FlushMode.Commit;

            CurrentUnitOfWork = new UnitOfWork
            {
                CurrentSession = session,
                CurrentTransaction = session.BeginTransaction()
            };

            return CurrentUnitOfWork;
        }

        public void Commit()
        {
            if (CurrentTransaction.IsActive)
                CurrentTransaction.Commit();
        }

        public void Rollback()
        {
            if (CurrentTransaction.IsActive)
                CurrentTransaction.Rollback();
        }

        public void Dispose()
        {
            if (CurrentSession != null)
            {
                CurrentSession.Close();
                CurrentSession.Dispose();
            }

            if (CurrentTransaction != null)
                CurrentTransaction.Dispose();

            CurrentUnitOfWork = null;
        }

        public static void SetGlobalUnitOfWork(IUnitOfWork current)
        {
            CurrentUnitOfWork = current;
        }
    }
}