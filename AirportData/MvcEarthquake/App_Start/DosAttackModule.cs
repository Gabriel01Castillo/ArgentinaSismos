#region Using

using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Timers;
using LogUtility;
using System.Net;

#endregion

/// <summary>
/// Block the response to attacking IP addresses.
/// </summary>
/// 
namespace MvcEarthquake.App_Start
{
    public class DosAttackModule : IHttpModule
    {

        #region IHttpModule Members

        void IHttpModule.Dispose()
        {
            // Nothing to dispose; 
        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        #endregion

        #region Private fields

        private static Dictionary<string, short> _IpAdresses = new Dictionary<string, short>();
        private static Stack<string> _Banned = new Stack<string>();
        private static Timer _Timer = CreateTimer();
        private static Timer _BannedTimer = CreateBanningTimer();

        #endregion

        private const int BANNED_REQUESTS = 10;
        private const int REDUCTION_INTERVAL = 1000; // 1 second
        private const int RELEASE_INTERVAL = 5 * 60 * 1000; // 5 minutes

        private void context_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                
                string tempIp = HttpContext.Current.Request.UserHostAddress;
                string ip =  IPAddress.Parse(tempIp).ToString();
                if (_Banned.Contains(ip))
                {
                    HttpContext.Current.Response.StatusCode = 403;
                    HttpContext.Current.Response.End();
                    ExceptionUtility.Warn(string.Concat("attacking request :",ip, this.GetType()));
                }

                CheckIpAddress(ip);
            }
            catch(Exception ex){

                ExceptionUtility.Error(ex, this.GetType());              
            }
        }

        /// <summary>
        /// Checks the requesting IP address in the collection
        /// and bannes the IP if required.
        /// </summary>
        private static void CheckIpAddress(string ip)
        {
            try
            {
                if (!_IpAdresses.ContainsKey(ip))
                {
                    _IpAdresses[ip] = 1;
                }
                else if (_IpAdresses[ip] == BANNED_REQUESTS)
                {
                    if (_Banned.Count > 0)
                    {
                        _Banned.Push(ip);
                    }
                    
                    _IpAdresses.Remove(ip);
                }
                else
                {
                    _IpAdresses[ip]++;
                }
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(string.Concat(ex.Message,"-->","DosAttackModule"));
            }
        }

        #region Timers

        /// <summary>
        /// Creates the timer that substract a request
        /// from the _IpAddress dictionary.
        /// </summary>
        private static Timer CreateTimer()
        {
            try
            {
                Timer timer = GetTimer(REDUCTION_INTERVAL);
                timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
                return timer;
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(string.Concat(ex.Message, "-->", "DosAttackModule"));
                return new Timer();
            }
        }

        /// <summary>
        /// Creates the timer that removes 1 banned IP address
        /// everytime the timer is elapsed.
        /// </summary>
        /// <returns></returns>
        private static Timer CreateBanningTimer()
        {
            try
            {
                Timer timer = GetTimer(RELEASE_INTERVAL);
                timer.Elapsed += delegate {
                    if (_Banned.Count > 0)
                    {
                        _Banned.Pop();
                    }
                
                };
                return timer;
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(string.Concat(ex.Message, "-->", "DosAttackModule"));
                return new Timer();
            }
        }

        /// <summary>
        /// Creates a simple timer instance and starts it.
        /// </summary>
        /// <param name="interval">The interval in milliseconds.</param>
        private static Timer GetTimer(int interval)
        {
            try
            {
                Timer timer = new Timer();
                timer.Interval = interval;
                timer.Start();

                return timer;
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(string.Concat(ex.Message, "-->", "DosAttackModule"));
                return new Timer();
            }
        }

        /// <summary>
        /// Substracts a request from each IP address in the collection.
        /// </summary>
        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {               
                var listOfIpsToModify = new List<string>();

                foreach (string key in _IpAdresses.Keys)
                {
                    listOfIpsToModify.Add(key);                  
                }
                foreach (var key in listOfIpsToModify)
                {
                    _IpAdresses[key]--;
                    if (_IpAdresses[key] == 0)
                        _IpAdresses.Remove(key);
                }               
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(string.Concat(ex.Message, "DosAttackModule"));
            }
        }

        #endregion

    }
}