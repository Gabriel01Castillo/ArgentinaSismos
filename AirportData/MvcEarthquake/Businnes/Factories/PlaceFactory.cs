using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp;

namespace MvcEarthquake.Businnes.Factories
{
    public class PlaceFactory: IPlaceFactory
    {
        public Place Create(string place, string country)
        {
            return new Place(Guid.NewGuid(), place.ToUpper(), country.ToUpper());
        }
    }
}