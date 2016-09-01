using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoJSON;

namespace MvcEarthquake.GeoJsonData
{
    public class SourceFeature : PointFeature<SourceProperties>
    {
        private string _ID;
        [Newtonsoft.Json.JsonProperty("id")]
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
    }
}