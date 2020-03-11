using System;
using System.Collections.Generic;
using System.Text;

namespace Kong.Aspnetcore.AdminApi
{
    public class KongServiceEdit
    {
        public string Name { get; set; }

        public int Retries { get; set; } = 5;

        public string Protocol { get; set; } = "http";

        public string Host { get; set; }

        public int Port { get; set; } = 80;

        public string Path { get; set; }

        public int Connect_timeout { get; set; } = 60000;

        public int Write_timeout { get; set; } = 60000;

        public int Read_timeout { get; set; } = 60000;

        public string[] Tags { get; set; } = Array.Empty<string>();
    }
}
