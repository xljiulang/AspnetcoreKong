using System;
using System.Collections.Generic;
using System.Text;

namespace Kong.Aspnetcore.AdminApi
{
    public class KongArrayData<TData>
    {
        public TData[] Data { get; set; }

        public string Next { get; set; }
    }    

}
