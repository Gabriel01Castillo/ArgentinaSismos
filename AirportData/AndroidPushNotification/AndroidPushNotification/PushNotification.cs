/* Project Name   : PushNotification
 * Module Name    : -
 * Description    : This classfile is used for push notification methods.
 * -----------------------------------------------------------------------------------------
 * DATE           | ID/ISSUE | AUTHOR           | REMARKS
 * -----------------------------------------------------------------------------------------
 * 02-Mar-2012    | 1        | Vimal Panara     | Implementing Android Push Notification 
 * -----------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PushNotification
{
    public class PushNotification
    {

        /// <summary>
        /// This method is used to integrate Android Push Notification
        /// </summary>
        /// <param name="RegistrationID">Registration ID or Token</param>        
        /// <param name="Message">Push Message</param>
        /// <returns>Status=Provided parameter missing the currect value,Authentication Fail, Unauthorized - need new token, Response from web service isn't OK, Success</returns>
        public string Android(string RegistrationID, string Message)
        {          
            Android objAndroid = new Android();
            return objAndroid.SendMessage(RegistrationID, Message);  
          
        }

        /// <summary>
        /// Check Parameter Validation for Android 
        /// </summary>
        /// <returns></returns>
        private bool CheckAndroidValidation(string RegistrationID, string SenderID, string Password, string Message)
        {
            bool RetValue = true;

            if (RegistrationID.Trim() == "")
            {
                RetValue = false;
            }

            if (SenderID.Trim() == "")
            {
                RetValue = false;
            }

            if (Password.Trim() == "")
            {
                RetValue = false;
            }

            if (Message.Trim() == "")
            {
                RetValue = false;
            }

            return RetValue;
        }
    }
}