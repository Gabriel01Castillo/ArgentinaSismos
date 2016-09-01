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
        /// <param name="SenderID">Google EmailID</param>
        /// <param name="Password">Password of EmailID</param>
        /// <param name="Message">Push Message</param>
        /// <returns>Status=Provided parameter missing the currect value,Authentication Fail, Unauthorized - need new token, Response from web service isn't OK, Success</returns>
        public string Android(string RegistrationID, string SenderID, string Password, string Message)
        {
            string Status = "";
            //--Validating the required parameter--//
            if (CheckAndroidValidation(RegistrationID, SenderID, Password, Message) == false)
            {
                Status = "Provided parameter missing the currect value.";
            }
            else
            {
                //-- Check Authentication --//
                Android objAndroid = new Android();
                string AuthString = objAndroid.CheckAuthentication(SenderID, Password);

                if (AuthString == "Fail")
                {
                    Status = "Authentication Fail";
                }
                else
                {
                    Status = objAndroid.SendMessage(RegistrationID, Message, AuthString);
                }

            }

            //-- Return the Status of Push Notification --//
            return Status;
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