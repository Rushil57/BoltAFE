namespace BoltAFE.Domain.Admin
{
    public interface IAdminRepository
    {
        string SendEmail(string bodyString, string userMail, string subject);
    }
}