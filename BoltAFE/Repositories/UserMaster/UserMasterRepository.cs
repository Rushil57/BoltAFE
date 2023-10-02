using BoltAFE.Domain.UserMaster;
using BoltAFE.Helpers;
using BoltAFE.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace BoltAFE.Repositories.UserMaster
{
    public class UserMasterRepository : IUserMasterRepository
    {
        public CommonResponseModel<bool> AddNewUser(string userName, string cardMemberName,decimal approverVal,int roleID)
        {
            CommonResponseModel<bool> responseModel = new CommonResponseModel<bool>();
            try
            {
                string password = Helper.RandomString(10);
                password = Helper.EncryptString(password);
                string query1 = $"SELECT u.User_ID,u.Password FROM UserDetail u WHERE u.user_email=@UserName ORDER BY u.User_ID";
                DataTable dt = CommonDatabaseOperationHelper.Get_Master(query1, new { @UserName = userName });
                if (dt.Rows.Count == 0)
                {
                    var query2 = $"INSERT INTO UserDetail (user_email, password,reset_password,User_Name,RoleID,Approver_amount) VALUES (@UserName,@Password,1,@cardMemberName,@roleID,@ApproverAmount)";
                    CommonDatabaseOperationHelper.InsertUpdateDelete_Master(query2, new { @UserName = userName, @Password = password, @cardMemberName = cardMemberName, @roleID = roleID, @ApproverAmount = approverVal });
                    responseModel.Data = true;
                }
                responseModel.Status = true;
                responseModel.Message = Helper.DecryptString(password);
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message;
                return responseModel;
            }
        }

        public CommonResponseModel<bool> CheckifUserExist(string userEmail)
        {
            CommonResponseModel<bool> responseModel = new CommonResponseModel<bool>();
            try
            {

                string query1 = $"SELECT u.User_ID,u.Password FROM UserDetail u WHERE u.user_email=@UserName ORDER BY u.User_ID";
                DataTable dt = CommonDatabaseOperationHelper.Get_Master(query1, new { @UserName = userEmail });
                if (dt.Rows.Count == 0)
                {
                    responseModel.Status = false;
                    responseModel.Data = false;
                    responseModel.Message = "No User Found.";
                }
                else
                {
                    responseModel.Status = true;
                    responseModel.Data = true;
                    responseModel.Message = "User Email already exists.";
                }
            }
            catch (Exception ex)
            {
                responseModel.Status = true;
                responseModel.Data = true;
                responseModel.Message = ex.Message;
            }
            return responseModel;
        }
        public CommonResponseModel<string> CreateNewUser(string userEmail)
        {
            CommonResponseModel<string> responseModel = new CommonResponseModel<string>();
            try
            {
                string password = Helper.RandomString(10);
                password = Helper.EncryptString(password);
                var query2 = $"INSERT INTO UserDetail (user_email, password,reset_password) VALUES (@UserName,@Password,1);";
                CommonDatabaseOperationHelper.InsertUpdateDelete_Master(query2, new { @UserName = userEmail, @Password = password });
                responseModel.Status = true;
                responseModel.Data = Helper.DecryptString(password);
                responseModel.Message = "User Created Successfully.";
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message;
            }
            return responseModel;
        }
        public ResponseModel ResetPassword(string UserEmail)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                string query1 = $"SELECT u.User_ID,u.password FROM UserDetail u WHERE u.user_email=@UserEmail ORDER BY u.User_ID";
                DataTable dt = CommonDatabaseOperationHelper.Get_Master(query1, new { @UserEmail = UserEmail });
                if (dt.Rows.Count > 0)
                {
                    string newPassword = Helper.RandomString(10);
                    newPassword = Helper.EncryptString(newPassword);
                    var query = $"UPDATE UserDetail SET password =@Password, reset_password = 1 Where user_email= @UserEmail";
                    CommonDatabaseOperationHelper.InsertUpdateDelete_Master(query, new { @Password = newPassword, @UserEmail = UserEmail });
                    responseModel.Status = true;
                    responseModel.Message = Helper.DecryptString(newPassword);
                }

                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message;
                return responseModel;
            }
        }

        public ResponseModel ChangePassword(string newPassword, string oldPassword)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                var userId = Convert.ToString(HttpContext.Current.Session["Email"]);
                string query1 = $"SELECT u.User_ID,u.password FROM UserDetail u WHERE u.user_email=@UserId ORDER BY u.User_ID";
                DataTable dt = CommonDatabaseOperationHelper.Get_Master(query1, new { @UserId = userId });
                if (dt.Rows.Count > 0)
                {
                    var cmpPassword = dt.Rows[0]["Password"];
                    cmpPassword = Helper.DecryptString(Convert.ToString(cmpPassword));
                    if (cmpPassword.Equals(oldPassword))
                    {
                        newPassword = Helper.EncryptString(newPassword);
                        var query = $"UPDATE UserDetail SET password =@Password, reset_password = 0 Where user_email= @UserId";
                        CommonDatabaseOperationHelper.InsertUpdateDelete_Master(query, new { @Password = newPassword, @UserId = userId });
                        responseModel.Status = true;
                        responseModel.Message = "Password Changed Successfully.";
                    }
                    else
                    {
                        responseModel.Status = false;
                        responseModel.Message = "Old Password is Invalid.";
                    }
                }
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.Message = ex.Message;
                return responseModel;
            }
        }
        public string GetAllUsers()
        {
            try
            {
                string query = $"SELECT  ud.*,r.RoleName as role FROM UserDetail ud  left join Roles r on ud.RoleID = r.roleID";
                DataTable dt = CommonDatabaseOperationHelper.Get(query);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string GetAllRoles()
        {
            try
            {
                string query = $"SELECT * FROM [Roles] order by RoleName desc";
                DataTable dt = CommonDatabaseOperationHelper.Get(query);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetAllRoles =>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
        }

        public bool DeleteUser(int id)
        {
            try
            {
                var query = $"select USER_EMAIL from UserDetail where User_ID=@Id";
                var deleteEmail = CommonDatabaseOperationHelper.Scalar(query, new { @Id = id });
                
                query = $"DELETE FROM UserDetail WHERE User_ID = @Id; ";
                CommonDatabaseOperationHelper.InsertUpdateDelete(query, new { @Id = id });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateUserNames(string userNameArr)
        {
            var connection = CommonDatabaseOperationHelper.CreateMasterConnection();
            try
            {
                string query = string.Empty;
                int updated = 0;
                var users = JsonConvert.DeserializeObject<List<UserMasterModel>>(userNameArr);
                for (int i = 0; i < users.Count; i++)
                {
                    query += $"Update [UserDetail] set User_Name = '{users[i].UserName}' ,  Approver_amount ={users[i].Approver}, RoleID = {users[i].RoleID} where User_email = '{users[i].UserEmail}';";
                }
                updated = CommonDatabaseOperationHelper.InsertUpdateDelete(query);
                return updated > 0 ? true : false;
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("UpdateUserNames =>", ex.Message + "==>" + ex.StackTrace, true);
                throw;
            }
            finally { connection.Close(); }
        }
    }
}