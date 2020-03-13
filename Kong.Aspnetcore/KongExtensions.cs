using Kong.Aspnetcore;
using Kong.Aspnetcore.AdminApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiClient;

namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// kong注册扩展
    /// </summary>
    public static class KongExtensions
    {
        /// <summary>
        /// 添加Kong配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section">kong配置节点名称</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, string section = "kong")
        {
            return services.AddKongCore((o, s) =>
            {
                s.GetService<IConfiguration>().GetSection(section).Bind(o);
            });
        }

        /// <summary>
        /// 添加Kong配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions">kong选项配置</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, Action<KongOptionsEditor, IServiceProvider> configureOptions)
        {
            return services.AddKongCore((o, s) =>
            {
                var editor = new KongOptionsEditor(o);
                configureOptions?.Invoke(editor, s);
            });
        }

        /// <summary>
        /// 添加Kong配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions">kong选项配置</param>
        /// <returns></returns>
        private static IServiceCollection AddKongCore(this IServiceCollection services, Action<KongOptions, IServiceProvider> configureOptions)
        {
            services
                .AddOptions<KongOptions>()
                .Configure(configureOptions)
                .Configure(o =>
                {
                    if (o.UpStream != null)
                    {
                        o.UpStream.Name = o.Service.Host;
                        var localIp = LANIPAddress.GetMatchLANIPAddress(o.AdminApi.Host);

                        foreach (var target in o.UpStream.Targets)
                        {
                            target.Target = Regex.Replace(target.Target, "{localhost}", match =>
                            {
                                if (localIp == null)
                                {
                                    throw new LANIPAddressNotMatchException($"无法获取到与{o.AdminApi.Host}处在同一网段的本机局域网ip");
                                }
                                return localIp.ToString();
                            }, RegexOptions.IgnoreCase);
                        }
                    }

                    if (o.RouteNamePrefix == true)
                    {
                        foreach (var route in o.Service.Routes)
                        {
                            route.Name = $"{o.Service.Name}_{route.Name}";
                        }
                    }
                });

            services.AddHttpApi<IKongAdminApi>((o, s) =>
            {
                var kong = s.GetService<IOptions<KongOptions>>().Value;
                o.HttpHost = kong.AdminApi;
                o.FormatOptions = new FormatOptions { UseCamelCase = true };
            });

            return services;
        }

        /// <summary>
        /// 使用Kong
        /// </summary>
        /// <param name="app"></param>
        public static async void UseKong(this IApplicationBuilder app)
        {
            try
            {
                var logger = app.ApplicationServices.GetService<ILogger<IKongAdminApi>>();
                var local = app.ApplicationServices.GetService<IOptions<KongOptions>>().Value;
                var kong = app.ApplicationServices.GetService<IKongAdminApi>();
                await UseKongAsync(local, kong, logger);
            }
            catch (Exception ex)
            {
                var logger = app.ApplicationServices.GetService<ILogger<IKongAdminApi>>();
                logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// 使用kong
        /// </summary>
        /// <param name="kong"></param>
        /// <param name="local"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private static async Task UseKongAsync(KongOptions local, IKongAdminApi kong, ILogger<IKongAdminApi> logger)
        {
            var service = await kong.GetServiceAsync(local.Service.Name).HandleAsDefaultWhenException();
            if (service == null)
            {
                logger.LogInformation($"正在添加服务{local.Service.Name}");
                service = await kong.AddServiceAsync(local.Service);
                logger.LogInformation($"添加服务{local.Service.Name} ok.");
            }

            var routes = await kong.GetRoutesAsync(service.Id);
            foreach (var localRoute in local.Service.Routes)
            {
                var route = routes.Data.FirstOrDefault(item => item.Name == localRoute.Name);
                if (route != null)
                {
                    logger.LogInformation($"正在更新路由{route.Name}");
                    localRoute.Service = new KongObject { Id = service.Id };
                    await kong.UpdateRouteAsync(route.Id, localRoute);
                    logger.LogInformation($"更新路由{route.Name} ok.");
                }
                else
                {
                    logger.LogInformation($"正在添加路由{route.Name}");
                    await kong.AddRouteAsync(service.Id, localRoute);
                    logger.LogInformation($"添加路由{route.Name} ok.");
                }
            }

            foreach (var route in routes.Data)
            {
                if (local.Service.Routes.Any(item => item.Name == route.Name) == false)
                {
                    logger.LogInformation($"正在删除路由{route.Name}");
                    await kong.DeleteRouteAsync(route.Id);
                    logger.LogInformation($"删除路由{route.Name} ok.");
                }
            }

            if (local.UpStream != null)
            {
                var upStream = await kong.GetUpstreamAsync(local.UpStream.Name).HandleAsDefaultWhenException();
                if (upStream == null)
                {
                    logger.LogInformation($"正在添加上游{upStream.Name}");
                    upStream = await kong.AddUpstreamAsync(local.UpStream);
                    logger.LogInformation($"添加上游{upStream.Name} ok.");
                }
                else
                {
                    logger.LogInformation($"正在更新上游{upStream.Name}");
                    upStream = await kong.UpdateUpstreamAsync(upStream.Id, local.UpStream);
                    logger.LogInformation($"更新上游{upStream.Name} ok.");
                }

                var targets = await kong.GetTargetsAsync(upStream.Id);
                foreach (var localTarget in local.UpStream.Targets)
                {
                    var target = targets.Data.FirstOrDefault(item => item.Target == localTarget.Target);
                    if (target != null)
                    {
                        if (target.Weight != localTarget.Weight)
                        {
                            logger.LogInformation($"正在更新上游{upStream.Name}的目标{localTarget.Target}");
                            await kong.DeleteTargetAsync(upStream.Id, target.Id);
                            await kong.AddTargetAsync(upStream.Id, localTarget);
                            logger.LogInformation($"更新上游{upStream.Name}的目标{localTarget.Target} ok.");
                        }
                    }
                    else
                    {
                        logger.LogInformation($"正在添加上游{upStream.Name}的目标{localTarget.Target}");
                        await kong.AddTargetAsync(upStream.Id, localTarget);
                        logger.LogInformation($"添加上游{upStream.Name}的目标{localTarget.Target} ok.");
                    }
                }
            }
        }
    }
}
