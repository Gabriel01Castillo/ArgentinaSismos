using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using LogUtility;

namespace MailManagement
{
    public static class MailManager 
    {
        public static void SendErrorMail(string  msj) {

            try            {
              
                MailAddress addressTo = new MailAddress(ConfigurationParameters.AddressTo);
                MailAddress addressFrom = new MailAddress(ConfigurationParameters.SMTPEmail, ConfigurationParameters.SMTPDisplayNameSendEmail);
                MailMessage message = new MailMessage(addressFrom, addressTo);
                message.Subject = ConfigurationParameters.AsuntoEmailSuggestion;
                StringBuilder body = new StringBuilder();

                body.AppendLine(" <table  cellspacing='10' style='font-family: Arial, Helvetica, sans-serif;color: #a8bcbe;font-size: 13px;text-decoration: none;' >");
               
                body.AppendLine("<tr>");
                body.AppendLine("<td>El error es el siguiente:<td>");
                body.AppendLine("</tr>");
                body.AppendLine("<tr>");
                body.AppendLine("<td>");
                body.AppendLine("<span style='font-weight:bold'>" + msj + "</span>");
                body.AppendLine("</td>");
                body.AppendLine("</tr>");               
                body.AppendLine("</table>");



                message.IsBodyHtml = true;
                message.Body = body.ToString();
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = ConfigurationParameters.SMTPHost;
                    smtp.Port = ConfigurationParameters.SMTPPort;
                    smtp.EnableSsl = ConfigurationParameters.SMTPEnableSSL;
                    smtp.Credentials = new System.Net.NetworkCredential(ConfigurationParameters.SMTPUserName, ConfigurationParameters.SMTPPassword);
                    smtp.Send(message);
                }

            }
            catch (Exception ex)
            {
                ExceptionUtility.ErrorMail(ex, null);
            }

        }
    }
}