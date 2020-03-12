using Kong.Aspnetcore.AdminApi;
using System;
using WebApiClient.DataAnnotations;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 表示kong服务选项
    /// </summary>
    public class KongServiceOptions : KongServiceEdit
    {
        /// <summary>
        /// 路由集合
        /// </summary>
        [IgnoreSerialized]
        public KongRouteEdit[] Routes { get; set; } = Array.Empty<KongRouteEdit>();
    }
}
