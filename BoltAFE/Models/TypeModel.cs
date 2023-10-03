namespace BoltAFE.Models
{
    public class TypeModel
    {
        public int Afe_type_id { get; set; }
        public string Type { get; set; }
        public int Include_gross_afe { get; set; }
        public int Include_wi { get; set; }
        public int Include_nri { get; set; }
        public int Include_roy { get; set; }
        public int Include_net_afe { get; set; }
        public int Include_oil { get; set; }
        public int Include_gas { get; set; }
        public int Include_ngl { get; set; }
        public int Include_boe { get; set; }
        public int Include_po { get; set; }
        public int Include_pv10 { get; set; } = 0;
        public int Include_f_and_d { get; set; } = 0;
        public int Include_ror { get; set; }
        public int Include_mroi { get; set; }
        public string Afe_num_code { get; set; }
        public int CategoryID { get; set; }
    }
}