namespace Match.Entities.Models.ReportDbModel
{
       //FP-02	
    public class FinancialStatement_02
    {
        public string DisplayId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public double PreviousWave { get; set; }
        public double CurrentWave { get; set; }
    }
}