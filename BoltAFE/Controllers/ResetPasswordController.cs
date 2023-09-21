using BoltAFE.Domain.UserMaster;
using BoltAFE.Helpers;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace BoltAFE.Controllers
{
    public class ResetPasswordController : BaseController
    {
        private IUserMasterRepository _userMasterRepository;
        public ResetPasswordController(IUserMasterRepository userMasterRepository)
        {
            _userMasterRepository = userMasterRepository;
        }
        public ActionResult Index()
        {
            return View();
        }
        public string ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                var session = HttpContext.Session;
                if (Convert.ToBoolean(session["ResetPassword"]))
                {
                    oldPassword = Convert.ToString(session["Password"]);
                    oldPassword = Helper.DecryptString(oldPassword);
                }
                var result = _userMasterRepository.ChangePassword(newPassword, oldPassword);

                if (result.Status)
                {
                    session["ResetPassword"] = false;
                }
                return JsonConvert.SerializeObject(new { IsValid = result.Status, Data = "", Message = result.Message });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { IsValid = false, Data = "", Message = ex.Message });
            }
        }
    }
}