using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MailManagment;

namespace LogUtility
{
    public static class ExceptionUtility
    {
        public static string BadRequestMessage = "Bad Request";
        public static string NoElementsMessage = "No Results";
        public static string NotificationErrorMessage = "Notification Problems";

        private static ILog Init()
        {
            ThreadContext.Properties["LogName"] = String.Format("{0}.log", Thread.CurrentThread.ManagedThreadId);

            ILog logger = LogManager.GetLogger(Thread.CurrentThread.ManagedThreadId.ToString());

            log4net.Config.XmlConfigurator.Configure();

            return logger;
        }

        public static void Info(Exception ex, Type classType)
        {

            ILog logger = Init();
            string message = GetExceptionData(ex, classType);
            logger.Info(message);
        }

        public static void Warn(Exception ex, Type classType)
        {
            ILog logger = Init();
            string message = GetExceptionData(ex, classType);
            logger.Warn(message);
        }

        public static void Error(Exception ex, Type classType)
        {           
            string message = GetExceptionData(ex, classType);
            MailManagement.SendErrorMail(message);
            ILog logger = Init();
            logger.Error(message);  
          
        }

        public static void Debug(Exception ex, Type classType)
        {
            ILog logger = Init();
            string message = GetExceptionData(ex, classType);
            logger.Debug(message);
        }

        public static void Fatal(Exception ex, Type classType)
        {
            string message = GetExceptionData(ex, classType);
            MailManagement.SendErrorMail(message);
            ILog logger = Init();           
            logger.Fatal(message);
        }



        public static void Info(string ex)
        {
            ILog logger = Init();
            logger.Info(ex);
        }

        public static void Warn(string ex)
        {
            ILog logger = Init();
            logger.Warn(ex);
        }

        public static void Error(string ex)
        {            
            MailManagement.SendErrorMail(ex);
            ILog logger = Init();
            logger.Error(ex);
        }

        public static void Debug(string ex)
        {
            ILog logger = Init();
            logger.Debug(ex);
        }

        public static void Fatal(string ex)
        {
            MailManagement.SendErrorMail(ex);
            ILog logger = Init();
            logger.Fatal(ex);
        }



        public static string GetExceptionData(Exception ex, Type classType)
        {

            StringBuilder sb = new StringBuilder();
            if (ex.StackTrace != null)
            {
                    var stackTraceLineTemp = ex.StackTrace.Split(':');
                    string stackTraceLine;

                    if (stackTraceLineTemp.Length > 0)
                    {
                        stackTraceLine = stackTraceLineTemp[stackTraceLineTemp.Length - 1];
                    }
                    else
                    {
                        stackTraceLine = "sin definir";
                    }


                    if (ex.InnerException != null && ex.InnerException.InnerException != null)
                    {
                        return sb.Append(ex.InnerException.InnerException.Message).Append("-->").Append(classType).Append("-->").Append(stackTraceLine).ToString();
                    }

                    return sb.Append(ex.Message).Append("-->").Append(classType).Append("-->").Append(stackTraceLine).ToString();
            }

            return sb.Append(ex.Message).Append("-->").Append(classType).ToString();
        }

        public static void ErrorMail(Exception ex, Type type)
        {
            ILog logger = Init();
            logger.Error(ex);
        }
    }
}
