
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PortalFacturas.Interfaces;
using PortalFacturas.Models;
using PortalFacturas.Pages;
using PortalFacturas.Services;

using System;

namespace PortalFacturas
{
    public class Startup
    {
        public OptionsModel options = new OptionsModel();

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddHttpClient<IApiCenService, ApiCenService>(c =>
            {
                c.BaseAddress = new Uri(Configuration.GetConnectionString("EndPointCen") ?? "");
            });
            services.AddSingleton(options);
            services.AddSingleton<IndexModel>();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                Configuration.Bind("AppOptionsLocal", options);
                app.UseDeveloperExceptionPage();
            }
            else
            {
                Configuration.Bind("AppOptionsProd", options);
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
