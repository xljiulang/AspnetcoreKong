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
        public static OptionsBuilder<KongOptions> AddKong(this IServiceCollection services)
        {
            return services.AddKongCore().AddKongOptions();
        }

        /// <summary>
        /// 添加Kong配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">kong配置</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddKong().Bind(configuration);
            return services;
        }

        /// <summary>
        /// 添加Kong配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">kong配置</param>
        /// <param name="configureOptions">kong选项配置</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, IConfiguration configuration, Action<KongOptions> configureOptions)
        {
            services.AddKong().Bind(configuration).Configure(configureOptions);
            return services;
        }

        /// <summary>
        /// 添加Kong配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">kong配置</param>
        /// <param name="configureOptions">kong选项配置</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, IConfiguration configuration, Action<KongOptions, IServiceProvider> configureOptions)
        {
            services.AddKong().Bind(configuration).Configure(configureOptions);
            return services;
        }

        /// <summary>
        /// 添加kong
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddKongCore(this IServiceCollection services)
        {
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
        /// 添加选项
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static OptionsBuilder<KongOptions> AddKongOptions(this IServiceCollection services)
        {
            return services
                .AddOptions<KongOptions>()
                .PostConfigure(o =>
                {
                    if (o.UpStream != null)
                    {
                        o.UpStream.Name = o.Service.Host;
                    }
                });
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
