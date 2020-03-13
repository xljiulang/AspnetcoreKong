using System;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 表示局域网IP匹配异常
    /// </summary>
    public class LANIPAddressNotMatchException : Exception
    {
        /// <summary>
        /// 局域网IP匹配异常
        /// </summary>
        /// <param name="message"></param>
        public LANIPAddressNotMatchException(string message)
            : base(message)
        {
        }
    }
}
