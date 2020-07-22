using Kong.Aspnetcore;
using Kong.Aspnetcore.AdminApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

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
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services)
        {
            return services.AddKong("kong");
        }

        /// <summary>
        /// 添加Kong配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section">kong配置节点名称</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, string section)
        {
            return services.AddKong(section, o => { });
        }


        /// <summary>
        /// 添加Kong配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section">kong配置节点名称</param>
        /// <param name="configureOptions">kong选项配置</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, string section, Action<KongOptions> configureOptions)
        {
            return services.AddKong(section, (o, s) => configureOptions?.Invoke(o));
        }

        /// <summary>
        /// 添加Kong配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section">kong配置节点名称</param>
        /// <param name="configureOptions">kong选项配置</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, string section, Action<KongOptions, IServiceProvider> configureOptions)
        {
            services
                .AddOptions<KongOptions>()
                .Configure<IConfiguration>((o, c) => c.GetSection(section).Bind(o))
                .Configure(configureOptions)
                .Configure(o =>
                {
                    if (o.UpStream != null)
                    {
                        o.UpStream.Name = o.Service.Host;
                    }
                });

            services
                .AddSingleton<KongApplication>()
                .AddHttpApi<IKongAdminApi>()
                .ConfigureHttpClient((s, c) =>
                {
                    var kong = s.GetService<IOptions<KongOptions>>().Value;
                    c.BaseAddress = kong.AdminApi;
                    foreach (var item in kong.AdminApiHeaders)
                    {
                        c.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                });

            return services;
        }

        /// <summary>
        /// 使用Kong
        /// </summary>
        /// <param name="app"></param>
        public static async void UseKong(this IApplicationBuilder app)
        {
            await app
                .ApplicationServices
                .GetRequiredService<KongApplication>()
                .RegisterAsync();
        }
    }
}
