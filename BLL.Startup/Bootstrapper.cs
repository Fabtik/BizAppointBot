using Autofac;
using System.Reflection;

namespace BLL.Startup
{
    public static class Bootstrapper
    {
        public static void Bootstrap(ContainerBuilder containerBuilder)
        {
            DAL.Startup.Bootstrapper.Bootstrap(containerBuilder);
            RegisterServices(containerBuilder);
        }

        private static void RegisterServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(Assembly.Load("BLL"))
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
