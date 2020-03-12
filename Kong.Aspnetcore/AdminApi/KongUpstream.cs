namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong上游
    /// </summary>
    public class KongUpstream : KongUpstreamEdit
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; set; }
    }
}
