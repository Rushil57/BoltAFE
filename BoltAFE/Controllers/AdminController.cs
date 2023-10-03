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


        #region Types
        public string SaveType(string type)
        {
            string data = "Issue occured when try to save type.";
            bool isValid = false;
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    _adminRepository.SaveType(type);
                    isValid = true;
                    data = "Type saved successfully.";
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                CommonDatabaseOperationHelper.Log("SaveType =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new
            {
                IsValid = isValid,
                data = data
            });
        }
        public string DeleteType(int typeID)
        {
            string data = "Issue occured when try to delete type.";
            bool isValid = false;
            try
            {
                if (typeID > 0)
                {
                    _adminRepository.DeleteType(typeID);
                    isValid = true;
                    data = "Type deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                isValid = false;
                CommonDatabaseOperationHelper.Log("DeleteType =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new
            {
                IsValid = isValid,
                data = data
            });
        }
        public string UpdateTypes(string typesArr)
        {
            bool isValid = false;
            string message = "Issue occured when try to update types.";
            try
            {
                if (!string.IsNullOrEmpty(typesArr))
                {
                    var result = _adminRepository.UpdateTypes(typesArr);
                    isValid = true;
                    message = "Types updated successfully.";
                }
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("UpdateTypes =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = isValid, Message = message });
        }
        #endregion
    }
}