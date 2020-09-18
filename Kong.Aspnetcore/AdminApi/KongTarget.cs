using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong目标编辑对象
    /// </summary>
    [DebuggerDisplay("Target = {Target}")]
    public class KongTarget
    {
        /// <summary>
        /// 目标地址(ip或主机名)和端口号
        /// 如果省略了端口,默认为8000
        /// </summary>
        [Required]
        public string Target { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; set; } = 100;

        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 关联的上游
        /// </summary>
        public KongObject Upstream { get; set; }

        /// <summary>
        /// 获取目标节点
        /// </summary>
        /// <returns></returns>
        public IPEndPoint GetTargetEndPoint()
        {
            const int defaultPort = 8000;
            if (string.IsNullOrEmpty(this.Target))
            {
                return new IPEndPoint(IPAddress.Any, defaultPort);
            }

            var endpoint = this.Target.Split(':');
            if (IPAddress.TryParse(endpoint.First(), out var ip) == false)
            {
                ip = IPAddress.Any;
            }

            if (int.TryParse(endpoint.Last(), out var port) == false)
            {
                port = defaultPort;
            }

            return new IPEndPoint(ip, port);
        }
    }
}
