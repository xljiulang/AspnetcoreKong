using Kong.Aspnetcore.AdminApi;
using System;
using System.Collections.Generic;
using WebApiClient.DataAnnotations;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 表示kong服务选项
    /// </summary>
    public class KongServiceOptions : KongService
    {
        /// <summary>
        /// 路由集合
        /// </summary>
        [IgnoreSerialized]
        public List<KongRoute> Routes { get; set; } = new List<KongRoute>();
    }
}
