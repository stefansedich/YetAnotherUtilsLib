using System;
using NHibernate;
using YetAnotherUtilsLib.Core.Common;

namespace YetAnotherUtilsLib.Core.NHibernate
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private const string UNIT_OF_WORK_KEY = "UnitOfWorkFactory.CurrentUnitOfWork";

        private readonly INHibernateSessionFactoryBuilder _sessionFactoryBuilder;

        internal static IUnitOfWork InternalCurrentUnitOfWork
        {
            get { return WebSafeCallContext.GetData(UNIT_OF_WORK_KEY) as IUnitOfWork; }
            set { WebSafeCallContext.SetData(UNIT_OF_WORK_KEY, value); }
        }

        public static IUnitOfWork CurrentUnitOfWork
        {
            get
            {
                if(InternalCurrentUnitOfWork == null)
                    throw new InvalidOperationException("You are not currently in a UnitOfWork");

                return InternalCurrentUnitOfWork;
            }
        }

        public UnitOfWorkFactory(INHibernateSessionFactoryBuilder sessionFactoryBuilder)
        {
            _sessionFactoryBuilder = sessionFactoryBuilder;
        }
        
        public IUnitOfWork Create()
        {
            if (InternalCurrentUnitOfWork != null)
                throw new InvalidOperationException("Cannot create a UnitOfWork while another is running");

            var session = _sessionFactoryBuilder.BuildSessionFactory().OpenSession();
            session.FlushMode = FlushMode.Commit;

            return (InternalCurrentUnitOfWork = new UnitOfWork(session, session.BeginTransaction()));
        }

        public static void SetGlobalUnitOfWork(IUnitOfWork current)
        {
            InternalCurrentUnitOfWork = current;
        }

        public static void CloseCurrentUnitOfWork()
        {
            InternalCurrentUnitOfWork = null;
        }
    }
}