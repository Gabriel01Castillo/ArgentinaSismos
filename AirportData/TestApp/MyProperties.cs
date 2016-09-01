using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class MyProperties
    {
        private string _Elevation;
        [Newtonsoft.Json.JsonProperty("elevation")]
        public string Elevation
        {
            get { return _Elevation; }
            set { _Elevation = value; }
        }

        private string _Name;
        [Newtonsoft.Json.JsonProperty("name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
    }
}
