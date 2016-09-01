using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp;

namespace MvcEarthquake.Businnes.Factories
{
    public interface IEarthqueakeFactory
    {
        Earthquake Create(DateTime LocalDateTime,decimal Latitude, decimal Longitude, decimal Depth, 
                                             decimal Magnitude,bool isSensible, Place Place, Source source,MagnitudeType magnitudeType); 
    }
}
