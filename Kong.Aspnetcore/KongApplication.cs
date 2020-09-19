using Kong.Aspnetcore.AdminApi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// kong应用
    /// </summary>
    public class KongApplication
    {
        private readonly KongOptions local;
        private readonly IKongAdminApi kong;
        private readonly ILogger logger;

        /// <summary>
        /// KongApplication
        /// </summary> 
        /// <param name="local"></param>
        /// <param name="kong"></param>
        /// <param name="logger"></param>
        public KongApplication(IOptions<KongOptions> local, IKongAdminApi kong, ILogger<IKongAdminApi> logger)
        {
            this.local = local.Value;
            this.kong = kong;
            this.logger = logger;
        }

        /// <summary>
        /// 注册服务到kong
        /// </summary>
        /// <returns></returns>
        public async Task RegisterAsync()
        {
            try
            {
                await FixOptionsAsync();
                await RegisterCoreAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// 修复kong选项
        /// </summary>
        /// <returns></returns>
        private async Task FixOptionsAsync()
        {
            if (local.AdminApi == null)
            {
                throw new InvalidOperationException($"请先设置{nameof(AdminApi)}");
            }

            if (local.RouteNamePrefix == true)
            {
                foreach (var route in local.Service.Routes)
                {
                    route.Name = $"{local.Service.Name}_{route.Name}";
                }
            }

            if (local.UpStream != null)
            {
                local.UpStream.Name = local.Service.Host;
                foreach (var target in local.UpStream.Targets)
                {
                    var endpoint = target.GetTargetEndPoint();
                    if (endpoint.Address.Equals(IPAddress.Any) == true)
                    {
                        var address = await ServerAddress.GetServerAddressAsync(local.AdminApi);
                        target.Target = $"{address}:{endpoint.Port}";
                    }
                }
            }
        }


        /// <summary>
        /// 注册服务到kong
        /// </summary> 
        /// <returns></returns>
        private async Task RegisterCoreAsync()
        {
            var service = await kong.GetServiceAsync(local.Service.Name);
            if (service == null)
            {
                logger.LogInformation($"正在添加服务{local.Service.Name}");
                service = await kong.AddServiceAsync(local.Service);
                logger.LogInformation($"添加服务{local.Service.Name} ok.");
            }
            else
            {
                logger.LogInformation($"正在更新服务{local.Service.Name}");
                service = await kong.UpdateServiceAsync(service.Id, local.Service);
                logger.LogInformation($"更新服务{local.Service.Name} ok.");
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
                    logger.LogInformation($"正在添加路由{localRoute.Name}");
                    await kong.AddRouteAsync(service.Id, localRoute);
                    logger.LogInformation($"添加路由{localRoute.Name} ok.");
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
                var upStream = await kong.GetUpstreamAsync(local.UpStream.Name);
                if (upStream == null)
                {
                    logger.LogInformation($"正在添加上游{local.UpStream.Name}");
                    upStream = await kong.AddUpstreamAsync(local.UpStream);
                    logger.LogInformation($"添加上游{local.UpStream.Name} ok.");
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
