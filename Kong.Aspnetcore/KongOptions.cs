using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

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
        [Required]
        public Uri AdminApi { get; set; }

        /// <summary>
        /// 自动给路由名称添加前缀
        /// </summary>
        public bool RouteNamePrefix { get; set; } = true;

        /// <summary>
        /// 服务
        /// </summary>
        [Required]
        public KongServiceOptions Service { get; set; } = new KongServiceOptions();

        /// <summary>
        /// 上游
        /// </summary>
        public KongUpStreamOptions UpStream { get; set; } 
    }
}
