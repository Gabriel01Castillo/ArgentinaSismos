using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using GeoJSON;


namespace TestApp
{
    public class MyFeature : PointFeature<MyProperties>
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
