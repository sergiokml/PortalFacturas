using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PortalFacturas.Models;
using PortalFacturas.Services;

using System;

namespace PortalFacturas
{
    public class Startup
    {

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment currentEnvironmen;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            currentEnvironmen = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            // Cliente Cen
            services.AddHttpClient<IApiCenService, ApiCenService>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetConnectionString("EndPointApiCen") ?? "");
            });
            // Cliente Sharepoint
            services.AddHttpClient<ISharePointService, SharePointService>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetConnectionString("EndPointSharepoint") ?? "");
            });
            if (currentEnvironmen.IsDevelopment())
            {
                // Cliente Function Azure
                services.AddHttpClient<IXslMapperFunctionService, XslMapperFunctionService>(c =>
                {
                    c.BaseAddress = new Uri(configuration.GetConnectionString("EndPointFunctionDev") ?? "");
                });
            }
            else
            {
                // Cliente Function Azure
                services.AddHttpClient<IXslMapperFunctionService, XslMapperFunctionService>(c =>
                {
                    c.BaseAddress = new Uri(configuration.GetConnectionString("EndPointFunctionProd") ?? "");
                });
            }


            // Cliente Restack.IO
            services.AddHttpClient<IConvertToPdfService, ConvertToPdfService>(c =>
            {
                c.BaseAddress = new Uri(configuration.GetConnectionString("EndPointApiConvertToPdf") ?? "");
            });

            services.Configure<OptionsModel>(configuration);


            services.AddAuthentication("appcookie")
                .AddCookie("appcookie", options =>
                {
                    options.LoginPath = "/Index";
                });

            services.AddSession();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }

}
