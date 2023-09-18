namespace BoltAFE.Models
{
    public class AfeEconDTLModel
    {
        public int Afe_econ_dtl_id { get; set; }
        public int Afe_hdr_id { get; set; }
        public string Description { get; set; }
        public decimal Gross_afe { get; set; } = 0.0M;
        public decimal Wi { get; set; } = 0.0M;
        public decimal Nri { get; set; }= 0.0M;
        public decimal Roy { get; set; }= 0.0M;
        public decimal Net_afe { get; set; }= 0.0M;
        public decimal Oil { get; set; }= 0.0M;
        public decimal Gas { get; set; }= 0.0M;
        public decimal Ngl { get; set; }= 0.0M;
        public decimal Boe { get; set; }= 0.0M;
        public decimal Und_po { get; set; }= 0.0M;
        public decimal Pv10 { get; set; }= 0.0M;
        public decimal F_and_d { get; set; }= 0.0M;
        public decimal Ror { get; set; }= 0.0M;
        public decimal Mroi { get; set; }= 0.0M;
        public int Changed_by_user_id { get; set; }
        public string Changed_date { get; set; }
    }
}