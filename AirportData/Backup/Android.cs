/* Project Name   : PushNotification
 * Module Name    : Android Push Notification
 * Description    : This classfile is used for push notification methods.
 * -----------------------------------------------------------------------------------------
 * DATE           | ID/ISSUE | AUTHOR           | REMARKS
 * -----------------------------------------------------------------------------------------
 * 02-Mar-2012    | 1        | Vimal Panara     | Implementing Android Push Notification Methods
 * -----------------------------------------------------------------------------------------
 */


using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Collections;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Specialized;

namespace PushNotification
{
    //Start: #1
    partial class Android
    {
      
       /// <summary>
       /// Check authentication with supplied credential
       /// </summary>
       /// <param name="SenderID">Google EmailID</param>
       /// <param name="Password">Password of EmailID</param>
       /// <returns></returns>
        public string CheckAuthentication(string SenderID, string Password)
        {
            string Array = "";

            string URL = "https://www.google.com/accounts/ClientLogin?";
            string fullURL = URL + "Email=" + SenderID.Trim() + "&Passwd=" + Password.Trim() + "&accountType=GOOGLE" + "&source=Company-App-Version" + "&service=ac2dm";
            HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create(fullURL);

            try
            {
            
            //-- Post Authentication URL --//
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
            StreamReader Reader;
            int Index = 0;
           

           
                //-- Check Response Status --//
                if (Response.StatusCode == HttpStatusCode.OK)
                {
                    Stream Stream = Response.GetResponseStream();
                    Reader = new StreamReader(Stream);
                    string File = Reader.ReadToEnd();
                    
                    Reader.Close();
                    Stream.Close();
                    Index = File.ToString().IndexOf("Auth=") + 5;
                    int len = File.Length - Index;
                    Array = File.ToString().Substring(Index, len);
                }

            }
            catch (Exception ex)
            {   
                Array = ex.Message;
                ex = null;
            }

            return Array;
        }

        /// <summary>
        /// Send Push Message to Device
        /// </summary>
        /// <param name="RegistrationID">RegistrationID or Token</param>
        /// <param name="Message">Message to be sent on device</param>
        /// <param name="AuthString">Authentication string</param>
        public string SendMessage(string RegistrationID, string Message, string AuthString)
        {            
            //-- Create C2DM Web Request Object --//
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.clients.google.com/c2dm/send");
            Request.Method = "POST";
            Request.KeepAlive = false;

            //-- Create Query String --//
            NameValueCollection postFieldNameValue = new NameValueCollection();
            postFieldNameValue.Add("registration_id", RegistrationID);
            postFieldNameValue.Add("collapse_key", "1");
            postFieldNameValue.Add("delay_while_idle", "0");
            // postFieldNameValue.Add("data.message", Message);
            postFieldNameValue.Add("data.payload", Message);           
            string postData = GetPostStringFrom(postFieldNameValue);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);


            Request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            Request.ContentLength = byteArray.Length;

            Request.Headers.Add(HttpRequestHeader.Authorization, "GoogleLogin auth=" + AuthString);

            //-- Delegate Modeling to Validate Server Certificate --//
            ServicePointManager.ServerCertificateValidationCallback += delegate(
                        object
                        sender,
                        System.Security.Cryptography.X509Certificates.X509Certificate
                        pCertificate,
                        System.Security.Cryptography.X509Certificates.X509Chain pChain,
                        System.Net.Security.SslPolicyErrors pSSLPolicyErrors)
            {
                return true;
            };

            //-- Create Stream to Write Byte Array --// 
            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
           
            //-- Post a Message --//
            WebResponse Response = Request.GetResponse();
            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
            {                
                return "Unauthorized - need new token";

            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                return "Response from web service isn't OK";
                //Console.WriteLine("Response from web service not OK :");
                //Console.WriteLine(((HttpWebResponse)Response).StatusDescription);
            }

            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string responseLine = Reader.ReadLine();
            Reader.Close();

            return responseLine;
        }

        /// <summary>
        /// Create Query String From Name Value Pair
        /// </summary>
        /// <param name="postFieldNameValue"></param>
        /// <returns></returns>
        private string GetPostStringFrom(NameValueCollection postFieldNameValue)
        {
            //throw new NotImplementedException();
            List<string> items = new List<string>();

            foreach (String name in postFieldNameValue)
                items.Add(String.Concat(name, "=", System.Web.HttpUtility.UrlEncode(postFieldNameValue[name])));

            return String.Join("&", items.ToArray());
        }

        /// <summary>
        /// Validate Server Certificate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="policyErrors"></param>
        /// <returns></returns>
        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }
    //End: #1

}

