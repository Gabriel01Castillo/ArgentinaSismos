using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security;

namespace MailManagement
{
    public static class ConfigurationParameters
    {

        public static string SMTPHost
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("SMTPHost");
            }
        }
        public static int SMTPPort
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("SMTPMailPort")))
                    return 0;
                else
                    return Convert.ToInt32(ConfigurationManager.AppSettings.Get("SMTPMailPort"));
            }
        }
        public static string SMTPUserName
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("SMTPUserName");
            }
        }
        public static string SMTPEmail
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("SMTPEmail");
            }
        }
        public static string SMTPDisplayNameSendEmail
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("DisplayNameSendEmail");
            }
        }
        public static System.Security.SecureString SMTPPassword
        {
            get
            {
                string password = ConfigurationManager.AppSettings.Get("SMTPPass");
                if (password == null)
                    throw new ArgumentNullException("password");

               unsafe
                {
                    fixed (char* passwordChars = password)
                    {
                        var securePassword = new SecureString(passwordChars, password.Length);
                        securePassword.MakeReadOnly();
                        return securePassword;
                    }
                }

            }
        }
        public static bool SMTPEnableSSL
        {
            get
            {

                return ConfigurationManager.AppSettings.Get("SMTPEnableSSL") == "1";
            }
        }

        public static string AsuntoEmailSuggestion
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AsuntoEmailSuggestion");
            }
        }



        public static string AddressTo
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AddressTo");
            }
        }
    }
}