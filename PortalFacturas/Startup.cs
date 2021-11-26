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
        public OptionsModel options = new OptionsModel();

        public IConfiguration Configuration { get; }
        public IServiceCollection _services { get; set; }

        private IWebHostEnvironment CurrentEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;
            services.AddRazorPages();
            // Cliente Cen
            services.AddHttpClient<IApiCenService, ApiCenService>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetConnectionString("EndPointApiCen") ?? "");
            });
            // Cliente Sharepoint
            services.AddHttpClient<ISharePointService, SharePointService>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetConnectionString("EndPointOtro") ?? "");
            });
            if (CurrentEnvironment.IsDevelopment())
            {
                // Cliente Function Azure
                services.AddHttpClient<IXslMapperFunctionService, XslMapperFunctionService>(c =>
                {
                    c.BaseAddress = new Uri(Configuration.GetConnectionString("EndPointFunctionDev") ?? "");
                });
            }
            else
            {
                // Cliente Function Azure
                _services.AddHttpClient<IXslMapperFunctionService, XslMapperFunctionService>(c =>
                {
                    c.BaseAddress = new Uri(Configuration.GetConnectionString("EndPointFunctionProd") ?? "");
                });
            }


            // Cliente Restack.IO
            services.AddHttpClient<IConvertToPdfService, ConvertToPdfService>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetConnectionString("EndPointApiConvertToPdf") ?? "");
            });


            services.AddSession();

            services.AddSingleton(options);
            //services.AddSingleton<IndexModel>();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Configuration.Bind("Values", options);
            if (env.IsDevelopment())
            {
                // Cliente Function Azure
                //_services.AddHttpClient<IXslMapperFunctionService, XslMapperFunctionService>(c =>
                //{
                //    c.BaseAddress = new Uri(Configuration.GetConnectionString("EndPointFunctionDev") ?? "");
                //});
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Cliente Function Azure
                //_services.AddHttpClient<IXslMapperFunctionService, XslMapperFunctionService>(c =>
                //{
                //    c.BaseAddress = new Uri(Configuration.GetConnectionString("EndPointFunctionProd") ?? "");
                //});
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }

}
