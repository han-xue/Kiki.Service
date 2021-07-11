using System;
using System.Collections.Generic;
using System.Text;

namespace Kiki.CourierService
{
    class KikiServiceConfig
    {
        public KikiServiceApiOptions KikiServiceApi { get; set; } 
    }

    public class KikiServiceApiOptions
    {
        public string Host { get; set; }
    }
}
