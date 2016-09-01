using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Caching;
using LogUtility;

namespace MvcEarthquake.Businnes
{
    public  class KeepAliveManager
    {
        private static KeepAliveManager instance;
     private static object sync = new object();
     private string _applicationUrl;
     private string _cacheKey;

     private KeepAliveManager(string applicationUrl)
     {
         _applicationUrl = applicationUrl;
         _cacheKey = Guid.NewGuid().ToString();
         instance = this;
     }

     public static bool IsKeepingAlive
     {
         get
         {
             lock (sync)
             {
                 return instance != null;
             }
         }
     }

     public static void Start(string applicationUrl)
     {
         if(IsKeepingAlive)
         {
             return;
         }
         lock (sync)
         {
             instance = new KeepAliveManager(applicationUrl);
             instance.Insert();
         }
     }

     public static void Stop()
     {
         lock (sync)
         {
             HttpRuntime.Cache.Remove(instance._cacheKey);
             instance = null;
         }
     }

     private void Callback(string key, object value, CacheItemRemovedReason reason)
     {
         if (reason == CacheItemRemovedReason.Expired)
         {
             FetchApplicationUrl();
             Insert();
         }
     }

     private void Insert()
     {
         HttpRuntime.Cache.Add(_cacheKey,
             this,
             null,
             Cache.NoAbsoluteExpiration,
             new TimeSpan(0, 10, 0),
             CacheItemPriority.Normal,
             this.Callback);
     }

     private void FetchApplicationUrl()
     {
         try
         {
             HttpWebRequest request = HttpWebRequest.Create(this._applicationUrl) as HttpWebRequest;
             using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
             {
                 HttpStatusCode status = response.StatusCode;
                 //log status
             }
         }
         catch (Exception ex)
         {
               ExceptionUtility.Error(ex.Message);
         }
     }
        public static void KeepAlive()
        {
            while (true)
            {  
                try
                {
                    WebRequest request = WebRequest.Create("http://argentinasismos.com/Home/WakeUp");
                    using (WebResponse resp = request.GetResponse())
                    {
                        
                    }
                    Thread.Sleep(60000);
                }
                catch (ThreadAbortException tae)
                {
                    ExceptionUtility.Error(tae.Message);
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionUtility.Error(ex.Message);
                    break;
                }
            }
        }

        public static void KeepWakeUp()
        {
           
                try
                {
                    WebRequest request = WebRequest.Create("http://argentinasismos.com/Home/WakeUp");
                    using (WebResponse resp = request.GetResponse())
                    {

                    }                    
                }
                
                catch (Exception ex)
                {
                    ExceptionUtility.Error(ex.Message);                  
                }
            
        }
    }
}