using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessEF.Implementation;
using GlobalAppData;
using TestApp;

namespace MvcEarthquake.Businnes
{
    public class DeviceLogManager : IDeviceLogManager
    {
          IEarthquakeRepository earthquakeRepository;

          public DeviceLogManager(IEarthquakeRepository repository)
        {
            earthquakeRepository = repository;
        }

          public void SaveLog(String log, String deviceId) {
              DeviceLog newlog = new DeviceLog();
              newlog.DeviceId = deviceId;
              newlog.Date = GlobalWebData.ToUniversalTime();
              newlog.Log = log;
              earthquakeRepository.DbContext.DevicesLogs.Add(newlog);
              earthquakeRepository.Save();
          
          }

    }
}