namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong目标对象
    /// </summary>
    public class KongTargetObject : KongTarget, IKongObject
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; set; }
    }
}
