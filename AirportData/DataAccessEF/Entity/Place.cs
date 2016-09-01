using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessEF;

namespace TestApp
{
    public class Place : Entity
    {
        public string PlaceName { get; set; }
        public string Country { get; set; }
        
        public Place() { }
        
        public Place(Guid id,string placeName, string country)
        {
            base.Id = id;
            PlaceName = placeName;
            Country = country;           
        }

        public override string ToString()
        {
            return string.Concat(PlaceName," "," ",Country);
        }
    }
}
