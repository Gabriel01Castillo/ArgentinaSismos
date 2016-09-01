using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp;

namespace MvcEarthquake.Businnes
{
    public interface ITwitterCollector
    {

        List<Tweet> GetTwitters();
    }
}
