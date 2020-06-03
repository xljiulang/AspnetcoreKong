using Kong.Aspnetcore.AdminApi;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public List<KongTarget> Targets { get; set; } = new List<KongTarget>();
    }
}
