﻿using System;

namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong服务对象
    /// </summary>
    public class KongServiceObject : KongService, IKongObject
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public string Id { get; set; }
    }
}
