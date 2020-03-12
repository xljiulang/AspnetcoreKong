namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong路由
    /// </summary>
    public class KongRouteObject : KongRoute, IKongObject
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; set; }
    }
}
