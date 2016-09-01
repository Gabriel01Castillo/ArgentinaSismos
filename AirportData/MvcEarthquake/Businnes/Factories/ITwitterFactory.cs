using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp;

namespace MvcEarthquake.Businnes.Factories
{
    interface  ITwitterFactory
    {
        Tweet Create(string userName, string screenName, string tweet, DateTime dateTime);
    }
}
