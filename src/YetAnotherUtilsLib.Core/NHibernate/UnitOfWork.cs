using NHibernate;
using YetAnotherUtilsLib.Core.Container;

namespace YetAnotherUtilsLib.Core.NHibernate
{
    public class UnitOfWork : IUnitOfWork
    {
        public ISession CurrentSession { get; protected set; }
        public ITransaction CurrentTransaction { get; protected set; }

        public UnitOfWork(ISession currentSession, ITransaction currentTransaction)
        {
            CurrentSession = currentSession;
            CurrentTransaction = currentTransaction;
        }

        public static IUnitOfWork Start()
        {
            return IoC.Resolve<IUnitOfWorkFactory>()
                .Create();
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

            UnitOfWorkFactory.CloseCurrentUnitOfWork();
        }
    }
}