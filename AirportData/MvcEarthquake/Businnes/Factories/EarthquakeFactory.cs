using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GlobalAppData;
using TestApp;

namespace MvcEarthquake.Businnes.Factories
{
    public class EarthquakeFactory : IEarthqueakeFactory
    {
        public Earthquake Create(DateTime utcTime, decimal latitude, decimal longitude, decimal depth, decimal
                            magnitude, bool isSensible, Place place, Source source, MagnitudeType magnitudeType)
        {
            return new Earthquake(Guid.NewGuid(), utcTime, latitude, longitude, depth, magnitude, isSensible, place, source, magnitudeType, GlobalWebData.ToUniversalTime());
        }
    }
}