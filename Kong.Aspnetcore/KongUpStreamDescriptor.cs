using Kong.Aspnetcore.AdminApi;
using System.Collections.Generic;
using WebApiClient.DataAnnotations;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 表示kong上游节点
    /// </summary>
    public class KongUpStreamDescriptor : KongUpstream
    {
        /// <summary>
        /// 目标集合
        /// </summary>
        [IgnoreSerialized]
        public List<KongTarget> Targets { get; set; } = new List<KongTarget>();
    }
}
