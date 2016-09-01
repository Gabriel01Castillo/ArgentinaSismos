using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEarthquake.GeoJsonData
{
    public class TweetProperties
    {
        [Newtonsoft.Json.JsonProperty("DateTime")]
        public string DateTime { get; set; }

        [Newtonsoft.Json.JsonProperty("Tweeter")]
        public string Tweeter { get; set; }

        [Newtonsoft.Json.JsonProperty("UserName")]
        public string UserName { get; set; }

         [Newtonsoft.Json.JsonProperty("UserScreenName")]
        public string UserScreenName { get; set; }
    }
}