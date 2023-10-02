using BoltAFE.Domain.Admin;
using BoltAFE.Domain.UserMaster;
using BoltAFE.Helpers;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace BoltAFE.Controllers
{
    public class UserMasterController : BaseController
    {

        private IUserMasterRepository _userMasterRepository;
        private IAdminRepository _adminRepository;
        public UserMasterController(IUserMasterRepository userMasterRepository, IAdminRepository adminRepository)
        {
            _userMasterRepository = userMasterRepository;
            _adminRepository = adminRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string DeleteUser(int id)
        {
            try
            {
                var result = _userMasterRepository.DeleteUser(id);
                return JsonConvert.SerializeObject(new { IsValid = result, Data = "" });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { IsValid = false, data = ex.Message });
            }
        }

        [HttpPost]
        public string AddNewUser(string userName, string cardMemberName,decimal approverVal , int roleID)
        {
            var result2 = "";
            try
            {
                var result = _userMasterRepository.AddNewUser(userName, cardMemberName, approverVal, roleID);

                if (result.Status)
                {
                    result2 = "User has been Created Successfully. ";
                    try
                    {
                        if (result.Data)
                        {
                            Helper.SendEmail(userName, result.Message, true, _adminRepository);
                        }

                    }
                    catch (Exception ex)
                    {
                        return JsonConvert.SerializeObject(new { IsValid = result.Status, Data = "", Message = result2 + "Email Not Sent." }); ;
                    }
                }
                else
                {
                    result2 = "User has not been created.";
                }
                return JsonConvert.SerializeObject(new { IsValid = result.Status, Data = "", Message = result2 });
            }
            catch (Exception ex)
            {
                result2 = "Something went wrong.PLease try again after some time.";
                return JsonConvert.SerializeObject(new { IsValid = false, data = ex.Message, Message = result2 });
            }
        }

        [HttpGet]
        public string GetAllUsers()
        {
            string allUsers = string.Empty;
            string roles = string.Empty;
            bool isValid = false;
            try
            {
                allUsers = _userMasterRepository.GetAllUsers();
                roles = _userMasterRepository.GetAllRoles();
                isValid = true;

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { IsValid = false, data = ex.Message });
            }
            return JsonConvert.SerializeObject(new
            {
                IsValid = isValid,
                allUsers = allUsers,
                roles = roles
            });
        }

        public string ResetPassword(string UserEmail)
        {
            try
            {
                var result = _userMasterRepository.ResetPassword(UserEmail);
                if (result.Status)
                {
                    Helper.SendEmail(UserEmail, result.Message, false, _adminRepository);
                }
                return JsonConvert.SerializeObject(new { IsValid = result.Status, Data = "", Message = "Password Reset Successful." });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { IsValid = false, data = ex.Message, Message = "Something went wrong. Please try again after some time." });
            }
        }
        public string UpdateUserNames(string userNameArr)
        {
            try
            {
                if (!string.IsNullOrEmpty(userNameArr))
                {
                    var result = _userMasterRepository.UpdateUserNames(userNameArr);
                    return JsonConvert.SerializeObject(new { IsValid = true, Message = "User details updated successfully." });
                }
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("UpdateUserNames =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = false, Message = "Issue occured when try to save user details." });
        }
    }
}