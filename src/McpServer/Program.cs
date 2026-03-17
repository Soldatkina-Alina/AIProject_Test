using McpServer.Services;
using McpServer.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Serilog;
using System;

namespace McpServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Настраиваем логирование
            bool isMcpMode = args.Length > 0 && (args[0] == "--mcp" || args[0] == "-m");
            
            if (isMcpMode)
            {
                // В MCP режиме отключаем логирование в консоль, чтобы не разрывать JSON протокол
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Warning()
                    .WriteTo.File("logs/mcp-server.log", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
            }
            else
            {
                // В Web API режиме логируем в консоль и файл
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("logs/mcp-server.log", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
            }

            try
            {
                if (isMcpMode)
                {
                    // Режим MCP с stdio транспортом (без веб-API)
                    CreateMcpHostBuilder(args).Build().Run();
                }
                else
                {
                    // Режим веб-API (HTTP)
                    CreateWebApiHostBuilder(args).Build().Run();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "MCP сервер не запустился");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        // Конфигурация для режима Web API
        public static IHostBuilder CreateWebApiHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddControllers();

                    services.AddHttpClient("PromoCodeFactoryApi", client =>
                    {
                        client.BaseAddress = new System.Uri(hostContext.Configuration["PromoCodeFactoryApi:BaseUrl"] ?? "http://localhost:5001");
                        int timeoutSeconds = 30;
                        if (!string.IsNullOrEmpty(hostContext.Configuration["PromoCodeFactoryApi:Timeout"]))
                        {
                            int.TryParse(hostContext.Configuration["PromoCodeFactoryApi:Timeout"], out timeoutSeconds);
                        }
                        client.Timeout = System.TimeSpan.FromSeconds(timeoutSeconds);
                    });

                    services.AddTransient<IPromoCodeFactoryApiClient, PromoCodeFactoryApiClient>();
                    services.AddTransient<CreateCustomerTool>();
                    services.AddTransient<GetCustomerTool>();
                    services.AddTransient<GetAllCustomersTool>();
                    services.AddTransient<UpdateCustomerTool>();
                    services.AddTransient<DeleteCustomerTool>();
                });

        // Конфигурация для режима MCP (stdio)
        public static IHostBuilder CreateMcpHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient("PromoCodeFactoryApi", client =>
                    {
                        client.BaseAddress = new System.Uri(hostContext.Configuration["PromoCodeFactoryApi:BaseUrl"] ?? "http://promocodefactory:8080");
                        int timeoutSeconds = 30;
                        if (!string.IsNullOrEmpty(hostContext.Configuration["PromoCodeFactoryApi:Timeout"]))
                        {
                            int.TryParse(hostContext.Configuration["PromoCodeFactoryApi:Timeout"], out timeoutSeconds);
                        }
                        client.Timeout = System.TimeSpan.FromSeconds(timeoutSeconds);
                    });

                    services.AddTransient<IPromoCodeFactoryApiClient, PromoCodeFactoryApiClient>();
                    services.AddTransient<CreateCustomerTool>();
                    services.AddTransient<GetCustomerTool>();
                    services.AddTransient<GetAllCustomersTool>();
                    services.AddTransient<UpdateCustomerTool>();
                    services.AddTransient<DeleteCustomerTool>();

                    // Add MCP Server with stdio transport
                    services.AddMcpServer()
                        .WithStdioServerTransport()
                        .WithTools<CreateCustomerTool>()
                        .WithTools<GetCustomerTool>()
                        .WithTools<GetAllCustomersTool>()
                        .WithTools<UpdateCustomerTool>()
                        .WithTools<DeleteCustomerTool>();
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
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
            });
        }
    }
}