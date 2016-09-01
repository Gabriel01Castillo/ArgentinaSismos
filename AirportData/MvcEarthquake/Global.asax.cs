using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.Mail;
using System.Web.Caching;
using System.Web.SessionState;
using System.IO;
using System.Messaging;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Optimization;
using MvcEarthquake.Businnes;
using System.Data.Entity;
using DataAccessEF;
using DataAccessEF.Implementation;
using System.Collections.Generic;
using MvcEarthquake.Businnes.Factories;
using TestApp;
using MvcEarthquake.Utils;
using DataAccessEF.Factories;
using log4net;
using System.Reflection;
using LogUtility;
using MvcEarthquake.Controllers;
using GlobalAppData;


namespace MvcEarthquake
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        IEarthquakeExpert earthquakeExpert;
        HomeController home;

        private System.Object lockCollectionEarthquakes = new System.Object();
        private System.Object lockNotification= new System.Object();
        private System.Object lockCollectionTweeters = new System.Object();
        private System.Object lockDeleteEarthquakes = new System.Object();
        private System.Object lockDeleteDuplicatesEarthquakes = new System.Object();
        private System.Object lockDeleteLogs = new System.Object();
        private System.Object lockDeleteTweeters = new System.Object();
              

        public MvcApplication()
		{
           
		}	



        protected void Application_Start()
        {
            try
            {
                AreaRegistration.RegisterAllAreas();
                WebApiConfig.Register(GlobalConfiguration.Configuration);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                AuthConfig.RegisterAuth();
                
                RouteTable.Routes.MapRoute("Robots.txt", "robots.txt",  new { controller = "Home", action = "Robots" });
              
                home = new HomeController();
                HomeController.SetDefaultDirectory();
                
		
                Database.SetInitializer(/*new DropCreateDatabaseIfModelChangesInitializer()*/new CreateIfNoExistInitilizer());


                ExceptionUtility.Info("iniciando ArgetinaSismos..." + GlobalWebData.ToUniversalTime());
                

                ISourceRepository sourceRepository = new SourceRepository(new AplicationContextFactory());
                var appRepository = new EarthquakeRepository(new AplicationContextFactory());
                

                  //--------------------------------------------------------------------------------------------------------

               
                /*for (int i = 100; i < 30000; i++) { 
                
                    RegistrationDevice rd = new RegistrationDevice();
                    rd.Date = DateTime.Now;
                    rd.DeviceId = "359718776788248"+i;
                    rd.RegistrationId = i+"reEq5wZykVbsOq_oPL7ZRBjyZJ3XIMSuCCDtipam8dYI4Rko0an0nw1-SjX-bRDrVVcaXGj9Koau4RkPAKM5k5We4qG_Xi-I04VdQAhteqPMVAYC2Xf7HlFeT3NCBKPMf_jTvRRaKsZ0BcXqOHZsu7MnLU3NCw";
                    sourceRepository.DbContext.RegistrationDevices.Add(rd);
                    sourceRepository.Save();

                }*/
                
               
                    //--------------------------------------------------------------------------------------------------------
               
                CollectorFactory collectorFactory = new CollectorFactory(sourceRepository);

              
                var inpresEarthquakeCollectorHtml = collectorFactory.GetInpresEarthquakeCollectorHtml();
                var eMSCEarthquakeCollectorXML = collectorFactory.GetEMSCEarthquakeCollectorXML();
                var ssuchEarthquakeCollectorHtml = collectorFactory.GetSSUCHEarthquakeCollectorHtml();

                IDictionary<string, EarthquakeCollector> collectors = new Dictionary<string, EarthquakeCollector>();
                collectors.Add("InpresEarthquakeCollectorHtml", inpresEarthquakeCollectorHtml);
                collectors.Add("EMSCEarthquakeCollectorXML", eMSCEarthquakeCollectorXML);
                collectors.Add("SSUCHEarthquakeCollectorHtml", ssuchEarthquakeCollectorHtml);

                earthquakeExpert = new EarthquakeExpert(collectors, new EarthquakeRepository(new AplicationContextFactory()), 
                                                                                new AplicationContextFactory(),new MylogRepository(new AplicationContextFactory()), new TwitterCollector() );
                //--------------------------------------------------------------------------------------------

                Scheduler.Run("test", 1, RunScheduledTasks);
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex,this.GetType());
                var message = ExceptionUtility.GetExceptionData(ex, this.GetType());                             
            }
        }

      
        public void RunScheduledTasks() {

            try
            {
                    
                lock (lockCollectionEarthquakes)
                {
                    earthquakeExpert.CollectData();
                }

                lock (lockNotification)
                {
                    earthquakeExpert.NotifySensibleEarthquakes();
                }

                lock (lockCollectionTweeters)
                {
                    earthquakeExpert.CollectTwitterData();
                }

                lock (lockDeleteEarthquakes)
                {
                    earthquakeExpert.DeleteEarthquakesBeforeTwoWeeks();
                }

                /*lock (lockDeleteDuplicatesEarthquakes)
                {
                    earthquakeExpert.DeleteDuplicateEarthQuakes();
                }*/

                lock (lockDeleteLogs)
                {
                    earthquakeExpert.DeleteUnusedLogs();
                }

                lock (lockDeleteTweeters)
                {
                    earthquakeExpert.DeleteEarliesTweeters();
                }

                ExceptionUtility.Info("ciclo completo..." + GlobalWebData.ToUniversalTime());
            }

            catch(Exception ex){
                ExceptionUtility.Error(ex, this.GetType());              
            }
        }
                
        protected void Session_Start(Object sender, EventArgs e)
        {
           
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            
        }

        protected void Application_EndRequest(Object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {

        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            ExceptionUtility.Error(Server.GetLastError().Message);  
        }

        protected void Session_End(Object sender, EventArgs e)
        {

        }

        protected void Application_End(Object sender, EventArgs e)
        {
            ExceptionUtility.Warn("Terminando Argentina Sismos.... " + GlobalWebData.ToUniversalTime()); 
            KeepAliveManager.KeepWakeUp();
        }       
        
    }
}