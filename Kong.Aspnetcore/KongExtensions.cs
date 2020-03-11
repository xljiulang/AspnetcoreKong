using Kong.Aspnetcore;
using Kong.Aspnetcore.AdminApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiClient;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KongExtensions
    {
        public static IServiceCollection AddKong(this IServiceCollection services)
        {
            services.AddOptions<KongOptions>().Configure<IConfiguration>((o, c) =>
            {
                c.GetSection("kong").Bind(o);
                o.UpStream.Name = o.Service.Url.Host;
            });

            services.AddHttpApi<IKongAdminApi>((o, s) =>
            {
                var kong = s.GetService<IOptions<KongOptions>>().Value;
                o.HttpHost = kong.AdminApi;
                o.FormatOptions = new FormatOptions { UseCamelCase = true };
            });

            return services;
        }

        public static async void UseKong(this IApplicationBuilder app)
        {
            try
            {
                await UseKongAsync(app.ApplicationServices);
            }
            catch (Exception ex)
            {
                var logger = app.ApplicationServices.GetService<ILogger<KongOptions>>();
                logger.LogError(ex, ex.Message);
            }
        }

        private static async Task UseKongAsync(IServiceProvider provider)
        {
            var kong = provider.GetService<IKongAdminApi>();
            var local = provider.GetService<IOptions<KongOptions>>().Value;

            var service = await kong.GetServiceAsync(local.Service.GetName());
            if (service == null)
            {
                service = await kong.AddServiceAsync(local.Service);
            }

            var routes = await kong.GetRoutesAsync(service.Id);
            foreach (var localRoute in local.Service.Routes)
            {
                var route = routes.Data.FirstOrDefault(item => item.Name == localRoute.Name);
                if (route != null)
                {
                    await kong.UpdateRouteAsync(route.Id, localRoute);
                }
                else
                {
                    await kong.AddRouteAsync(service.Id, localRoute);
                }
            }

            if (local.UpStream != null)
            {
                var upStream = await kong.GetUpstreamAsync(local.UpStream.Name);
                if (upStream == null)
                {
                    upStream = await kong.AddUpstreamAsync(local.UpStream);
                }

                var targets = await kong.GetTargetsAsync(upStream.Id);
                foreach (var localTarget in local.UpStream.Targets)
                {
                    var target = targets.Data.FirstOrDefault(item => item.Target == localTarget.Target);
                    if (target != null)
                    {
                        if (target.Weight != localTarget.Weight)
                        {
                            await kong.DeleteTargetAsync(upStream.Id, target.Id);
                            await kong.AddTargetAsync(upStream.Id, localTarget);
                        }
                    }
                    else
                    {
                        await kong.AddTargetAsync(upStream.Id, localTarget);
                    }
                }
            }
        }
    }
}
