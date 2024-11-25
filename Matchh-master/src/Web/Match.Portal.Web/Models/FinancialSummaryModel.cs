namespace Match.Web.Models;

public class FinancialSummaryModel
{
      public int ReportTypeId { get; set; } = 1;//default olarak yıllık rapor gelsin
      public string CariDonemDate { get; set; }
      public List<FinancialSummaryTableData> SummaryAssetData { get; set; }
      public List<SummarySeriesData> SummaryAssetChartData { get; set; }
      public List<FinancialSummaryTableData> ReceivablesDebitsData { get; set; }
      public double ReceivablesDebitsChartData { get; set; }
      public List<FinancialSummaryTableData> ReceivablesAgingData { get; set; }
      public SummaryArrayData ReceivablesAgingChartData { get; set; }
      public List<FinancialSummaryTableData> DebtsData { get; set; }
      public SummaryArrayData DebtsChartData { get; set; }
      public List<FinancialSummaryTableData> DebtsAgingData { get; set; }
      public SummaryArrayData DebtsAgingChartData { get; set; }
      public FinancialSummaryChart1 DebtsReceivablesChartData { get; set; }





}

public class FinancialSummaryTableData
{
      public string Title { get; set; }
      public SummaryData Data { get; set; }


}
public class FinancialSummaryChart1
{
      public string[] Title { get; set; }
      public List<SummarySeriesData> Data { get; set; }


}
public class SummaryData
{
      public string BalanceTL { get; set; }
      public string BalanceUSD { get; set; }
      public string BalanceEUR { get; set; }
      public string BalanceDoviz { get; set; }
      public string Toplam { get; set; }

}
public class SummarySeriesData
{
      public string name { get; set; }
      public double[] data { get; set; }
}
public class SummaryArrayData
{
      public string[] name { get; set; }
      public double[] data { get; set; }
}