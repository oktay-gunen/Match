namespace Match.Web.Models
{
    public class GeneralSumModel
    {
        //S01_E5_01_001
        public string Assets { get; set; }
        //S01-E5-01-002
        public string Stocks { get; set; }
        //S01_E5_01_003
        public string WillReceive { get; set; }
        //S01_E5_01_004
        public string Debt { get; set; }
        public List<SeriesData> KasaBankaCekSenetPos { get; set; }
        public MaturityDebt VadesiGecenGecmeyenBorcAlacak { get; set; }
        public SatislarData Satislar { get; set; }
        public SatislarData SatislarinMaliyeti { get; set; }
        public SatislarData SatislarBrutKar { get; set; }
        //"S01-E5-02-006
        public DoubleArray StokAnaGruplari { get; set; }
        //S01-E5-03-011-01
        public double[] BrutKarYuzde { get; set; }
        public string[] BrutKarYuzdeTitle { get; set; }
        //S01-E5-03-012-01
        public string Gelirler { get; set; }
        //S01-E5-03-012-01 bir önceki aya göre değişim yüzde
        public double GelirlerYuzde { get; set; }
        //S01-E5-03-013-01
        public string Giderler { get; set; }
        //giderler yüzde
        public double GiderlerYuzde { get; set; }
        //S01-E5-03-014-04
        public string NetKarZararAylik { get; set; }
        //NetKarZararAylik bir önceki aya göre değişim yüzde
        public double NetKarZararAylikYuzde { get; set; }
        //S01-E5-03-014-05
        public string NetKarZarar { get; set; }
        //NetKarZarar bir önceki aya göre değişim yüzde
        public double NetKarZararYuzde { get; set; }
        public GelirGiderKarZararData GelirGiderKarZarar { get; set; }

    }

    public class SeriesData
    {
        public string name { get; set; }
        public double[] data { get; set; }
    }

    public class MaturityDebt
    {
        public List<SeriesData> Data { get; set; }
        public int[] Years { get; set; }


    }
    //Aylık Satışlar Grafiği S01-E5-03-008-02
    public class SatislarData
    {
        public List<SeriesData> ChartData { get; set; }
        public string Satislar { get; set; }
        public string Degisim { get; set; }
    }

    public class DoubleArray
    {
        public string[] Name { get; set; }
        public double[] Data { get; set; }
    }
    public class GelirGiderKarZararData
    {
        public double[] Gelirler { get; set; }
        public double[] Giderler { get; set; }
        public double[] NetKarZarar { get; set; }


    }

}
