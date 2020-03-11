using Kong.Aspnetcore.AdminApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace Kong.Aspnetcore
{
    [DebuggerDisplay("AdminApi = {AdminApi}")]
    public class KongOptions
    {
        [Required]
        public Uri AdminApi { get; set; }

        [Required]
        public KongServiceOptions Service { get; set; }

        public KongUpStreamOptions UpStream { get; set; } 
    }
}
