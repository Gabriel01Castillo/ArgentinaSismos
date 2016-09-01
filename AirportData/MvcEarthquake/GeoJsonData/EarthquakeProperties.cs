using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcEarthquake.GeoJsonData
{
    public class EarthquakeProperties
    {

        [Newtonsoft.Json.JsonProperty("UTCDateTime")]
        public string UTCDateTime { get; set; }

        [Newtonsoft.Json.JsonProperty("Depth")]
        public string Depth { get; set; }

        [Newtonsoft.Json.JsonProperty("Magnitude")]
        public string Magnitude { get; set; }

        [Newtonsoft.Json.JsonProperty("IsSensible")]
        public string IsSensible { get; set; }

        [Newtonsoft.Json.JsonProperty("PlaceName")]
        public string PlaceName { get; set; }

        [Newtonsoft.Json.JsonProperty("Country")]
        public string Country { get; set; }
        
        [Newtonsoft.Json.JsonProperty("SourceName")]
        public string SourceName { get; set; }

        [Newtonsoft.Json.JsonProperty("SourceDirection")]
        public string SourceDirection { get; set; }

        [Newtonsoft.Json.JsonProperty("MagnitudeType")]
        public string MagnitudeType { get; set; }
        
      
    }
}
