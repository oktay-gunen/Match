namespace Match.Web.Models
{
    public class StockIncomeExpenseSummaryModel
    {
        public int ReportTypeId { get; set; } = 1;//default olarak yıllık rapor gelsin
        public string CariDonemDate { get; set; }
        public string Sales { get; set; }
        public string SalesQuantity { get; set; }
        public string Cost { get; set; }
        public string Profit { get; set; }
        public List<StockGroup> StockGroup { get; set; }
        public List<GroupDataChartData> StockGroupChartData { get; set; }
        public SeriesChartData ProductSalesChartData { get; set; }
        public SeriesChartData ProductCostChartData { get; set; }
        public List<WarehouseBranchSalesChartData> WarehouseBranchSalesChartData { get; set; }
        public string ServiceSales { get; set; }//Hizmet Satışları
        public string OtherProfits { get; set; }//Diğer Gelirler
        public string FinancialExpenses { get; set; }//Faaliyet Giderleri
        public string NetProfitPrice { get; set; }
        public double NetProfitPercent { get; set; }
        public List<GroupDataChartData> ServiceSalesMonthChartData { get; set; }
        public SeriesChartData GelirGiderKarChartData { get; set; }
         public SeriesChartData ToplamSatislarDagilimiChartData { get; set; }
        
        







    }
    public class StockGroup
    {
        public string GroupName { get; set; }
        public string Sales { get; set; }
        public string SalesQuantity { get; set; }
        public string Cost { get; set; }
        public string Profit { get; set; }
        public string RemainingAmount { get; set; }
    }
    public class GroupDataChartData
    {
        public string name { get; set; }
        public double[] data { get; set; }
    }
    public class SeriesChartData
    {
        public string[] Name { get; set; }
        public double[] Data { get; set; }
    }
    public class WarehouseBranchSalesChartData
    {
        public string Name { get; set; }
        public double Sales { get; set; }
        public double Percentage { get; set; }
    }

}