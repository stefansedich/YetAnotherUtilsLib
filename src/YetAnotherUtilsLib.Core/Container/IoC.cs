using Castle.Windsor;

namespace YetAnotherUtilsLib.Core.Container
{
    public class IoC
    {
        public static IWindsorContainer Container { get; protected set; }

        public static void InitializeWith(IWindsorContainer container)
        {
            Container = container;
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}