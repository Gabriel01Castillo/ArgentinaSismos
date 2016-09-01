using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeoJSON;


namespace MvcEarthquake.GeoJsonData
{
    public class EarthquakeFeature : PointFeature<EarthquakeProperties>
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
