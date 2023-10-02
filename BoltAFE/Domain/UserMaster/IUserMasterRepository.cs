using BoltAFE.Models;

namespace BoltAFE.Domain.UserMaster
{
    public interface IUserMasterRepository
    {
        CommonResponseModel<bool> AddNewUser(string userName, string cardMemberName,decimal approverVal , int roleID);
        string GetAllUsers();
        string GetAllRoles();
        bool DeleteUser(int id);
        ResponseModel ChangePassword(string newPassword, string oldPassword);
        ResponseModel ResetPassword(string UserEmail);
        CommonResponseModel<bool> CheckifUserExist(string userEmail);
        CommonResponseModel<string> CreateNewUser(string userEmail);

        bool UpdateUserNames(string userNameArr);
    }
}