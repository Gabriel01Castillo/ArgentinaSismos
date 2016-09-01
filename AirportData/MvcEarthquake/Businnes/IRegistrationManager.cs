using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcEarthquake.Businnes
{
    interface IRegistrationManager
    {

        bool RegisterIfNotExist(string registerId, String deviceId);

        bool Unregister(string registerId);
    }
}
