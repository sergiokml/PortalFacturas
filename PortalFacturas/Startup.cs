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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
            // Cliente Function Azure
            services.AddHttpClient<IXslMapperFunctionService, XslMapperFunctionService>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetConnectionString("EndPointFunction") ?? "");
            });

            // Cliente Restack.IO
            services.AddHttpClient<IConvertToPdfService, ConvertToPdfService>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetConnectionString("EndPointApiConvertToPdf") ?? "");
            });




            services.AddSingleton(options);
            //services.AddSingleton<IndexModel>();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Configuration.Bind("Values", options);
            if (env.IsDevelopment())
            {
                // SharePoint
                //_services.AddOptions<OptionsModel>().Configure<IConfiguration>((setttings, configuration) =>
                //{
                //    configuration.Bind(setttings); // From host.json              
                //    setttings.UrlFunction = "http://localhost:7071/api/mappers/xml/xml";
                //    //http://xslmapperfunction.azurewebsites.net/api/mappers/xml/xml
                //});
                //Configuration.Bind("Values", options);
                options.UrlFunction = "http://localhost:7071/api/mappers/xml/xml";
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //// SharePoint
                //_services.AddOptions<OptionsModel>().Configure<IConfiguration>((setttings, configuration) =>
                //{
                //    configuration.Bind(setttings); // From host.json              
                //});

                app.UseHsts();
            }
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
