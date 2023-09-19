namespace BoltAFE.Models
{
    public class AfeApprovalHistoryModel
    {
        public int Aprvl_hist_dtl_id { get; set; }
        public int Afe_hdr_id { get; set; }
        public int Approver_user_id { get; set; }
        public string Approved_date { get; set; }
        public int User_ID { get; set; }
        public string User_email { get; set; }
    }
}