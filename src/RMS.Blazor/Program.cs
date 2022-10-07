using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RMS.Currencies;
using RMS.Remittances;
using Serilog;
using Serilog.Events;
namespace RMS.Blazor
{

    public class Program
    {
        [Obsolete]
        public async static Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File("Logs/logs.txt"))
                .WriteTo.Async(c => c.Console())
                .CreateLogger();

            try
            {
           

                Log.Information("Starting web host.");
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.AddAppSettingsSecretsJson()
                    .UseAutofac()
                    .UseSerilog();
                await builder.AddApplicationAsync<RMSBlazorModule>();


                //builder.Services.AddRazorPages();
                //builder.Services.AddServerSideBlazor();
                //builder.Services.AddSingleton<CustomerAppService>();
                //builder.Services.AddSingleton<CurrencyAppService>();
                //builder.Services.AddSingleton<RemittanceAppService>();

                builder.Services.AddOptions();
                builder.Services.AddAuthorizationCore();


                var app = builder.Build();
                await app.InitializeApplicationAsync();
                await app.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}