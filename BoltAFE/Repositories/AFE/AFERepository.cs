using BoltAFE.Domain.AFE;
using BoltAFE.Helpers;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;

namespace BoltAFE.Repositories.AFE
{
    public class AFERepository : IAFERepository
    {
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
                string query = $"SELECT * FROM [Afe_comments] where Afe_hdr_id = {afeHDRID} and User_id = {userID}   order by Timestamp desc";
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
                string query = $"Declare @docOrder int;SET @docOrder = (select COUNT(*) from [Afe_docs] where Afe_hdr_id = 0);SET @docOrder += 1;INSERT INTO [Afe_docs]([Afe_hdr_id],[User_id],[Doc_path],[Doc_description],[Doc_order])  OUTPUT Inserted.[Afe_doc_id]   VALUES({afeHDRID},{userID},'{filePath}','{docDiscription}',@docOrder)";
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
                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                string query = $"SELECT * FROM [Afe_docs] where Afe_hdr_id = {afeHDRID} order by Doc_order ";
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
                string query = $"SELECT * FROM [Afe_hdr] ah left join Afe_econ_dtl aed on aed.Afe_hdr_id = ah.Afe_hdr_id left join Afe_category ac on ah.Afe_category_id = ac.Afe_category_id left join Afe_type at on ah.Afe_type_id = at.Afe_type_id   left join UserDetail ud  on ud.User_ID = ah.Created_By";
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

        #endregion
    }
}