using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;

namespace Cve.Coordinador.Infraestructure;

public static class CoordinadorExtension
{
    public static IHostBuilder CoordinadorBuild(this IHostBuilder host)
    {
        _ = host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        _ = host.ConfigureContainer<ContainerBuilder>(
            (config, builder) =>
            {
                _ = builder.RegisterModule(new ContainerModule());
            }
        );
        _ = host.ConfigureServices(
            (config, services) =>
            {
                string server = config.Configuration.GetSection("CENConfig:UrlServer").Value!;
                _ = services.AddHttpClient("CEN", client => client.BaseAddress = new Uri(server));
                _ = services.AddScoped<IRepositoryBase, RepositoryBase>();
                //_ = services.AddScoped<CoordinadorInit>();
                _ = services
                    .AddRefitClient<IGitHubApi>()
                    .ConfigureHttpClient(c =>
                        c.BaseAddress = new Uri("https://ppagos-sen.coordinador.cl/")
                    );
            }
        );
        return host;
    }

    public static async Task EnsureSuccess(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }
        string? content = await response.Content.ReadAsStringAsync();
        response.Content?.Dispose();
        throw new HttpRequestException(content, null, response.StatusCode);
    }
}
