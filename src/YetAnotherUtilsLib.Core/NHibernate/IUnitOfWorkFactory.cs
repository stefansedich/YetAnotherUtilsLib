namespace YetAnotherUtilsLib.Core.NHibernate
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}