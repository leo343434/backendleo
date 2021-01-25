using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Backend.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            try
            {
                Log.Information("Iniciando backend");
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception err)
            {
                Log.Fatal(err, "No es posible iniciar el servicio");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}