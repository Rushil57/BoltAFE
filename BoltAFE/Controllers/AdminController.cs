using BoltAFE.Domain.Admin;
using BoltAFE.Helpers;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace BoltAFE.Controllers
{
    public class AdminController :  BaseController
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            this._adminRepository = adminRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateEditCategories()
        {
            return View();
        }

        #region Category
        public string SaveCategory(string category)
        {
            string data = "Issue occured when try to save category.";
            bool isValid =false;
            try
            {
                if (!string.IsNullOrEmpty(category))
                {
                    _adminRepository.SaveCategory(category);
                    isValid = true;
                    data = "Category saved successfully.";
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                CommonDatabaseOperationHelper.Log("SaveCategory =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new
            {
                IsValid = isValid,
                data = data
            });
        }
        public string DeleteCategory(int categoryID)
        {
            string data = "Issue occured when try to delete category.";
            bool isValid =false;
            try
            {
                if (categoryID > 0)
                {
                    _adminRepository.DeleteCategory(categoryID);
                    isValid = true;
                    data = "Category deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                CommonDatabaseOperationHelper.Log("DeleteCategory =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new
            {
                IsValid = isValid,
                data = data
            });
        }
        public string UpdateCategory(string categoryArr)
        {
            bool isValid =false;
            string message = "Issue occured when try to update category.";
            try
            {
                if (!string.IsNullOrEmpty(categoryArr))
                {
                    var result = _adminRepository.UpdateCategory(categoryArr);
                    isValid = true;
                    message = "Category updated successfully.";
                }
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("UpdateCategory =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = isValid, Message = message });
        }
        #endregion
    }
}