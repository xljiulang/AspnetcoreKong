using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Kong.Aspnetcore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Usage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var kong = new KongOptions
            {
                AdminApi = new Uri("http://localhost:8001"),
                Service = new KongServiceOptions
                {
                    Url = new Uri("http://myservice"),
                    Routes = new[] { new Kong.Aspnetcore.AdminApi.KongRouteEdit {
                         Name="route1"
                    }}
                },
                UpStream = new KongUpStreamOptions
                {
                    Healthchecks = new Kong.Aspnetcore.AdminApi.KongUpstreamEdit.HealthchecksInfo
                    {
                        Active = new Kong.Aspnetcore.AdminApi.KongUpstreamEdit.HealthchecksInfo.ActiveInfo
                        {
                            Http_path = "/healthchecks",
                        }
                    },
                    Targets = new[] { new Kong.Aspnetcore.AdminApi.KongTargetEdit{
                        Target="127.0.0.1:520"
                 }}
                }
            };

            var json = JsonSerializer.Serialize(kong, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((h, c) =>
                {
                    c.AddJsonFile("kongsettings.json", optional: false);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
