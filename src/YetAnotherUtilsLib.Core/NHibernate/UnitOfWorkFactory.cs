using System;
using NHibernate;
using YetAnotherUtilsLib.Core.Common;

namespace YetAnotherUtilsLib.Core.NHibernate
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private const string UNIT_OF_WORK_KEY = "UnitOfWorkFactory.CurrentUnitOfWork";

        private readonly INHibernateSessionFactoryBuilder _sessionFactoryBuilder;
        
        public static IUnitOfWork CurrentUnitOfWork
        {
            get { return WebSafeCallContext.GetData(UNIT_OF_WORK_KEY) as IUnitOfWork; }
            set { WebSafeCallContext.SetData(UNIT_OF_WORK_KEY, value); }
        }

        public UnitOfWorkFactory(INHibernateSessionFactoryBuilder sessionFactoryBuilder)
        {
            _sessionFactoryBuilder = sessionFactoryBuilder;
        }
        
        public IUnitOfWork Create()
        {
            if(CurrentUnitOfWork != null)
                throw new InvalidOperationException("Cannot create a UnitOfWork while another is running");

            var session = _sessionFactoryBuilder.BuildSessionFactory().OpenSession();
            session.FlushMode = FlushMode.Commit;

            return (CurrentUnitOfWork = new UnitOfWork(session, session.BeginTransaction()));
        }

        public static void SetGlobalUnitOfWork(IUnitOfWork current)
        {
            CurrentUnitOfWork = current;
        }
    }
}