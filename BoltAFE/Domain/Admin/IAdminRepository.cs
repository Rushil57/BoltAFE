namespace BoltAFE.Domain.Admin
{
    public interface IAdminRepository
    {
        string SendEmail(string bodyString, string userMail, string subject);

        #region Category
        bool SaveCategory(string categoryName);
        bool DeleteCategory(int categoryID);

        bool UpdateCategory(string categoryArr);
        #endregion

        #region Types
        bool SaveType(string typeStr);
        bool DeleteType(int typeID);

        bool UpdateTypes(string typesArr);
        #endregion
    }
}