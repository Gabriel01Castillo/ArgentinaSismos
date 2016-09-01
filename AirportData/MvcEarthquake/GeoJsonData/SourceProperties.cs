using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEarthquake.GeoJsonData
{
    public class SourceProperties
    {

        [Newtonsoft.Json.JsonProperty("SourceName")]
        public string SourceName { get; set; }

        [Newtonsoft.Json.JsonProperty("SourceDirection")]
        public string SourceDirection { get; set; }


        [Newtonsoft.Json.JsonProperty("IsOnLine")]
        public string IsOnLine { get; set; }
    }
}