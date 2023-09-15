using BoltAFE.Domain.AFE;
using BoltAFE.Domain.UserMaster;
using BoltAFE.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;

namespace BoltAFE.Controllers
{
    public class AFEController : Controller
    {
        private readonly IAFERepository _aFERepository;
        private readonly IUserMasterRepository _userMasterRepository;

        public AFEController(IAFERepository aFERepository, IUserMasterRepository userMasterRepository)
        {
            this._aFERepository = aFERepository;
            this._userMasterRepository = userMasterRepository;
        }

        #region Views 
        public ActionResult CreateAFE()
        {
            return View();
        }

        public ActionResult ApproveEditAFE()
        {
            return View();
        }

        #endregion

        #region Get Types and Categories
        public string GetAFETypesAndCategories()
        {
            string AFETypes = string.Empty;
            string AFECategories = string.Empty;
            try
            {
                AFETypes = _aFERepository.GetTypes();
                AFECategories = _aFERepository.GetCategories();
                return JsonConvert.SerializeObject(new
                {
                    IsValid = true,
                    AFETypes = AFETypes,
                    AFECategories = AFECategories
                });
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetAFETypesAndCategories =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new
            {
                IsValid = false,
                AFETypes = AFETypes,
                AFECategories = AFECategories
            });
        }

        #endregion

        #region Save Comment
        [HttpPost]
        public string SaveComment(int afeHDRID, string message)
        {

            try
            {
                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);

                if (userID > 0)
                {
                    var isFileNameUpdated = _aFERepository.InsertComment(afeHDRID, message, userID);
                    return JsonConvert.SerializeObject(new { IsValid = true, data = "Comment saved successfully." });
                }
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("SaveComment =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = false, data = "Issue occured when try to save comment." });
        }
        #endregion

        #region Get Users

        public string GetUsers()
        {
            string users = string.Empty;
            try
            {
                users = _userMasterRepository.GetAllUsers();
                return JsonConvert.SerializeObject(new
                {
                    IsValid = true,
                    users = users
                });
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetUsers =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new
            {
                IsValid = false,
                users = users
            });
        }
        #endregion

        #region Upload File

        [HttpPost]
        public string UploadFile(HttpPostedFileBase file, int afeHDRID,  string docDiscription)
        {
            string path = string.Empty;
            string folderPath = string.Empty;
            int createdFileID = 0;
            try
            {
                if (file == null)
                {
                    return JsonConvert.SerializeObject(new { IsValid = false, data = "Please select file.", folderPath = folderPath });
                }
                Random r = new Random();
                int rInt = r.Next(0, 1000);
                int fileID = afeHDRID > 0 ? afeHDRID : rInt;
                path = AppDomain.CurrentDomain.BaseDirectory.ToString();
                folderPath = @"Files\" + fileID + "_" + file.FileName;
                path += folderPath;
                file.SaveAs(path);
                createdFileID = _aFERepository.CreateFile(afeHDRID, docDiscription, "./" + folderPath.Replace(@"\", "/"));
                return JsonConvert.SerializeObject(new { IsValid = true, data = "File imported successfully.", folderPath = folderPath, createdFileID = createdFileID });
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("UploadFile =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = false, data = "Issue occured when file is imported.", folderPath = folderPath , createdFileID  = createdFileID });
        }
        #endregion

        #region Get Doc
        public string GetDocuments(int afeHDRID)
        {
            string docs = string.Empty;
            try
            {
                docs = _aFERepository.GetDocs(afeHDRID);
                return JsonConvert.SerializeObject(new { IsValid = true, docs = docs });
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetDocuments =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = false, docs = docs });
        }

        #endregion

        #region AFE List 
        public string GetAllAFE()
        {
            string AFEHdr = string.Empty;
            try
            {
                AFEHdr = _aFERepository.GetAllAFE();
                return JsonConvert.SerializeObject(new
                {
                    IsValid = true,
                    AFEHdr = AFEHdr
                });
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetAllAFE =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new
            {
                IsValid = false,
                AFEHdr = AFEHdr
            });
        }
        #endregion
    }
}