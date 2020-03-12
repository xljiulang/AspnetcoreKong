namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 定义kong对象的接口
    /// </summary>
    interface IKongObject
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        string Id { get; set; }
    }
}
