using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.EntityModels
{
    public  class EarthquakeModel
    {
        public LocationModel LocationModel { get; set; }
        public DateTime DateTime { get; set; }        
        public string Depth { get; set; }        
        public string Magnitude { get; set; }       
        public string IsSensible { get; set; }       
        public string PlaceName { get; set; }      
          
        public string SourceName { get; set; }    
        public string SourceDirection { get; set; }        
        public string MagnitudeType { get; set; }
    }
}
