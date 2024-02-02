using Cve.Notificacion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortalFacturas.Interfaces;
using PortalFacturas.Services;
using System;

namespace PortalFacturas;

public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddScoped<GraphService>();
        //services.AddScoped<CoordinadorInit>();

        // CLIENTE PDF
        services.AddHttpClient<IConvertToPdfService, ConvertToPdfService>(
            c =>
                c.BaseAddress = new Uri(
                    configuration.GetConnectionString("EndPointApiConvertToPdf") ?? ""
                )
        );
        // COOKIES
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
            // app.UseExceptionHandler("/Index");
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
                if (ctx.Response.StatusCode == 500)
                {
                    ctx.Request.Path = "/Index";
                    await next();
                }
                if (ctx.Response.StatusCode == 503)
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
