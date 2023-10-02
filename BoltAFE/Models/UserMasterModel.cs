namespace BoltAFE.Models
{
    public class UserMasterModel
    {
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public int RoleID { get; set; }
        public decimal Approver { get; set; }
    }
}