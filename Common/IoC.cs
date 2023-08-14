using Autofac;

namespace Common.IoC
{
    public class IoC
    {
        public static ILifetimeScope Container { private get; set; } = null!;
    }
}
