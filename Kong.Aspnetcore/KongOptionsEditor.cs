using Kong.Aspnetcore.AdminApi;
using System;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 表示kong选项编辑器
    /// </summary>
    public class KongOptionsEditor
    {
        private readonly KongOptions options = new KongOptions();

        /// <summary>
        /// kong选项编辑器
        /// </summary>
        /// <param name="options"></param>
        public KongOptionsEditor(KongOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// 指定AdminApi地址
        /// </summary>
        /// <param name="host">kong的ip或域名</param>
        /// <param name="port">admin管理端口</param>
        /// <returns></returns>
        public KongOptionsEditor WithAdminApi(string host, int port = 8001)
        {
            this.options.AdminApi = new Uri($"http://{host}:{port}");
            return this;
        }

        /// <summary>
        /// 指定路由使用服务器前端
        /// </summary>
        /// <param name="usePrefix">是否使用前缀</param>
        /// <returns></returns>
        public KongOptionsEditor WithRouteNamePrefix(bool usePrefix = true)
        {
            this.options.RouteNamePrefix = usePrefix;
            return this;
        }

        /// <summary>
        /// 指定服务
        /// </summary>
        /// <param name="name">服务名</param>
        /// <param name="host">主机(当配置upstream时，值为upstream的名称)</param>
        /// <param name="port">服务端口(当配置upstream时，不使用该值）</param>
        /// <param name="path">服务路径(当配置upstream时，不使用该值）</param>
        /// <returns></returns>
        public KongOptionsEditor WithService(string name, string host, int port = default, string path = default)
        {
            this.options.Service.Name = name;
            this.options.Service.Host = host;

            if (port > 0)
            {
                this.options.Service.Port = port;
            }
            if (path != default)
            {
                this.options.Service.Path = path;
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
        public KongOptionsEditor WithRoute(string name, bool strip_path, params string[] paths)
        {
            if (paths.Length > 0)
            {
                this.options.Service.Routes.Add(new KongRoute
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
        public KongOptionsEditor WithUpStream(string activeHealthchecksPath = default, int interval = 10)
        {
            var upstream = this.options.UpStream ?? new KongUpStreamOptions();

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

            this.options.UpStream = upstream;
            return this;
        }

        /// <summary>
        /// 指定本机为目标主机
        /// </summary>
        /// <param name="host">本机域名或ip，{localhost}则自动替换为本机合适的局域网ip</param>
        /// <param name="port">本机服务端口</param>
        /// <param name="weight">主机的服务比重</param>
        /// <returns></returns>
        public KongOptionsEditor WithTarget(string host = "{localhost}", int port = 8000, int weight = 100)
        {
            this.WithUpStream();
            this.options.UpStream.Targets.Add(new KongTarget
            {
                Target = $"{host}:{port}",
                Weight = weight
            });
            return this;
        }

        /// <summary>
        /// 获取配置选项
        /// </summary>
        /// <returns></returns>
        public KongOptions GetOptions()
        {
            return this.options;
        }
    }
}
