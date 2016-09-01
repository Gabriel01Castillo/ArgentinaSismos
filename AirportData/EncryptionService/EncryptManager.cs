using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace EncryptionService
{
    public class EncryptManager
    {
        private static string saltkey = "KzM0Hpk+9TkJEJmfYLjA8w==";

        public static string CreatePasswordHash(string password, string passwordFormat)
        {

            if (String.IsNullOrEmpty(passwordFormat))
            {
                passwordFormat = PasswordFormat.MD5.ToString();
            }

            string saltAndPassword = String.Concat(password, saltkey);
            string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPassword, passwordFormat);
            return hashedPassword;
        }

        public static string Encrypt(string data)
        {
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    TripleDESCryptoServiceProvider cryptDES3 = new TripleDESCryptoServiceProvider();
                    MD5CryptoServiceProvider cryptMD5Hash = new MD5CryptoServiceProvider();


                    cryptDES3.Key = cryptMD5Hash.ComputeHash(ASCIIEncoding.UTF8.GetBytes(saltkey));
                    cryptDES3.Mode = CipherMode.ECB;
                    ICryptoTransform desdencrypt = cryptDES3.CreateEncryptor();
                    byte[] buff = ASCIIEncoding.UTF8.GetBytes(data);
                    string Encrypt = Convert.ToBase64String(desdencrypt.TransformFinalBlock(buff, 0, buff.Length));
                    Encrypt = Encrypt.Replace("+", "!");
                    return Encrypt;
                }
                return data;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string Decrypt(string data)
        {
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    TripleDESCryptoServiceProvider cryptDES3 = new TripleDESCryptoServiceProvider();
                    MD5CryptoServiceProvider cryptMD5Hash = new MD5CryptoServiceProvider();

                    data = data.Replace("!", "+");
                    byte[] buf = new byte[data.Length];
                    cryptDES3.Key = cryptMD5Hash.ComputeHash(ASCIIEncoding.UTF8.GetBytes(saltkey));
                    cryptDES3.Mode = CipherMode.ECB;
                    ICryptoTransform desdencrypt = cryptDES3.CreateDecryptor();
                    buf = Convert.FromBase64String(data);
                    string Decrypt = ASCIIEncoding.UTF8.GetString(desdencrypt.TransformFinalBlock(buf, 0, buf.Length));
                    return Decrypt;
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public enum PasswordFormat
        {
            MD5,
            SHA1
        }

    }
}
