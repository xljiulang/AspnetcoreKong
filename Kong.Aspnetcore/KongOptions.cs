using Kong.Aspnetcore.AdminApi;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 表示kong选项
    /// </summary>
    [DebuggerDisplay("AdminApi = {AdminApi}")]
    public class KongOptions
    {
        /// <summary>
        /// kong的adminApi地址
        /// </summary>      
        public Uri AdminApi { get; set; }

        /// <summary>
        /// AdminApi的请求头
        /// </summary>
        public Dictionary<string, string> AdminApiHeaders { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 自动给路由名称添加前缀
        /// </summary>
        public bool RouteNamePrefix { get; set; } = true;

        /// <summary>
        /// 服务
        /// </summary>      
        public KongServiceDescriptor Service { get; set; } = new KongServiceDescriptor();

        /// <summary>
        /// 上游
        /// </summary>
        public KongUpStreamDescriptor UpStream { get; set; }



        /// <summary>
        /// 指定AdminApi请求地址
        /// </summary>
        /// <param name="host">kong的ip或域名</param>
        /// <param name="port">admin管理端口</param>
        /// <returns></returns>
        public KongOptions WithAdminApi(string host, int port = 8001)
        {
            this.AdminApi = new Uri($"http://{host}:{port}");
            return this;
        }

        /// <summary>
        /// 指定AdminApi请求头
        /// </summary>
        /// <param name="name">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public KongOptions WithAdminApiHeader(string name, string value)
        {
            this.AdminApiHeaders.TryAdd(name, value);
            return this;
        }

        /// <summary>
        /// 指定路由使用服务器前端
        /// </summary>
        /// <param name="usePrefix">是否使用前缀</param>
        /// <returns></returns>
        public KongOptions WithRouteNamePrefix(bool usePrefix = true)
        {
            this.RouteNamePrefix = usePrefix;
            return this;
        }

        /// <summary>
        /// 指定服务
        /// </summary>
        /// <param name="name">服务名</param>
        /// <param name="host">主机(当配置upstream时，upstream的名称为该值)</param>
        /// <param name="port">服务端口(当配置upstream时，不使用该值）</param>
        /// <param name="path">服务路径(当配置upstream时，不使用该值）</param>
        /// <returns></returns>
        public KongOptions WithService(string name, string host, int port = default, string path = default)
        {
            this.Service.Name = name;
            this.Service.Host = host;

            if (port > 0)
            {
                this.Service.Port = port;
            }
            if (path != default)
            {
                this.Service.Path = path;
            }
            return this;
        }

        /// <summary>
        /// 指定一个路由
        /// </summary>
        /// <param name="name">路由名称</param>
        /// <param name="strip_path">是否可以隐式通过服务名+path匹配</param>
        /// <param name="paths">路径集合，至少一个值</param>
        /// <returns></returns>
        public KongOptions WithServiceRoute(string name, bool strip_path, params string[] paths)
        {
            if (paths.Length > 0)
            {
                this.Service.Routes.Add(new KongRoute
                {
                    Name = name,
                    Strip_path = strip_path,
                    Paths = paths
                });
            }
            return this;
        }

        /// <summary>
        /// 指定上游
        /// </summary>
        /// <param name="activeHealthchecksPath">主动健康检查路径</param>
        /// <param name="interval">检查时间间隔的秒数</param>
        /// <returns></returns>
        public KongOptions WithUpStream(string activeHealthchecksPath = default, int interval = 10)
        {
            var upstream = this.UpStream ?? new KongUpStreamDescriptor();

            if (activeHealthchecksPath != default)
            {
                var active = upstream.Healthchecks.Active;
                active.Http_path = activeHealthchecksPath;
                active.Timeout = 5;
                active.Unhealthy.Tcp_failures = 1;
                active.Unhealthy.Timeouts = 1;
                active.Unhealthy.Http_failures = 1;
                active.Unhealthy.Interval = interval;
                active.Healthy.Successes = 1;
                active.Healthy.Interval = interval;
            }

            this.UpStream = upstream;
            return this;
        }

        /// <summary>
        /// 指定本机为目标主机
        /// </summary>
        /// <param name="host">目标主机ip</param>
        /// <param name="port">目标主机端口</param>
        /// <param name="weight">主机的服务比重</param>
        /// <returns></returns>
        public KongOptions WithUpstreamTarget(string host, int port, int weight = 100)
        {
            this.WithUpStream();
            this.UpStream.Targets.Add(new KongTarget
            {
                Target = $"{host}:{port}",
                Weight = weight
            });
            return this;
        }

        /// <summary>
        /// 使用配置的urls作为目标主机
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="weight">主机的服务比重</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="LANIPAddressNotMatchException"></exception>
        /// <returns></returns>
        public KongOptions WithUpstreamTarget(IConfiguration configuration, int weight = 100)
        {
            var urls = GetConfigurationUrls(configuration).ToArray();

            if (urls.Length == 1)
            {
                var uri = urls.FirstOrDefault();
                return this.WithUpstreamTarget(uri.Host, uri.Port, weight);
            }

            if (this.AdminApi == null)
            {
                throw new InvalidOperationException($"请先设置{nameof(AdminApi)}");
            }

            var hostIp = LANIPAddress.GetMatchLANIPAddress(this.AdminApi.Host);
            if (hostIp == null)
            {
                throw new LANIPAddressNotMatchException($"无法获取到与{this.AdminApi.Host}可通讯的主机ip");
            }

            var matchUri = urls.FirstOrDefault(item => item.Host == hostIp.ToString());
            if (matchUri == null)
            {
                throw new LANIPAddressNotMatchException($"无法获取到与{this.AdminApi.Host}可通讯的主机ip");
            }
            return this.WithUpstreamTarget(matchUri.Host, matchUri.Port, weight);
        }

        /// <summary>
        /// 获取配置监听的Uri
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static IEnumerable<Uri> GetConfigurationUrls(IConfiguration configuration)
        {
            var defaultUri = new Uri("http://127.0.0.1:5000/");
            var urls = configuration.GetValue<string>("urls");
            if (string.IsNullOrEmpty(urls))
            {
                yield return defaultUri;
                yield break;
            }

            var http = urls
                .Split(";", StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault(item => item.StartsWith("http://", StringComparison.OrdinalIgnoreCase));

            if (http == null)
            {
                yield return defaultUri;
                yield break;
            }

            if (Uri.TryCreate(http, UriKind.Absolute, out var uri) == true)
            {
                yield return uri;
                yield break;
            }

            // http://*:{port}
            var match = Regex.Match(http, @"(?<=:)\d+");
            var port = match.Success ? int.Parse(match.Value) : 80;
            foreach (var ip in LANIPAddress.GetAllAddresses())
            {
                yield return new Uri($"http://{ip}:{port}/");
            }
        }
    }
}
