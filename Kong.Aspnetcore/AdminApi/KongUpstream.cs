using System;
using System.Diagnostics;

namespace Kong.Aspnetcore.AdminApi
{
    /// <summary>
    /// 表示kong上游编辑对象
    /// </summary>
    [DebuggerDisplay("Name = {Name}")]
    public class KongUpstream
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public string Hash_on { get; set; } = "none";

        public string Hash_fallback { get; set; } = "none";

        public string Hash_on_cookie_path { get; set; } = "/";

        /// <summary>
        /// 权重总槽数量
        /// </summary>
        public int Slots { get; set; } = 10000;

        /// <summary>
        /// 健康检查
        /// </summary>
        public HealthchecksInfo Healthchecks { get; set; } = new HealthchecksInfo();

        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; set; } = Array.Empty<string>();

        /// <summary>
        /// 健康检查信息
        /// </summary>
        public class HealthchecksInfo
        {
            /// <summary>
            /// 主动检查
            /// </summary>
            public ActiveInfo Active { get; set; } = new ActiveInfo();

            /// <summary>
            /// 被动检查
            /// </summary>
            public PassiveInfo Passive { get; set; } = new PassiveInfo();

            /// <summary>
            /// 主动检查信息
            /// </summary>
            public class ActiveInfo
            {
                /// <summary>
                /// 验证https证书
                /// </summary>
                public bool Https_verify_certificate { get; set; } = true;

                /// <summary>
                /// 不健康配置
                /// </summary>
                public UnhealthyInfo Unhealthy { get; set; } = new UnhealthyInfo();

                /// <summary>
                /// 请求的http路径
                /// </summary>
                public string Http_path { get; set; } = "/";

                /// <summary>
                /// Number of timeouts in active probes to consider a target unhealthy.
                /// </summary>
                public int Timeout { get; set; } = 1;

                public HealthyInfo Healthy { get; set; } = new HealthyInfo();

                /// <summary>
                /// The hostname to use as an SNI (Server Name Identification) when performing active health checks using HTTPS. 
                /// This is particularly useful when Targets are configured using IPs, so that the target host’s certificate can be verified with the proper SNI.
                /// </summary>
                public string Https_sni { get; set; }

                /// <summary>
                /// Number of targets to check concurrently in active health checks.
                /// </summary>
                public int Concurrency { get; set; } = 10;

                /// <summary>
                /// Whether to perform active health checks using HTTP or HTTPS, or just attempt a TCP connection. 
                /// Possible values are tcp, http or https. Defaults to "http".
                /// </summary>
                public string Type { get; set; } = "http";

                public class UnhealthyInfo
                {
                    /// <summary>
                    /// An array of HTTP statuses to consider a failure, indicating unhealthiness, when returned by a probe in active health checks. 
                    /// Defaults to [429, 404, 500, 501, 502, 503, 504, 505]
                    /// </summary>
                    public int[] Http_statuses { get; set; } = new[] { 429, 404, 500, 501, 502, 503, 504, 505 };

                    /// <summary>
                    /// Number of TCP failures in active probes to consider a target unhealthy.
                    /// </summary>
                    public int Tcp_failures { get; set; }

                    /// <summary>
                    /// Number of timeouts in active probes to consider a target unhealthy.
                    /// </summary>
                    public int Timeouts { get; set; }

                    /// <summary>
                    /// Number of HTTP failures in active probes (as defined by healthchecks.active.unhealthy.http_statuses) to consider a target unhealthy.
                    /// </summary>
                    public int Http_failures { get; set; }

                    /// <summary>
                    /// Interval between active health checks for unhealthy targets (in seconds). 
                    /// A value of zero indicates that active probes for healthy targets should not be performed.
                    /// </summary>
                    public int Interval { get; set; }
                }


                public class HealthyInfo
                {
                    /// <summary>
                    /// An array of HTTP statuses to consider a success, indicating healthiness, when returned by a probe in active health checks. 
                    /// Defaults to [200, 302].
                    /// </summary>
                    public int[] Http_statuses { get; set; } = new[] { 200, 302 };

                    /// <summary>
                    /// Interval between active health checks for healthy targets (in seconds). 
                    /// A value of zero indicates that active probes for healthy targets should not be performed.
                    /// </summary>
                    public int Interval { get; set; }

                    /// <summary>
                    /// Number of successes in active probes (as defined by healthchecks.active.healthy.http_statuses) to consider a target healthy.
                    /// </summary>
                    public int Successes { get; set; }
                }
            }

            public class PassiveInfo
            {
                public UnhealthyInfo Unhealthy { get; set; } = new UnhealthyInfo();

                public string Type { get; set; } = "http";

                public HealthyInfo Healthy { get; set; } = new HealthyInfo();

                public class UnhealthyInfo
                {
                    public int Http_failures { get; set; }

                    public int[] Http_statuses { get; set; } = new[] { 429, 500, 503 };

                    public int Tcp_failures { get; set; }

                    public int Timeouts { get; set; }
                }

                public class HealthyInfo
                {
                    public int Successes { get; set; }

                    public int[] Http_statuses { get; set; } = new[] { 200, 201, 202, 203, 204, 205, 206, 207, 208, 226, 300, 301, 302, 303, 304, 305, 306, 307, 308 };
                }
            }
        }
    }
}