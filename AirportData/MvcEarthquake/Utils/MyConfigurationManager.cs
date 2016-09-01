using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEarthquake.Utils
{
    public static class MyConfigurationManager
    {
        public static string GetServerIp() {
            return System.Configuration.ConfigurationManager.AppSettings["ServerIP"];
        }

        public static string GetTweeterQuery()
        {
            return System.Configuration.ConfigurationManager.AppSettings["TweeterQuery"];
        }
    }
}