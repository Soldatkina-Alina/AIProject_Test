using McpServer.Services;
using McpServer.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace McpServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpClient("PromoCodeFactoryApi", client =>
            {
                client.BaseAddress = new System.Uri(Configuration["PromoCodeFactoryApi:BaseUrl"] ?? "http://localhost:5001");
                int timeoutSeconds = 30;
                if (!string.IsNullOrEmpty(Configuration["PromoCodeFactoryApi:Timeout"]))
                {
                    int.TryParse(Configuration["PromoCodeFactoryApi:Timeout"], out timeoutSeconds);
                }
                client.Timeout = System.TimeSpan.FromSeconds(timeoutSeconds);
            });

            services.AddTransient<IPromoCodeFactoryApiClient, PromoCodeFactoryApiClient>();
            services.AddTransient<CreateCustomerTool>();
            services.AddTransient<GetCustomerTool>();
            services.AddTransient<GetAllCustomersTool>();
            services.AddTransient<UpdateCustomerTool>();
            services.AddTransient<DeleteCustomerTool>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("API is running");
                });
            });
        }
    }
}