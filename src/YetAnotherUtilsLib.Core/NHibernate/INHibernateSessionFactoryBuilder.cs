using NHibernate;

namespace YetAnotherUtilsLib.Core.NHibernate
{
    public interface INHibernateSessionFactoryBuilder
    {
        ISessionFactory BuildSessionFactory(); 
    }
}