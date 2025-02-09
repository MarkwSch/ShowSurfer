using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowSurfer.Models
{
    // Class for WatchProviders
    public class WatchProvider
    {
        public string LogoPath { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public int DisplayPriority { get; set; }
        public string ProviderCountryCode { get; set; }
    }
}
