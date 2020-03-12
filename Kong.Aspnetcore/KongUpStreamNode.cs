using Kong.Aspnetcore.AdminApi;
using System;
using WebApiClient.DataAnnotations;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 表示kong上游选项
    /// </summary>
    public class KongUpStreamNode : KongUpstreamEdit
    {
        /// <summary>
        /// 目标集合
        /// </summary>
        [IgnoreSerialized]
        public KongTargetEdit[] Targets { get; set; } = Array.Empty<KongTargetEdit>();
    }
}
