using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Kong.Aspnetcore.AdminApi
{
    [DebuggerDisplay("Target = {Target}")]
    public class KongTargetEdit
    {
        /// <summary>
        /// The target address (ip or hostname) and port
        /// </summary>
        [Required]
        public string Target { get; set; }

        public int Weight { get; set; } = 100;

        public string[] Tags { get; set; } = Array.Empty<string>();

        public KongObject Upstream { get; set; }
    }
}
