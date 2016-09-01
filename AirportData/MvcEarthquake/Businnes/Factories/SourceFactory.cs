using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp;

namespace MvcEarthquake.Businnes.Factories
{
    public class SourceFactory: ISourceFactory
    {
        public Source Create(string name, string direction)
        {
            return new Source(Guid.NewGuid(),name.ToUpper(),direction);
        }
    }
}