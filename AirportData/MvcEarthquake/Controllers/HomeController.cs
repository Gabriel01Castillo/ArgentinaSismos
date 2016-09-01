using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccessEF.Factories;
using DataAccessEF.Implementation;
using GlobalAppData;
using LogUtility;
using MvcEarthquake.Businnes;


namespace MvcEarthquake.Controllers
{
    public class HomeController : Controller
    {
        IEarthquakeService earthquakeService;

        public HomeController() {

            earthquakeService = new EarthquakeService();  
        }
        public ActionResult Index()        {
            
            return View();
        }

        public static void SetDefaultDirectory()
        {

            var path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
            Environment.CurrentDirectory = path;
            Directory.SetCurrentDirectory(path);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Reviews()
        {
            return View();
        }

        public ActionResult Contact()
        {
            /*var dateTime =DateTime.UtcNow;
            var masTres = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0).AddHours(3);
            var global = GlobalWebData.ToUniversalTime(dateTime);*/
            ViewBag.Message = "Your contact page.";

            return View();
           // return Json(new { dateTime = dateTime.ToString(), masTres = masTres.ToString(), global = global.ToString() }, JsonRequestBehavior.AllowGet);
        }

        //http://localhost:5129/Home/GetData?location=3&latitude=-34.1338888889&longitude=-68.7611111111&magnitude=2.0&date=4
        public ActionResult GetData(string location , string latitude, string longitude,string magnitude,string date)
        {   
            return Json(earthquakeService.GetData(location, latitude, longitude, magnitude, date), JsonRequestBehavior.AllowGet);
        }

        public ActionResult WakeUp() {
            ExceptionUtility.Info("Wakeup!!!!..." + GlobalWebData.ToUniversalTime());
            return Json(new { Message = "OK!"}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExternalWakeUp(string site)
        {
            ExceptionUtility.Info(site + " :Wakeup!!!!..."+ GlobalWebData.ToUniversalTime());
            return Json(new { Message = "OK!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTweets(string days) {
            return Json(earthquakeService.GetTweets(days), JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegisterIfNotExist(String registerId, String deviceId)
        {
            bool registered = earthquakeService.RegisterIfNotExist(registerId, deviceId);
            return Json(new { Registered = registered.ToString() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Unregister(String deviceId)
        {
            bool registered = earthquakeService.Unregister(deviceId);
            return Json(new { Registered = registered.ToString() }, JsonRequestBehavior.AllowGet);
        }

        //llamando a este metodo, se vuelve a cero el contador de mail enviados, y el sistema 
        //nuevamente comienza a enviar mails
        public ActionResult ResetMailCount()
        {
            earthquakeService.ResetMailCount();
            return Json(new { Reset = "ok" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupportUs()
        {
            return View();
        }

        public ActionResult Log(String log, String deviceId)
        {
            earthquakeService.Log(log, deviceId);
            return Json(new { LogOk = "ok" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Robots()
        {
            Response.ContentType = "text/plain";
            return View();
        }
    }
}
