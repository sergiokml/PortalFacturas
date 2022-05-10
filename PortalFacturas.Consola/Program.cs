//using System;
//using System.IO;
//using System.Threading.Tasks;

//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//using PortalFacturas.Consola.Services;

//namespace PortalFacturas.Consola
//{
//    internal class Program
//    {
//        private static async Task Main(string[] args)
//        {
//            IHost host = CreateDefaultBuilder().Build();
//            using IServiceScope serviceScope = host.Services.CreateScope();
//            IServiceProvider provider = serviceScope.ServiceProvider;
//            Start workerInstance = provider.GetRequiredService<Start>();
//            await workerInstance.DoWorkAsync();
//            await host.StartAsync();
//            // await host.WaitForShutdownAsync();
//        }

//        private static IHostBuilder CreateDefaultBuilder()
//        {
//            return Host.CreateDefaultBuilder()
//                .ConfigureHostConfiguration(
//                    hostConfig =>
//                    {
//                        hostConfig.SetBasePath(Directory.GetCurrentDirectory());
//                        hostConfig.AddJsonFile("hostsettings.json", optional: true);
//                        //hostConfig.AddEnvironmentVariables(prefix: "PREFIX_");
//                        //hostConfig.AddCommandLine(args);
//                    }
//                )
//                .ConfigureAppConfiguration(
//                    app =>
//                    {
//                        app.AddJsonFile("appsettings.json");
//                    }
//                )
//                .ConfigureServices(
//                    services =>
//                    {
//                        services.AddSingleton<Start>();
//                        services.AddHttpClient<ISharePointService, SharePointService>();

//                        // services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
//                    }
//                )
//                .ConfigureServices(
//                    (hostContext, services) =>
//                    {
//                        services.Configure<AppSettings>(
//                            hostContext.Configuration.GetSection(AppSettings.Variables)
//                        );
//                        //services.AddDbContext<SoftlandContext>(
//                        //    options =>
//                        //    {
//                        //        options.UseSqlServer(
//                        //            hostContext.Configuration.GetConnectionString("Database1")
//                        //        );
//                        //    }
//                        //);
//                    }
//                );
//        }
//    }
//}
