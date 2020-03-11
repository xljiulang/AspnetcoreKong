using Kong.Aspnetcore.AdminApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kong.Aspnetcore
{
    public class KongUpStreamOptions : KongUpstreamEdit
    {
        public KongTargetEdit[] Targets { get; set; } = Array.Empty<KongTargetEdit>();
    }
}
