using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Place : BaseClass
    {
        public string PlaceName { get; set; }
        public int UTC { get; set; }
        public Country Country { get; set; }
        
        public Place() { }
        
        public Place(string placeName, Country country, int utc)
        {
            PlaceName = placeName;
            Country = country;
            UTC = utc;
        }

        public override string ToString()
        {
            return string.Concat(PlaceName," ", UTC," ",Country);
        }
    }
}
