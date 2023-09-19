using BoltAFE.Domain.Admin;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;

namespace BoltAFE.Helpers
{
    public class Helper
    {
        public static string key = WebConfigurationManager.AppSettings["EncrytionKey"];
        public static string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789#$!@*";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string SendEmail(string userName, string Password, bool flag, IAdminRepository adminRepository)
        {
            try
            {
                string subject = "Login Credentials for Bolt AFE  System";
                string bodyString = $@"<p>Hello {userName},<p>
                    <br>
                    {(flag == true ? "<p>Welcome to Bolt AFE  System.<p>" : "<p>Your Password has been Reset.</p>")}
                    
                    <p>Your Login Credentials are mentioned below.<p>
                   
                    <p>UserName: {userName}<p>
                    <p>Password: {Password}<p>
                    <br>
                    <p>Thanks & Regards,</p>
                    <p>Bolt AFE </p>";
                return adminRepository.SendEmail(bodyString, userName, subject);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string SendEmailOfAFEApprrove(string receiverUserEmail, string senderUserEmail,string afeName, string afeNumber, IAdminRepository adminRepository)
        {
            try
            {
                string subject = "You have an new AFE to approve";
                string bodyString = $@"<p>Hello {receiverUserEmail},<p>
                    <br>
                    <p>You have new AFE received for approve or reject. which is created by {senderUserEmail}.
                    <br/>
                    <p><b>Afe Name: </b>{afeName}.</p>
                    <p><b>Afe Number: </b>{afeNumber}.</p>
                    Please review and reject or approve it.<p>
                    <br>
                    <p>Thanks & Regards,</p>
                    <p>Bolt AFE </p>";
                return adminRepository.SendEmail(bodyString, receiverUserEmail, subject);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string SendEmailOfAFEApprroved(string receiverUserEmail, string senderUserEmail, string afeName, string afeNumber, IAdminRepository adminRepository)
        {
            try
            {
                string subject = $"{afeName} AFE approved";
                string bodyString = $@"<p>Hello {receiverUserEmail},<p>
                    <br>
                    <p><b>Afe Name: </b>{afeName}.</p>
                    <p><b>Afe Number: </b>{afeNumber}.</p>
                    This AFE is approved by {senderUserEmail}.<p>
                    <br>
                    <p>Thanks & Regards,</p>
                    <p>Bolt AFE </p>";
                return adminRepository.SendEmail(bodyString, receiverUserEmail, subject);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
      
    }
}