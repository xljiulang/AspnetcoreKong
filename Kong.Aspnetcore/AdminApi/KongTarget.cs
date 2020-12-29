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
        /// 获取端口
        /// </summary>
        /// <returns></returns>
        public int GetPort()
        {
            if (string.IsNullOrEmpty(this.Target))
            {
                return 0;
            }
            var hostPort = this.Target.Split(':');
            if (hostPort.Length == 1)
            {
                return 0;
            }

            int.TryParse(hostPort.Last(), out var port);
            return port;
        }

        /// <summary>
        /// 获取主机
        /// </summary>
        /// <returns></returns>
        public string GetHost()
        {
            if (string.IsNullOrEmpty(this.Target))
            {
                return null;
            }

            var host = this.Target.Split(':').FirstOrDefault();
            return host == "*"
                || host == "+"
                || host == IPAddress.Any.ToString()
                || host == IPAddress.IPv6Any.ToString()
                ? null
                : host;
        }
    }
}
