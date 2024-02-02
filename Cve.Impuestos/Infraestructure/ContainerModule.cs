using System.Reflection;

using Autofac;

namespace Cve.Impuestos.Infraestructure
{
    public class ContainerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly? assembly = Assembly.GetExecutingAssembly();
            _ = builder
                .RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }
    }
}
