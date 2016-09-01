using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp;

namespace MvcEarthquake.Businnes.Factories
{
    public class MagnitudeTypeFactory : IMagnitudeTypeFactory
    {
        public MagnitudeType Create(string type)
        {
            return new MagnitudeType(Guid.NewGuid(),type.ToUpper());
        }
    }
}