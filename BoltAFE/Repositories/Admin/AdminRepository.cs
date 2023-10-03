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

        #region Types

        public bool SaveType(string typeStr)
        {
            string query = string.Empty;
            try
            {
                var type = JsonConvert.DeserializeObject<TypeModel>(typeStr);
                query = $"INSERT INTO [Afe_type]([Type],[Include_gross_afe],[Include_wi],[Include_nri],[Include_roy],[Include_net_afe],[Include_oil],[Include_gas],[Include_ngl],[Include_boe],[Include_po],[Include_pv10],[Include_f_and_d],[Include_ror],[Include_mroi],[Afe_num_code],[CategoryID]) VALUES ('{type.Type}',{type.Include_gross_afe},{type.Include_wi},{type.Include_nri},{type.Include_roy},{type.Include_net_afe},{type.Include_oil},{type.Include_gas},{type.Include_ngl},{type.Include_boe},{type.Include_po},{type.Include_pv10},{type.Include_f_and_d},{type.Include_ror},{type.Include_mroi},'{type.Afe_num_code}',{type.CategoryID})";
                int inserted = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                return true;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log(" SaveType =>", ex.Message + "==>" + ex.StackTrace, true);
                return false;
            }
        }

        public bool DeleteType(int typeID)
        {
            string query = string.Empty;
            try
            {
                query = $"DELETE FROM [Afe_type] WHERE Afe_type_id = {typeID}";
                int deleted = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                return true;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log(" DeleteType  =>", ex.Message + "==>" + ex.StackTrace, true);
                return false;
            }
        }

        public bool UpdateTypes(string typesArr)
        {
            try
            {
                string query = string.Empty;
                int updated = 0;
                var types = JsonConvert.DeserializeObject<List<TypeModel>>(typesArr);
                for (int i = 0; i < types.Count; i++)
                {
                    query += $"UPDATE [Afe_type] SET [Type] = '{types[i].Type}',[Include_gross_afe] = {types[i].Include_gross_afe},[Include_wi] = {types[i].Include_wi},[Include_nri] = {types[i].Include_nri},[Include_roy] ={types[i].Include_roy},[Include_net_afe] = {types[i].Include_net_afe},[Include_oil] = {types[i].Include_oil},[Include_gas] = {types[i].Include_gas},[Include_ngl] ={types[i].Include_ngl},[Include_boe] = {types[i].Include_boe},[Include_po] = {types[i].Include_po},[Include_pv10] = {types[i].Include_pv10},[Include_f_and_d] ={types[i].Include_f_and_d},[Include_ror] = {types[i].Include_ror},[Include_mroi] = {types[i].Include_mroi },[Afe_num_code] = '{types[i].Afe_num_code }',[CategoryID] = {types[i].CategoryID } WHERE Afe_type_id = {types[i].Afe_type_id}";
                }
                updated = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                return updated > 0 ? true : false;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("UpdateTypes =>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
        }
        #endregion
    }
}