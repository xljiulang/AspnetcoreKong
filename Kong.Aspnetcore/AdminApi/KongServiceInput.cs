using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace Kong.Aspnetcore.AdminApi
{
    [DebuggerDisplay("Url = {Url}")]
    public class KongServiceInput
    {
        [Required]
        public Uri Url { get; set; }
    }
}
