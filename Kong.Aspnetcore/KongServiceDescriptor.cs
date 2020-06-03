using Kong.Aspnetcore.AdminApi;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 表示kong服务选项
    /// </summary>
    public class KongServiceDescriptor : KongService
    {
        /// <summary>
        /// 路由集合
        /// </summary>
        [JsonIgnore]
        public List<KongRoute> Routes { get; set; } = new List<KongRoute>();
    }
}
