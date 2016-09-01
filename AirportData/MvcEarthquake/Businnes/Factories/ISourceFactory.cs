using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp;

namespace MvcEarthquake.Businnes.Factories
{
    public interface ISourceFactory
    {
        Source Create(string name, string direction);
    }
}
