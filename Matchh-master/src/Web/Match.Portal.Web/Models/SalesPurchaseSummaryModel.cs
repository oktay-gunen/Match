namespace Match.Web.Models
{
    public class SalesPurchaseSummaryModel
    {
        public int ReportTypeId { get; set; } = 1;//default olarak yıllık rapor gelsin
        public string CariDonemDate { get; set; }
        public string TotalSalesPrice { get; set; }
        public double TotalSalesPercentage { get; set; }

        public string SalesTaxPrice { get; set; }
        public double SalesTaxPercentage { get; set; }

        public string CustomerCount { get; set; }
        public double CustomerCountPercentage { get; set; }

        public string AverageMaturity { get; set; }
        public List<ChartDataNameDataArray> AlisTutariTedarikciChartData { get; set; }
        public List<ChartDataNameDataArray> SatisTutariTedarikciChartData { get; set; }

        public DoubleArray MostCustomerSalesChartData { get; set; }
        public DoubleArray MostSupplierSalesChartData { get; set; }

        public SeriesChartData VadeliPesinSatisChartData { get; set; }
        public SeriesChartData VadeliPesinAlisChartData { get; set; }

        public string TotalBuyPrice { get; set; }
        public double TotalBuyPercentage { get; set; }

        public string BuyTaxPrice { get; set; }
        public double BuyTaxPercentage { get; set; }

        public string SuppliersCount { get; set; }
        public double SuppliersCountPercentage { get; set; }

        public string BuyAverageMaturity { get; set; }

        public List<ChartDataNameDataArray> SellBuyCompare { get; set; }
        public List<ChartDataNameDataArray> SellBuyTaxCompare { get; set; }
    }
    public class ChartDataNameDataArray
    {
        public string name { get; set; }
        public double[] data { get; set; }
    }
}