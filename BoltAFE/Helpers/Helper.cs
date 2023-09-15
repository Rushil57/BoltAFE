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
                string subject = "Login Credentials for Bolt Expense  System";
                string bodyString = $@"<p>Hello {userName},<p>
                    <br>
                    {(flag == true ? "<p>Welcome to Bolt Expense  System.<p>" : "<p>Your Password has been Reset.</p>")}
                    
                    <p>Your Login Credentials are mentioned below.<p>
                   
                    <p>UserName: {userName}<p>
                    <p>Password: {Password}<p>
                    <br>
                    <p>Thanks & Regards,</p>
                    <p>Bolt Expense </p>";
                return adminRepository.SendEmail(bodyString, userName, subject);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public static string SendEmailOfSubmitExpense(string receiverMail, string senderUserName, string expenseHDRName, IAdminRepository adminRepository)
        //{
        //    try
        //    {
        //        string subject = $"New report {expenseHDRName} submitted by {senderUserName} for approval.";
        //        string bodyString = $@"<p>Hi {receiverMail},</p>
        //            <br>
        //            <p>I am submitted my expenses claim.</p>
        //            <p> I've attached all the appropriate documents and receipts for you to process my claim.</p>
        //            <p> Please review my expense claim and let me know if you require additional information to ensure it is paid.</p>
        //            <br>
        //            <p>Thanks & Regards,</p>
        //            <p> {senderUserName}</p>
        //            <p>Bolt Expense </p>";
        //        return adminRepository.SendEmail(bodyString, receiverMail, subject);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public static string SendEmailOfApproveExpense(string receiverMail, string senderUserName, string expenseHDRName, IAdminRepository adminRepository)
        //{
        //    try
        //    {
        //        string subject = $"Your report {expenseHDRName} approved by {senderUserName}.";
        //        string bodyString = $@"<p>Hi {receiverMail},</p>
        //            <br>
        //            <p>I am approved your claim.Your claim amount will be credit with in 24 hrs.</p>
        //            <p>If not received after 24 hrs. then let me know about it.</p>
        //            <br>
        //            <p>Thanks & Regards,</p>
        //            <p> {senderUserName}</p>
        //            <p>Bolt Expense </p>";
        //        return adminRepository.SendEmail(bodyString, receiverMail, subject);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public static string SendEmailOfRejectExpense(string receiverMail, string senderUserName, string expenseHDRName, IAdminRepository adminRepository)
        //{
        //    try
        //    {
        //        string subject = $"Your report {expenseHDRName} rejected by {senderUserName}.";
        //        string bodyString = $@"<p>Hi {receiverMail},</p>
        //            <br>
        //            <p>I am rejecting your claim.Please provide more details about it.</p>
        //            <br>
        //            <p>Thanks & Regards,</p>
        //            <p> {senderUserName}</p>
        //            <p>Bolt Expense </p>";
        //        return adminRepository.SendEmail(bodyString, receiverMail, subject);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}