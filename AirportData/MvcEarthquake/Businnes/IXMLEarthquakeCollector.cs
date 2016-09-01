using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcEarthquake.Businnes
{
    public interface IXMLEarthquakeCollector
    {
        IWebSourceManagerXML SourceManagerXML { get; set; }       
    }
}
