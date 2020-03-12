namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong上游
    /// </summary>
    public class KongUpstreamObject : KongUpstream, IKongObject
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; set; }
    }
}
