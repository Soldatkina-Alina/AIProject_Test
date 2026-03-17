using McpServer.Services;
using McpServer.Tools;
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
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/mcp-server-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Запуск MCP сервера");
                CreateHostBuilder(args).Build().Run();
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
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
}