using BoltAFE.Domain.Admin;
using System.Net.Mail;
using System.Net;
using System.Web.Configuration;
using System;
using BoltAFE.Helpers;
using BoltAFE.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BoltAFE.Repositories.Admin
{
    public class AdminRepository : IAdminRepository
    {
        string connectionString;
        public AdminRepository()
        {
            connectionString = WebConfigurationManager.AppSettings["sqldb_connection"];
        }
        public string SendEmail(string bodyString, string userMail, string subject)
        {
            if (SendEmailUsingSmtp(bodyString, userMail, subject))
                return "Email sent successfully";
            else
                return "Email not sent";

        }
        public bool SendEmailUsingSmtp(string bodyString, string userMail, string subject)
        {
            bool result = false;
            try
            {
                string UserName = WebConfigurationManager.AppSettings["UserName"];
                string Password = WebConfigurationManager.AppSettings["Password"];
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(UserName);
                string[] singleEmail = userMail.Split(',');
                foreach (string email in singleEmail)
                {
                    message.To.Add(new MailAddress(email));
                }
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = bodyString;
                smtp.Port = 587;
                smtp.Host = "smtp.office365.com"; //for microsoft outlook host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(UserName, Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                result = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        #region Category

        public bool SaveCategory(string categoryName)
        {
            string query = string.Empty;
            try
            {
                query = $"IF NOT EXISTS (SELECT * FROM [Afe_category] where Category = '{categoryName}') BEGIN INSERT INTO[Afe_category] VALUES('{categoryName}')  END";
                int inserted = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                return true;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log(" SaveCategory =>", ex.Message + "==>" + ex.StackTrace, true);
                return false;
            }
        }

        public bool DeleteCategory(int categoryID)
        {
            string query = string.Empty;
            try
            {
                query = $"DELETE FROM [Afe_category] WHERE Afe_category_id = {categoryID}";
                int deleted = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                return true;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log(" DeleteCategory  =>", ex.Message + "==>" + ex.StackTrace, true);
                return false;
            }
        }

        public bool UpdateCategory(string categoryArr)
        {
            try
            {
                string query = string.Empty;
                int updated = 0;
                var category = JsonConvert.DeserializeObject<List<CategoryModel>>(categoryArr);
                for (int i = 0; i < category.Count; i++)
                {
                    query += $"IF NOT EXISTS (SELECT * FROM [Afe_category] where Category = '{category[i].Category}') BEGIN UPDATE [Afe_category] SET [Category] = '{category[i].Category}' WHERE Afe_category_id ={category[i].Afe_category_id} END;";
                }
                updated = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                return updated > 0 ? true : false;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("UpdateCategory =>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
        }

        #endregion
    }
}