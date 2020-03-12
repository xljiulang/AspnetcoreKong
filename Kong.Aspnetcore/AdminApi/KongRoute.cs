using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// kong路由配置信息
    /// </summary>
    [DebuggerDisplay("Name = {Name}")]
    public class KongRoute
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 协议
        /// </summary>
        public string[] Protocols { get; set; } = new[] { "http", "https" };

        /// <summary>
        /// 请求方法
        /// </summary>
        public string[] Methods { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 主机
        /// </summary>
        public string[] Hosts { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 路径
        /// </summary>
        [Required]
        public string[] Paths { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Https重定向状态码
        /// </summary>
        public int Https_redirect_status_code { get; set; } = 426;

        /// <summary>
        /// 正则表达式优先
        /// </summary>
        public int Regex_priority { get; set; } = 0;

        /// <summary>
        /// 当匹配通过的一个路线 路径 ,去上游请求URL中的匹配的前缀
        /// </summary>
        public bool Strip_path { get; set; } = false;

        /// <summary>
        /// 保护主机
        /// </summary>
        public bool Preserve_host { get; set; } = false;

        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 关联的服务
        /// </summary>
        public KongObject Service { get; set; }
    }
}
