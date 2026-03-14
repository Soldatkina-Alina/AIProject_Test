using McpServer.Services;
using McpServer.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
                client.BaseAddress = new System.Uri(Configuration["PromoCodeFactoryApi:BaseUrl"]);
                client.Timeout = System.TimeSpan.FromSeconds(int.Parse(Configuration["PromoCodeFactoryApi:Timeout"]));
            });

            services.AddTransient<IPromoCodeFactoryApiClient, PromoCodeFactoryApiClient>();
            services.AddTransient<CreateCustomerTool>();
            services.AddTransient<GetCustomerTool>();
            services.AddTransient<UpdateCustomerTool>();
            services.AddTransient<DeleteCustomerTool>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}