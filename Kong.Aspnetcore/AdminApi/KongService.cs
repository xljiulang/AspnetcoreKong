using System;
using System.ComponentModel.DataAnnotations;

namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// kong服务编辑信息
    /// </summary>
    public class KongService
    {
        /// <summary>
        /// 服务名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int Retries { get; set; } = 5;

        /// <summary>
        /// 协议
        /// </summary>
        public string Protocol { get; set; } = "http";

        /// <summary>
        /// 主机或上游名称
        /// </summary>
        [Required]
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 80;

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int Connect_timeout { get; set; } = 60000;

        /// <summary>
        /// 写入超时超时
        /// </summary>
        public int Write_timeout { get; set; } = 60000;

        /// <summary>
        /// 读取超时时间
        /// </summary>
        public int Read_timeout { get; set; } = 60000;

        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; set; } = Array.Empty<string>();
    }
}
