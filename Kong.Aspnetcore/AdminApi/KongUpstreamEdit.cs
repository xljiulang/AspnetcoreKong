using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Kong.Aspnetcore.AdminApi
{
    [DebuggerDisplay("Name = {Name}")]
    public class KongUpstreamEdit
    {
        public string Name { get; set; }

        public string Hash_on { get; set; } = "none";

        public string Hash_fallback { get; set; } = "none";

        public string Hash_on_cookie_path { get; set; } = "/";

        public int Slots { get; set; } = 10000;

        public HealthchecksInfo Healthchecks { get; set; } = new HealthchecksInfo();

        public string[] Tags { get; set; } = Array.Empty<string>();


        public class HealthchecksInfo
        {
            public ActiveInfo Active { get; set; } = new ActiveInfo();

            public PassiveInfo Passive { get; set; } = new PassiveInfo();


            public class ActiveInfo
            {
                public bool Https_verify_certificate { get; set; } = true;

                public UnhealthyInfo Unhealthy { get; set; } = new UnhealthyInfo();

                public string Http_path { get; set; } = "/";

                public int Timeout { get; set; } = 1;

                public HealthyInfo Healthy { get; set; } = new HealthyInfo();

                public string Https_sni { get; set; }

                public int Concurrency { get; set; } = 10;

                public string Type { get; set; } = "http";

                public class UnhealthyInfo
                {
                    public int[] Http_statuses { get; set; } = new[] {429, 404, 500, 501, 502, 503, 504, 505};

                    public int Tcp_failures { get; set; }

                    public int Timeouts { get; set; }

                    public int Http_failures { get; set; }

                    public int Interval { get; set; }
                }


                public class HealthyInfo
                {
                    public int[] Http_statuses { get; set; } = new[] {200, 302};

                    public int Interval { get; set; }

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

                    public int[] Http_statuses { get; set; } = new[] {429, 500, 503};

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