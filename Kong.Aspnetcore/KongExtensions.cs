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
        /// 添加Kong
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static OptionsBuilder<KongOptions> AddKong(this IServiceCollection services)
        {
            services
                .AddSingleton<KongApplication>()
                .AddHttpApi<IKongAdminApi>()
                .ConfigureHttpClient((s, httpClient) =>
                {
                    var options = s.GetService<IOptions<KongOptions>>().Value;
                    httpClient.BaseAddress = options.AdminApi;
                    foreach (var item in options.AdminApiHeaders)
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                    }
                });

            return services.AddOptions<KongOptions>();
        }

        /// <summary>
        /// 添加Kong
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">kong配置</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddKong().Bind(configuration).Services;
        }

        /// <summary>
        /// 添加Kong
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">kong配置</param>
        /// <param name="configureOptions">kong选项配置</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, IConfiguration configuration, Action<KongOptions> configureOptions)
        {
            return services.AddKong().Bind(configuration).Configure(configureOptions).Services;
        }

        /// <summary>
        /// 添加Kong
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">kong配置</param>
        /// <param name="configureOptions">kong选项配置</param>
        /// <returns></returns>
        public static IServiceCollection AddKong(this IServiceCollection services, IConfiguration configuration, Action<KongOptions, IServiceProvider> configureOptions)
        {
            return services.AddKong().Bind(configuration).Configure(configureOptions).Services;
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
