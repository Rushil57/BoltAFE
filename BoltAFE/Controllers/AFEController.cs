using BoltAFE.Domain.AFE;
using BoltAFE.Domain.UserMaster;
using BoltAFE.Helpers;
using Newtonsoft.Json;
using System;
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
        public ActionResult CreateAFE(int afeHDR = 0, int afeDTLID = 0)
        {
            ViewBag.AfeHDRID = afeHDR;
            ViewBag.AfeDTLID = afeDTLID;
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
            string AFETypesRecordDTL = string.Empty;
            string AFECategories = string.Empty;
            try
            {
                AFETypes = _aFERepository.GetTypes();
                AFECategories = _aFERepository.GetCategories();
                return JsonConvert.SerializeObject(new
                {
                    IsValid = true,
                    AFETypes = AFETypes,
                    AFECategories = AFECategories,
                    AFETypesRecordDTL = AFETypesRecordDTL
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
                AFECategories = AFECategories,
                AFETypesRecordDTL = AFETypesRecordDTL
            });
        }

        public string GetTypesRecordDetails(int month, int year)
        {
            string AFETypesRecordDTL = string.Empty;
            try
            {
                AFETypesRecordDTL = _aFERepository.GetTypesRecordDetails(month,year);
                return JsonConvert.SerializeObject(new
                {
                    IsValid = true,
                    AFETypesRecordDTL = AFETypesRecordDTL
                });
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetTypesRecordDetails =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new
            {
                IsValid = false,
                AFETypesRecordDTL = AFETypesRecordDTL
            });
        }

        #endregion

        #region  Comments

        public string GetComments(int afeHDRID = 0)
        {
            bool isValid = false;
            string comments = string.Empty;
            try
            {
                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                if (userID > 0)
                {
                    comments = _aFERepository.GetComments(afeHDRID, userID);
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetComments =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = isValid, comments = comments });
        }

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
        public string UploadFile(HttpPostedFileBase file, int afeHDRID, string docDiscription)
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
            return JsonConvert.SerializeObject(new { IsValid = false, data = "Issue occured when file is imported.", folderPath = folderPath, createdFileID = createdFileID });
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

        public string DeleteDocument(int afeHDRID, int docID)
        {
            bool isValid = false;
            string data = string.Empty;
            try
            {
                var isDeleted = _aFERepository.DeleteDoc(afeHDRID, docID);
                data = "Document deleted successfully.";
                isValid = true;
            }
            catch (Exception ex)
            {
                data = "Issue occured when try to delete this doc.";
                CommonDatabaseOperationHelper.Log("DeleteDocument =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = isValid, docs = data });
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
        public string GetAFE(int afeHDRID)
        {
            string AFEHdr = string.Empty;
            string AFEHdrAprvlHistory = string.Empty;
            try
            {
                AFEHdr = _aFERepository.GetAFE(afeHDRID);
                AFEHdrAprvlHistory = _aFERepository.GetAFEHdrAprvlHistory(afeHDRID);
                return JsonConvert.SerializeObject(new
                {
                    IsValid = true,
                    AFEHdr = AFEHdr,
                    AFEHdrAprvlHistory = AFEHdrAprvlHistory
                });
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("GetAFE =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new
            {
                IsValid = false,
                AFEHdr = AFEHdr,
                AFEHdrAprvlHistory = AFEHdrAprvlHistory
            });
        }
        [HttpPost]
        public string DeleteAFEHDR(int afeHDRID)
        {
            bool isValid = false;
            string data = string.Empty;
            try
            {
                var isDeleted = _aFERepository.DeleteAFEHDR(afeHDRID);
                data = "AFE deleted successfully.";
                isValid = true;
            }
            catch (Exception ex)
            {
                data = "Issue occured when try to delete this header.";
                CommonDatabaseOperationHelper.Log("DeleteAFEHDR =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = isValid, data = data });
        }
        #endregion

        #region Save AFE and DTL

        public string SaveHDRAndDTL(string afeHDR, string afeHDRDTL, bool isApproveAFE)
        {
            bool isValid = false;
            bool isDuplicateAFENum = false;
            string data = "Issue occured when try to save AFE Header and Details.";
            try
            {
                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserId"]);
                if (userID > 0)
                {
                    isDuplicateAFENum = isApproveAFE ? isDuplicateAFENum : _aFERepository.CheckIfDuplicateAFENum(afeHDR);
                    var isFileNameUpdated = _aFERepository.SaveHDRAndDTL(afeHDR, afeHDRDTL, isApproveAFE, isDuplicateAFENum);
                    isValid = true;
                    data = "AFE Header and Details saved successfully.";

                }
            }
            catch (Exception ex)
            {
                CommonDatabaseOperationHelper.Log("SaveHDRAndDTL =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = isValid, data = data });
        }
        #endregion

        #region Approve AFE

        public string ApproveAFE(int afeHDRID)
        {
            bool isValid = false;
            string data = string.Empty;
            try
            {
                var isApproved = _aFERepository.ApproveAFE(afeHDRID);
                data = "AFE approved successfully.";
                isValid = isApproved;
            }
            catch (Exception ex)
            {
                data = "Issue occured when try to approve AFE.";
                CommonDatabaseOperationHelper.Log("ApproveAFE =>", ex.Message + "==>" + ex.StackTrace, true);
            }
            return JsonConvert.SerializeObject(new { IsValid = isValid, data = data });
        }

        #endregion

    }
}