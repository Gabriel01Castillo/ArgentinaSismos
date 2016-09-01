using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessEF.Implementation;
using TestApp;

namespace MvcEarthquake.Businnes
{
    public interface IEarthquakeCollector
    {
         string WebSiteUrl { get; set; }

         IList<Earthquake> Collect();
         
    }
}
