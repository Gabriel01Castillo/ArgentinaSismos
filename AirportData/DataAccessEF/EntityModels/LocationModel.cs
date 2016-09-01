using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp;

namespace DataAccessEF.EntityModels
{
    public class LocationModel
    {
        public bool IsByCountry { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public CountryEnum [] Country { get; set; }
        public IList<Earthquake> NearEarthquakes { get; set; }

         public LocationModel(){
           Country = new CountryEnum [3];
           NearEarthquakes = new List<Earthquake>();
         }

    }
}
