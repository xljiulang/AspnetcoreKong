using System;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 表示找不到服务地址的异常
    /// </summary>
    public class ServerAddressNotFoundException : Exception
    {
        /// <summary>
        /// 找不到服务地址的异常
        /// </summary>
        /// <param name="message"></param>
        public ServerAddressNotFoundException(string message)
            : base(message)
        {
        }
    }
}
