using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GlobalAppData
{
    public class GlobalWebData
    {
        private static string rootPath;

        private static string publishServer = "h:\\root\\home\\argentinasismos-001\\www\\site1\\bin";

        public static void SetRootPath(string path)
        {
            //rootPath = path;
            rootPath = publishServer;
        }

        public static string GetRootPath(){

            if (Directory.Exists(publishServer)) {
                return publishServer + "\\";
            }
            return "";
        }
        
        public static DateTime ToUniversalTime(){
            var hour = DateTime.UtcNow.Hour;
            var minute = DateTime.UtcNow.Minute;
            var second = DateTime.UtcNow.Second;
            var year = DateTime.UtcNow.Year;
            var month = DateTime.UtcNow.Month;
            var day = DateTime.UtcNow.Day;
            
            return new DateTime(year, month, day, hour, minute, second);
        }

        public static DateTime ToUniversalTime(DateTime date)
        {
            // 4 o 5
            var time = new DateTime(date.Ticks).Subtract(System.TimeSpan.FromHours(System.Convert.ToDouble(5))).ToUniversalTime();
            var hour = time.Hour;
            var minute = time.Minute;
            var second = time.Second;
            var year = time.Year;
            var month = time.Month;
            var day = time.Day;

            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}