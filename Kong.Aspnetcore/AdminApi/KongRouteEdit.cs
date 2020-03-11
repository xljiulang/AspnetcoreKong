using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace Kong.Aspnetcore.AdminApi
{
    [DebuggerDisplay("Name = {Name}")]
    public class KongRouteEdit
    {
        public string Name { get; set; }

        public string[] Protocols { get; set; } = new[] { "http", "https" };

        public string[] Methods { get; set; } = Array.Empty<string>();

        public string[] Hosts { get; set; } = Array.Empty<string>();

        [Required]
        public string[] Paths { get; set; } = Array.Empty<string>();

        public int Https_redirect_status_code { get; set; } = 426;

        public int Regex_priority { get; set; } = 0;

        public bool Strip_path { get; set; } = false;

        public bool Preserve_host { get; set; } = false;

        public string[] Tags { get; set; } = Array.Empty<string>();

        public KongObject Service { get; set; }
    }
}
