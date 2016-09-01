using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessEF.Implementation;
using GlobalAppData;
using TestApp;

namespace MvcEarthquake.Businnes
{
    public class RegistrationManager : IRegistrationManager
    {
        IEarthquakeRepository earthquakeRepository;

        public RegistrationManager(IEarthquakeRepository repository)
        {
            earthquakeRepository = repository;
        }


        public bool RegisterIfNotExist(string registerId, string deviceId)
        {
            if (!string.IsNullOrEmpty(registerId) && !string.IsNullOrEmpty(deviceId))
            {
                var registrationDevices = earthquakeRepository.DbContext.RegistrationDevices.Where(rd => rd.DeviceId == deviceId).FirstOrDefault();

                if (registrationDevices == null) {
                    return Register(registerId,deviceId);
                }
                return true;
            }
            return false;
        }

        private bool Register(string registerId, string deviceId)
        {
            RegistrationDevice registration = new RegistrationDevice();
            registration.Date = GlobalWebData.ToUniversalTime();
            registration.RegistrationId = registerId;
            registration.DeviceId = deviceId;
            earthquakeRepository.DbContext.RegistrationDevices.Add(registration);
            earthquakeRepository.Save();
            return true;
        }


        public bool Unregister(string deviceId)
        {
            if (!string.IsNullOrEmpty(deviceId))
            {
                var registrationDevices = earthquakeRepository.DbContext.RegistrationDevices.Where(rd => rd.DeviceId == deviceId).ToList();

                if (registrationDevices.Count == 0)
                {
                    return true;
                }
                else { 
                  foreach(var rd in registrationDevices){

                      UnregisterDevice(rd);
                  }
                }
                 
            }
            return true;
        }

        private void UnregisterDevice(RegistrationDevice registrationDevices)
        {
            earthquakeRepository.DbContext.RegistrationDevices.Remove(registrationDevices);
            earthquakeRepository.Save();
           
        }
        

     
    }
}