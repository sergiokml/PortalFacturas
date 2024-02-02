using System.Net;
using System.Security.Cryptography.X509Certificates;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Cve.Impuestos.Infraestructure;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cve.Impuestos.Extensions
{
    public static class ImpuestosExtension
    {
        public static IHostBuilder ImpuestosBuild(this IHostBuilder host)
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
                    _ = services
                        .AddHttpClient(
                            "SII_WEB",
                            client =>
                            {
                                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
                            }
                        )
                        .ConfigurePrimaryHttpMessageHandler(() =>
                        {
                            X509Certificate2 x509 = GetCertFromPc(
                                config.Configuration.GetSection("Certificado:Rut").Value!
                            );
                            CookieContainer? cookieContainer = new();
                            HttpClientHandler? handler =
                                new() { CookieContainer = cookieContainer };
                            handler.ClientCertificates.Add(x509);
                            return handler;
                        });
                    _ = services.AddHttpClient("SII_REST");
                    _ = services.AddScoped<IRepositoryBaseWeb, RepositoryBaseWeb>();
                    _ = services.AddScoped<IRepositoryBaseRest, RepositoryBaseRest>();
                    _ = services.AddScoped<ImpuestosInit>();
                }
            );
            return host;
        }

        public static X509Certificate2 GetCertFromPc(string rut)
        {
            X509Store store = new(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            var cert = store.Certificates
                .Where(c => c.Subject.Contains(rut) && c.NotAfter > DateTime.Now)
                .FirstOrDefault();
            store.Close();
            if (cert == null)
            {
                throw new Exception($"No existe certificado válido para este rut: {rut}.");
            }
            return cert!;
        }
    }
}
