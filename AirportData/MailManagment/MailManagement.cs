using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using GlobalAppData;

namespace MailManagment
{
    public static class MailManagement
    {
        static string  mailFromAddress = "castillo.gabriel.mail@gmail.com";
        static string fromMail = "Argentina sismos"; 
        static string mailToAdress = "castillo.gabriel.mail@gmail.com";
        static string toMail = "Dev";
        static string errorSubject = "ERROR en Argentina Sismos";
        static string password = "AD8dR0PnNFCOnMGeewHpHQ==";
         const string permitted = "12";
        const string count = "0";
       
        public static void SendErrorMail(string msj)
        {
            var mailInfo = GetmailManagementInfo();
            string count ;
            mailInfo.TryGetValue("count",out count);
            int intCount = int.Parse(count);             
            string permitted;
            mailInfo.TryGetValue("permitted", out permitted);
            int intePermitted = int.Parse(permitted);

            if (intePermitted >= intCount)
            {
                SendError(msj);
                intCount++;
                UpdateXMLFile(intCount);
            }
        
        
        }

        public static void ResetCount() {
            UpdateXMLFile(0);
        }

        private static void UpdateXMLFile(int intCount)
        {
            try
            {
                var file = GlobalWebData.GetRootPath() + "mailManagementInfo.xml";

                if (!File.Exists(file))
                {
                    CreateMailManagementInfo();
                }

                XmlDocument myXmlDocument = new XmlDocument();
                myXmlDocument.Load(file);
                XmlNode node;
                node = myXmlDocument.DocumentElement;

                foreach (XmlNode node1 in node.ChildNodes)
                {

                    if (node1.Name == "count")
                    {
                        string newCount = intCount.ToString();
                        node1.InnerText = newCount;
                    }

                }

                myXmlDocument.Save(file);
            }
            catch(Exception ex){
            
            }
        }

        private static void SendError(string msj)
        {
            var fromAddress = new MailAddress(mailFromAddress, fromMail);
            var toAddress = new MailAddress(mailToAdress, toMail);
            string fromPassword = EncryptionService.EncryptManager.Decrypt(password);
            string subject = errorSubject;
            string body = msj;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public static Dictionary<string, string> GetmailManagementInfo()
        {
            Dictionary<string, string> defaults = new Dictionary<string, string>();
            var file = GlobalWebData.GetRootPath() + "mailManagementInfo.xml";
            try
            {
                if (File.Exists(file))
                {

                    defaults = ReadWords();
                }
                else
                {
                    defaults = CreateMailManagementInfo();
                }

                return defaults;

            }
            catch (Exception ex)
            {
                return GetDefaultMailManagementInfo();
            }
        }

        private static  Dictionary<string, string> ReadWords()
        {
            var file = GlobalWebData.GetRootPath() + "mailManagementInfo.xml";
            XElement ele = XElement.Load(file);
            var count = from c in XElement.Load(file).Elements("count") select c;
            var permitted = from p in XElement.Load(file).Elements("permitted") select p;


            Dictionary<string, string> elements = new Dictionary<string, string>();
            elements.Add("count", count.FirstOrDefault().Value);
            elements.Add("permitted", permitted.FirstOrDefault().Value);

           
            return elements;
        }

        private static Dictionary<string, string> GetDefaultMailManagementInfo()
        {            
            
            Dictionary<string, string> elements = new Dictionary<string, string>();
            elements.Add("count", count);
            elements.Add("permitted", permitted);
            return elements;
        }

        private static Dictionary<string, string> CreateMailManagementInfo()
        {

            var file = GlobalWebData.GetRootPath() + "mailManagementInfo.xml";

            Dictionary<string,string> elements = new Dictionary<string,string>();
            elements.Add("count",count) ;
            elements.Add("permitted", permitted);


            CreateFile(file);

            return elements;
        }

        private static void CreateFile(string file)
        {
            try
            {

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(file, settings);
                writer.WriteStartDocument();
                writer.WriteStartElement("MailManagementInfo");

                writer.WriteElementString("count", count);
                writer.WriteElementString("permitted", permitted);


                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            catch(Exception e){
            }
            
        }

        

    }
}
