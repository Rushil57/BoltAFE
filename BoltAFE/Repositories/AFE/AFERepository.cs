using BoltAFE.Domain.Admin;
using BoltAFE.Domain.AFE;
using BoltAFE.Helpers;
using BoltAFE.Models;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;

namespace BoltAFE.Repositories.AFE
{
    public class AFERepository : IAFERepository
    {
        private readonly IAdminRepository _adminRepository;

        public AFERepository(IAdminRepository adminRepository)
        {
            this._adminRepository = adminRepository;
        }
        public string GetCategories()
        {
            try
            {
                string query = $"SELECT * FROM [Afe_category]";
                DataTable dt = CommonDatabaseOperationHelper.Get(query);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetCategories =>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
        }

        public string GetTypes()
        {
            try
            {
                string query = $"SELECT * FROM [Afe_type]";
                DataTable dt = CommonDatabaseOperationHelper.Get(query);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetTypes =>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
        }

        #region Comment

        public string GetComments(int afeHDRID, int userID)
        {
            try
            {
                string query = $"SELECT * FROM [Afe_comments]  ac left join UserDetail ud on ac.User_id = ud.User_ID where Afe_hdr_id = {afeHDRID}  order by Timestamp desc";
                DataTable dt = CommonDatabaseOperationHelper.Get(query);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log(" InsertComment=>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
        }
        public bool InsertComment(int afeHDRID, string message, int userID)
        {
            try
            {
                string query = $"INSERT INTO [Afe_comments] ([Afe_hdr_id],[User_id],[Message]) VALUES ({afeHDRID}, {userID},'{message}');";
                int inserted = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                return inserted > 0 ? true : false;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log(" InsertComment=>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
        }
        #endregion

        #region Upload File
        public int CreateFile(int afeHDRID, string docDiscription, string filePath)
        {
            var connection = CommonDatabaseOperationHelper.CreateMasterConnection();
            try
            {
                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                string query = $"Declare @docOrder int;SET @docOrder = (select COUNT(*) from [Afe_docs] where Afe_hdr_id = 0 and User_id = {userID});SET @docOrder += 1;INSERT INTO [Afe_docs]([Afe_hdr_id],[User_id],[Doc_path],[Doc_description],[Doc_order])  OUTPUT Inserted.[Afe_doc_id]   VALUES({afeHDRID},{userID},'{filePath}','{docDiscription}',@docOrder)";
                var ID = connection.Query<int>(query).FirstOrDefault();
                return ID;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("UpdateFileName =>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
            finally { connection.Close(); }
        }

        #endregion

        #region Get Doc
        public string GetDocs(int afeHDRID)
        {
            try
            {
                string query = $"SELECT * FROM [Afe_docs] where Afe_hdr_id = {afeHDRID} ";

                if (afeHDRID == 0)
                {
                    var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                    query += $" and User_id = {userID}";
                }
                query += $"  order by Doc_order ";
                DataTable dt = CommonDatabaseOperationHelper.Get(query);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetDocs =>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
        }
        public bool DeleteDoc(int afeHDRID, int docID)
        {
            try
            {
                string query = $"DELETE FROM [Afe_docs] where Afe_hdr_id = {afeHDRID}  and Afe_doc_id = {docID}";
                if (afeHDRID == 0)
                {
                    var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                    query += $" and User_id = {userID}";
                }
                int deleted = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                return true;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log(" InsertComment=>", ex.Message + "==>" + ex.StackTrace, true);
                return false;
            }
        }


        #endregion

        #region AFE List 
        public string GetAllAFE()
        {
            try
            {
                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                string query = $"SELECT * FROM [Afe_hdr] ah left join Afe_econ_dtl aed on aed.Afe_hdr_id = ah.Afe_hdr_id left join Afe_category ac on ah.Afe_category_id = ac.Afe_category_id left join Afe_type at on ah.Afe_type_id = at.Afe_type_id   left join UserDetail ud  on ud.User_ID = ah.Created_By where Inbox_user_id = {userID}";
                DataTable dt = CommonDatabaseOperationHelper.Get(query);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetAllAFE =>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
        }

        public string GetAFE(int afeHDRID)
        {
            try
            {
                string query = $"SELECT * FROM [Afe_hdr] ah left join Afe_econ_dtl aed on aed.Afe_hdr_id = ah.Afe_hdr_id left join Afe_category ac on ah.Afe_category_id = ac.Afe_category_id left join Afe_type at on ah.Afe_type_id = at.Afe_type_id   left join UserDetail ud  on ud.User_ID = ah.Created_By where ah.Afe_hdr_id = {afeHDRID}";
                DataTable dt = CommonDatabaseOperationHelper.Get(query);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetAFE =>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
        }

        public bool DeleteAFEHDR(int afeHDRID)
        {
            try
            {
                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                string query = $"begin transaction; DELETE FROM [Afe_econ_dtl] where [Afe_hdr_id] = {afeHDRID}; DELETE FROM [Afe_docs] where [Afe_hdr_id] = {afeHDRID}  and [User_id] = {userID} ; DELETE FROM [Afe_comments] where [Afe_hdr_id] = {afeHDRID}  and [User_id] = {userID} ; DELETE FROM [Afe_aprvl_hist_dtl] where [Afe_hdr_id] = {afeHDRID} ;  DELETE FROM [Afe_hdr] where [Afe_hdr_id] = {afeHDRID}  and Created_By = {userID} commit transaction;";
                int deleted = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                return true;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log(" DeleteAFEHDR =>", ex.Message + "==>" + ex.StackTrace, true);
                return false;
            }
        }

        #endregion

        #region Save AFE and DTL
        public bool CheckIfDuplicateAFENum(string afeHDR)
        {
            var connection = CommonDatabaseOperationHelper.CreateMasterConnection();
            try
            {
                var afeHDRValues = JsonConvert.DeserializeObject<AfeHDRModel>(afeHDR);
                string query = $"SELECT COUNT(*) from [Afe_hdr] WHERE Afe_num = '{afeHDRValues.Afe_num}'";
                var count = connection.Query<int>(query).FirstOrDefault();
                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("CheckIfDuplicateAFENum =>", ex.Message + "==>" + ex.StackTrace, true);
                return false;
            }
            finally { connection.Close(); }
        }

        public bool SaveHDRAndDTL(string afeHDR, string afeHDRDTL)
        {
            var connection = CommonDatabaseOperationHelper.CreateMasterConnection();
            try
            {
                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                var userEmail = Convert.ToString(System.Web.HttpContext.Current.Session["Email"]);
                var afeHDRValues = JsonConvert.DeserializeObject<AfeHDRModel>(afeHDR);
                var afeHDRDTLValues = JsonConvert.DeserializeObject<AfeEconDTLModel>(afeHDRDTL);
                string query = $"INSERT INTO [Afe_hdr] ([Afe_name],[Afe_type_id],[Afe_category_id],[Afe_num],[Created_date],[Created_By],[Inbox_user_id]) OUTPUT Inserted.[Afe_hdr_id] VALUES ('{afeHDRValues.Afe_name}',{afeHDRValues.Afe_type_id},{afeHDRValues.Afe_category_id},'{afeHDRValues.Afe_num}','{afeHDRValues.Created_date}',{userID},{afeHDRValues.Inbox_user_id}) ;";
                int afeHDRID = connection.Query<int>(query).FirstOrDefault();
                query = $"INSERT INTO [Afe_aprvl_hist_dtl] ([Afe_hdr_id],[Approver_user_id]) VALUES ({afeHDRID} ,{afeHDRValues.Inbox_user_id}) ; INSERT INTO [Afe_econ_dtl] ([Afe_hdr_id],[Description],[Gross_afe],[Wi],[Nri],[Roy],[Net_afe],[Oil],[Gas],[Ngl],[Boe],[Und_po],[Pv10],[F_and_d],[Ror],[Mroi],[Changed_by_user_id],[Changed_date]) VALUES ({afeHDRID},'{afeHDRDTLValues.Description}',{afeHDRDTLValues.Gross_afe},{afeHDRDTLValues.Wi},{afeHDRDTLValues.Nri},{afeHDRDTLValues.Roy},{afeHDRDTLValues.Net_afe},{afeHDRDTLValues.Oil},{afeHDRDTLValues.Gas},{afeHDRDTLValues.Ngl},{afeHDRDTLValues.Boe},{afeHDRDTLValues.Und_po},{afeHDRDTLValues.Pv10},{afeHDRDTLValues.F_and_d},{afeHDRDTLValues.Ror},{afeHDRDTLValues.Mroi},{userID},'{afeHDRDTLValues.Changed_date}') ; UPDATE [Afe_comments] SET [Afe_hdr_id] ={afeHDRID}   where  [User_id] = {userID} and [Afe_hdr_id] = 0 ; UPDATE [Afe_docs] SET [Afe_hdr_id] = {afeHDRID} where [User_id] ={userID} and [Afe_hdr_id] = 0";
                int inserted = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                if (userID != afeHDRValues.Inbox_user_id)
                {
                    Helper.SendEmailOfAFEApprrove(afeHDRValues.Inbox_user_email, userEmail, afeHDRValues.Afe_name, afeHDRValues.Afe_num, _adminRepository);
                }
                return true;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log(" SaveHDRAndDTL =>", ex.Message + "==>" + ex.StackTrace, true);
                return false;
            }
        }
        #endregion
    }
}