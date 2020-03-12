using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong目标编辑对象
    /// </summary>
    [DebuggerDisplay("Target = {Target}")]
    public class KongTargetEdit
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
    }
}
