using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp;

namespace MvcEarthquake.Businnes
{
    public interface IEarthquakeExpert
    {
        void CollectData();
        void DeleteEarthquakesBeforeTwoWeeks();       
        void DeleteUnusedLogs();
        void DeleteDuplicateEarthQuakes();
        void DeleteEarliesTweeters();
        void NotifySensibleEarthquakes();
        void CollectTwitterData();
    }
}
