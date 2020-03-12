namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong响应数据
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class KongArray<TData>
    {
        /// <summary>
        /// 数据
        /// </summary>
        public TData[] Data { get; set; }

        /// <summary>
        /// 下一个数据标识
        /// </summary>
        public string Next { get; set; }
    }

}
