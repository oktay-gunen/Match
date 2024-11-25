namespace Match.Web.Models;

public class FinancialStatementsModel
{
    public int ReportTypeId { get; set; } = 1;//default olarak yıllık rapor gelsin
    public string CariDonemDate { get; set; }
    public string OncekiDonemDate { get; set; }
    public List<FinanceTableOneData> Aktifler { get; set; }
    public List<FinanceTableOneData> Pasifler { get; set; }
      public List<FinanceTableOneData> Ozetler { get; set; }
}
public class FinanceTableOneData
{
    public string Title { get; set; }
    public DonemData Data { get; set; }

}
public class DonemData
{
    public string OncekiDonem { get; set; }
    public string OncekiDonemNS { get; set; }
    public string CariDonem { get; set; }
    public string CariDonemNS { get; set; }
    public string Degisim { get; set; }

}