using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddHttpClient<IApiCenService, ApiCenService>(
                c =>
                {
                    c.BaseAddress = new Uri(
                        configuration.GetConnectionString("EndPointApiCen") ?? ""
                    );
                }
            );
            // Cliente Sharepoint
            services.AddHttpClient<ISharePointService, SharePointService>(
                c =>
                {
                    c.BaseAddress = new Uri(
                        configuration.GetConnectionString("EndPointSharepoint") ?? ""
                    );
                }
            );

            // Cliente Pdf
            services.AddHttpClient<IConvertToPdfService, ConvertToPdfService>();
            services.Configure<AppSettings>(configuration);
            services
                .AddAuthentication("appcookie")
                .AddCookie(
                    "appcookie",
                    options =>
                    {
                        options.LoginPath = "/Index";
                    }
                );
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
            app.Use(
                async (ctx, next) =>
                {
                    await next();

                    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                    {
                        ctx.Request.Path = "/Error";
                        await next();
                    }
                }
            );
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapRazorPages();
                }
            );
        }
    }
}
