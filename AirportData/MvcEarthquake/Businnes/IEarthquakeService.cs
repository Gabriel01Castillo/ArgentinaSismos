using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcEarthquake.Businnes
{
    public interface IEarthquakeService
    {
        string GetAll();
        object GetData(string location, string latitude, string longitude, string magnitude, string date);
        object GetTweets(string days);

        bool RegisterIfNotExist(string registerId, String deviceId);

        bool Unregister(string deviceId);

        void ResetMailCount();

        void Log(string log, string deviceId);
    }
}
