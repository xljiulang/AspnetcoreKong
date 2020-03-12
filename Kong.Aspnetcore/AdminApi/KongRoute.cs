namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong路由
    /// </summary>
    public class KongRoute : KongRouteEdit
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; set; }
    }
}
