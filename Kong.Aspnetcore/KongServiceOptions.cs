using Kong.Aspnetcore.AdminApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kong.Aspnetcore
{
    public class KongServiceOptions : KongServiceInput
    {
        public KongRouteEdit[] Routes { get; set; } = Array.Empty<KongRouteEdit>();

        public string GetName()
        {
            return this.Url.Host;
        }
    }
}
