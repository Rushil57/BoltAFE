using BoltAFE.Models;

namespace BoltAFE.Domain.UserMaster
{
    public interface IUserMasterRepository
    {
        CommonResponseModel<bool> AddNewUser(string userName);
        string GetAllUsers();
        bool DeleteUser(int id);
        ResponseModel ChangePassword(string newPassword, string oldPassword);
        ResponseModel ResetPassword(string UserEmail);
        CommonResponseModel<bool> CheckifUserExist(string userEmail);
        CommonResponseModel<string> CreateNewUser(string userEmail);
    }
}