using System.Reflection;

using Autofac;

namespace Cve.Coordinador.Infraestructure
{
    public class ContainerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly? assembly = Assembly.GetExecutingAssembly();
            //builder
            //    .RegisterAssemblyTypes(assembly)
            //    .Where(t => t.Name.EndsWith("Repo"))
            //    .AsImplementedInterfaces();
            _ = builder
                .RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }
    }
}
