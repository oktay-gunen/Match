using System.Net.Http.Json;
using Match.Business.Constants;
using Match.Business.Constants.Enums;
using Match.Business.Services;
using Match.Core.Utilities.Results;
using Match.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Match.Business.Helper;


namespace Match.Web.Controllers
{
    public class GeneralSummaryController : BaseController
    {
        private readonly IReportService _repoReportService;
        private readonly ILogger<HomeController> _logger;

        public GeneralSummaryController(IReportService repoReportService, ILogger<HomeController> logger)
        {
            _repoReportService = repoReportService;
            _logger = logger;
        }



        //Genel Durum FP-01-01			
        public async Task<IActionResult> GeneralSum()
        {
            var result = await _repoReportService.GetReportFP_01_01Async();
            var reportInfoList = new GeneralSumModel();
            if (result != null && result.Any())
            {
                reportInfoList.Assets = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-01-001")?.Price ?? 0).ToString("n2");
                reportInfoList.Stocks = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-01-002")?.Price ?? 0).ToString("n2");
                reportInfoList.WillReceive = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-01-003")?.Price ?? 0).ToString("n2");
                reportInfoList.Debt = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-01-004")?.Price ?? 0).ToString("n2");
                #region ### S01-E5-02-005 Kasa-Banka-Çek-Senet-Pos ###
                var kasaBankaCekSenetPosData = new List<SeriesData>();
                var walletData = result.Where(a => a.DisplayId == "S01-E5-02-005").ToList();
                var displayNames = walletData.Select(a => a.DisplayTitle).Distinct().ToList();

                foreach (var displayName in displayNames)
                {
                    var chartData = new SeriesData();
                    chartData.name = displayName;
                    var itemData = new double[12];

                    var items = walletData.Where(a => a.DisplayTitle == displayName).ToList();
                    for (int i = 1; i < 13; i++)
                    {
                        var item = items.FirstOrDefault(a => a.ReportMonth == i);
                        if (item != null)
                        {
                            itemData[i - 1] = item.Price;
                        }
                        else
                        {
                            itemData[i - 1] = 0;
                        }

                    }
                    chartData.data = itemData;
                    kasaBankaCekSenetPosData.Add(chartData);
                }
                reportInfoList.KasaBankaCekSenetPos = kasaBankaCekSenetPosData;
                #endregion

                #region ### Vadesi Geçen - Geçmeyen Borç Alacaklar ###

                var vadesiGGBorcAlacakData = new List<SeriesData>();
                var yearList = new List<int>();
                var vadesiGGBorcAlacakDataIds = new List<string>() { "S01-E5-02-007-01", "S01-E5-02-007-02", "S01-E5-02-007-03", "S01-E5-02-007-04" };

                var debtData = result.Where(a => vadesiGGBorcAlacakDataIds.Contains(a.DisplayId)).ToList();

                foreach (var item in debtData)
                {
                    var chartData = new SeriesData();
                    chartData.name = item.DisplayTitle;
                    chartData.data = new double[1] { item.Price };

                    vadesiGGBorcAlacakData.Add(chartData);
                    yearList.Add(Convert.ToInt32(item.ReportYear));

                }

                var vadesiGGBorcAlacak = new MaturityDebt();
                vadesiGGBorcAlacak.Data = vadesiGGBorcAlacakData;
                vadesiGGBorcAlacak.Years = yearList.Distinct().ToArray();

                reportInfoList.VadesiGecenGecmeyenBorcAlacak = vadesiGGBorcAlacak;

                #endregion

                #region ### Satışlar ###
                //Satışlar                      S01-E5-03-008-01
                //Aylık Satışlar Grafiği        S01-E5-03-008-02
                //Değişim % 'si					S01-E5-03-008-04
                var satislar = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-008-01")?.Price ?? 0).ToString("n2");
                var degisim = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-008-04")?.Price ?? 0).ToString();

                var satislarData = new List<SeriesData>();
                var satislarResult = result.Where(a => a.DisplayId == "S01-E5-03-008-02").ToList();
                var chartDataSatislar = new SeriesData();
                var itemDataSatislar = new double[12];
                var satislarModel = new SatislarData();

                for (int i = 1; i < 13; i++)
                {
                    var itemSatislar = satislarResult.FirstOrDefault(a => a.ReportMonth == i);
                    if (itemSatislar != null)
                    {
                        itemDataSatislar[i - 1] = itemSatislar.Price;
                    }
                    else
                    {
                        itemDataSatislar[i - 1] = 0;
                    }

                }
                chartDataSatislar.data = itemDataSatislar;
                chartDataSatislar.name = (satislarResult.FirstOrDefault()?.DisplayTitle ?? "");
                satislarData.Add(chartDataSatislar);

                satislarModel.ChartData = satislarData;
                satislarModel.Satislar = satislar;
                satislarModel.Degisim = degisim;

                reportInfoList.Satislar = satislarModel;

                #endregion
                #region ### Satışların Maliyeti ###
                //Satışların Maliyeti  S01-E5-03-009-01
                //Aylık Grafik         S01-E5-03-009-02
                //Değişim % 'si        S01-E5-03-009-04
                var satislarMaliyet = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-009-01")?.Price ?? 0).ToString("n2");
                var degisimrMaliyet = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-009-04")?.Price ?? 0).ToString();

                var satislarrMaliyetData = new List<SeriesData>();
                var satislarMaliyetResult = result.Where(a => a.DisplayId == "S01-E5-03-009-02").ToList();
                var chartDataSatislarMaliyet = new SeriesData();
                var itemDataSatislarMaliyet = new double[12];
                var satislarMaliyetModel = new SatislarData();

                for (int i = 1; i < 13; i++)
                {
                    var itemSatislar = satislarResult.FirstOrDefault(a => a.ReportMonth == i);
                    if (itemSatislar != null)
                    {
                        itemDataSatislarMaliyet[i - 1] = itemSatislar.Price;
                    }
                    else
                    {
                        itemDataSatislarMaliyet[i - 1] = 0;
                    }

                }
                chartDataSatislarMaliyet.data = itemDataSatislar;
                chartDataSatislarMaliyet.name = (satislarResult.FirstOrDefault()?.DisplayTitle ?? "");
                satislarrMaliyetData.Add(chartDataSatislar);

                satislarMaliyetModel.ChartData = satislarrMaliyetData;
                satislarMaliyetModel.Satislar = satislarMaliyet;
                satislarMaliyetModel.Degisim = degisimrMaliyet;

                reportInfoList.SatislarinMaliyeti = satislarMaliyetModel;

                #endregion
                #region ### Satışlar Brüt Kar ###
                //Brüt Kar      S01-E5-03-010-01
                //Aylık Grafik  S01-E5-03-010-02
                //Değişim % 'si S01-E5-03-010-04
                var satislarBrutKar = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-010-01")?.Price ?? 0).ToString("n2");
                var degisimBrutKar = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-010-04")?.Price ?? 0).ToString();

                var satislarBrutKarData = new List<SeriesData>();
                var satislarBrutKarResult = result.Where(a => a.DisplayId == "S01-E5-03-010-02").ToList();
                var chartDataSatislarBrutKar = new SeriesData();
                var itemDataSatislarBrutKar = new double[12];
                var satislarBrutKarModel = new SatislarData();

                for (int i = 1; i < 13; i++)
                {
                    var itemSatislar = satislarResult.FirstOrDefault(a => a.ReportMonth == i);
                    if (itemSatislar != null)
                    {
                        itemDataSatislarBrutKar[i - 1] = itemSatislar.Price;
                    }
                    else
                    {
                        itemDataSatislarBrutKar[i - 1] = 0;
                    }

                }
                chartDataSatislarBrutKar.data = itemDataSatislar;
                chartDataSatislarBrutKar.name = (satislarResult.FirstOrDefault()?.DisplayTitle ?? "");
                satislarBrutKarData.Add(chartDataSatislar);

                satislarBrutKarModel.ChartData = satislarBrutKarData;
                satislarBrutKarModel.Satislar = satislarBrutKar;
                satislarBrutKarModel.Degisim = degisimBrutKar;

                reportInfoList.SatislarBrutKar = satislarBrutKarModel;

                #endregion
                #region ### Stok Ana grupları toplam tutar ###
                var stoklarAnaGrupData = new List<double>();
                var stoklarAnaGrupName = new List<string>();
                var stokAnaGruplariToplamData = result.Where(a => a.DisplayId == "S01-E5-02-006").ToList();
                foreach (var item in stokAnaGruplariToplamData)
                {
                    stoklarAnaGrupData.Add(Math.Round(item.Price, 2));
                    stoklarAnaGrupName.Add(item.DisplayTitle);
                }
                var stockData = new DoubleArray();
                stockData.Data = stoklarAnaGrupData.ToArray();
                stockData.Name = stoklarAnaGrupName.ToArray();

                reportInfoList.StokAnaGruplari = stockData;

                #endregion

                #region ### Brüt Kar ###
                var brutKarYuzdeList = new List<double>();
                var brutKarYuzde = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-011-01")?.Price ?? 0);
                brutKarYuzdeList.Add(brutKarYuzde < 0 ? brutKarYuzde * -1 : brutKarYuzde);

                reportInfoList.BrutKarYuzdeTitle = new string[] { (brutKarYuzde < 0 ? "Brüt Zarar" : "Brüt Kar") };
                reportInfoList.BrutKarYuzde = brutKarYuzdeList.ToArray();

                #endregion
                #region ### Firma Gelir Gider Kar Zararı ###

                var gelirGiderKarZarar = new GelirGiderKarZararData();
                var gelirGiderKarZararData = result.Where(a => a.DisplayId == "S01-E5-03-015").ToList();

                gelirGiderKarZarar.Giderler = gelirGiderKarZararData.Where(a => a.DisplayTitle == "Giderler").ToList().GetDataByMonthArray();
                gelirGiderKarZarar.Gelirler = gelirGiderKarZararData.Where(a => a.DisplayTitle == "Gelirler").ToList().GetDataByMonthArray();
                gelirGiderKarZarar.NetKarZarar = gelirGiderKarZararData.Where(a => a.DisplayTitle == "Net Kar Zarar").ToList().GetDataByMonthArray();
                reportInfoList.GelirGiderKarZarar = gelirGiderKarZarar;

                #endregion

                #region ### Gelirler ###

                reportInfoList.Gelirler = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-012-01")?.Price ?? 0).ToString("n2");
                reportInfoList.GelirlerYuzde = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-012-04")?.Price ?? 0);
                reportInfoList.Giderler = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-013-01")?.Price ?? 0).ToString("n2");
                reportInfoList.GiderlerYuzde = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-013-04")?.Price ?? 0);
                reportInfoList.NetKarZararAylik = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-014-01")?.Price ?? 0).ToString("n2");
                reportInfoList.NetKarZararAylikYuzde = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-014-04")?.Price ?? 0);
                reportInfoList.NetKarZarar = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-014-05")?.Price ?? 0).ToString("n2");
                reportInfoList.NetKarZararYuzde = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-03-014-05")?.Price ?? 0);


                #endregion
            }
            else
            {
                ModelState.AddModelError(string.Empty, Messages.DontFoundData);
            }


            return View(reportInfoList);
        }

        public async Task<IActionResult> FinancialStatements()
        {
            var model = new FinancialStatementsModel();
            var cariDate = DateTime.Now;
            var oncekiDate = DateTime.Now.AddMonths(-1);


            model.CariDonemDate = $"{cariDate.Month}/{cariDate.Year}";
            model.OncekiDonemDate = $"{oncekiDate.Month}/{oncekiDate.Year}";

            var result = await GetFinancialStatementsData(model);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, Messages.DontFoundData);
                return View(model);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> FinancialStatements(FinancialStatementsModel model)
        {

            return View(model);

        }

        public async Task<FinancialStatementsModel?> GetFinancialStatementsData(FinancialStatementsModel model)
        {
            var cariDate = GetDateFromText("/", model.CariDonemDate);
            var oncekiDate = GetDateFromText("/", model.OncekiDonemDate);

            var monthCariParam = model.ReportTypeId == 1 ? 0 : cariDate.Month;
            var monthOncekiParam = model.ReportTypeId == 1 ? 0 : oncekiDate.Month;

            var resultCari = await _repoReportService.GetReportDisplayTwoAsync(cariDate.Year, monthCariParam);
            var resultOnceki = await _repoReportService.GetReportDisplayTwoAsync(oncekiDate.Year, monthOncekiParam);

            if (resultCari == null || resultOnceki == null)
            {
                return null;
            }

            #region ###   Aktifler	###
            var aktiflerDataList = new List<FinanceTableOneData>();
            var aktifitemDic = new Dictionary<string, string>();

            aktifitemDic.Add("1.A", "Dönen Varlıklar");
            aktifitemDic.Add("0", "Hazır Değerler (Nakit ve Benzerleri)");
            aktifitemDic.Add("4", "Ticari Alacaklar");
            aktifitemDic.Add("1", "Diğer Alacaklar");
            aktifitemDic.Add("2", "Stoklar");
            aktifitemDic.Add("3", "Diğer Dönen Varlıklar");

            foreach (var item in aktifitemDic)
            {
                var data = new FinanceTableOneData();
                var financeData = new DonemData();
                data.Title = item.Value;
                financeData.CariDonem = GenerateRandomNumber(120000, 2400000).ToString("n2");
                financeData.OncekiDonem = GenerateRandomNumber(120000, 2400000).ToString("n2");
                financeData.Degisim = GenerateRandomNumber(10, 60).ToString("n2");
                data.Data = financeData;
                aktiflerDataList.Add(data);

            }
            model.Aktifler = aktiflerDataList;
            #endregion

            #region ### Pasifler	###			
            var pasiflerDataList = new List<FinanceTableOneData>();
            var pasifItemDic = new Dictionary<string, string>();

            pasifItemDic.Add("0", "Borçlar");
            pasifItemDic.Add("1.A", "Mali Borçlar");
            pasifItemDic.Add("1", "Ticari Borçlar");
            pasifItemDic.Add("2", "Diğer Borçlar");
            pasifItemDic.Add("3", "Vergi Borçları");
            pasifItemDic.Add("4", "Diğer Yükümlülükler");

            foreach (var item in pasifItemDic)
            {
                var data = new FinanceTableOneData();
                var financeData = new DonemData();
                data.Title = item.Value;
                financeData.CariDonem = GenerateRandomNumber(120000, 2400000).ToString("n2");
                financeData.OncekiDonem = GenerateRandomNumber(120000, 2400000).ToString("n2");
                financeData.Degisim = GenerateRandomNumber(10, 60).ToString("n2");
                data.Data = financeData;
                pasiflerDataList.Add(data);

            }
            model.Pasifler = pasiflerDataList;

            #endregion
            #region ### Özet Gelir Tablosu	###			
            var ozetlerDataList = new List<FinanceTableOneData>();
            var ozetItemDic = new Dictionary<string, string>();

            ozetItemDic.Add("0", "Net Satışlar");
            ozetItemDic.Add("1.A", "Satışların Maliyeti(-)");
            ozetItemDic.Add("1", "Brüt Kar/Zararı");
            ozetItemDic.Add("2", "Faaliyet Giderleri (-)");
            ozetItemDic.Add("3", "Faaliyet Kar/Zararı");
            ozetItemDic.Add("4", "Diğer Faaliyetler (+-)");
            ozetItemDic.Add("5", "Vergi (YYK) (-)");
            ozetItemDic.Add("6", "Dönem Net Kar/Zararı");
            ozetItemDic.Add("7", "Esas Faaliyet K/Z (Ebit)");
            ozetItemDic.Add("8", "Faiz ve Vergi Öncesi K/Z (Ebitda)");

            foreach (var item in ozetItemDic)
            {
                var data = new FinanceTableOneData();
                var financeData = new DonemData();
                data.Title = item.Value;
                financeData.CariDonem = GenerateRandomNumber(120000, 2400000).ToString("n2");
                financeData.CariDonemNS = GenerateRandomNumber(0, 10).ToString("n2");
                financeData.OncekiDonem = GenerateRandomNumber(120000, 2400000).ToString("n2");
                financeData.OncekiDonemNS = GenerateRandomNumber(1, 5).ToString("n2");
                financeData.Degisim = GenerateRandomNumber(10, 60).ToString("n2");
                data.Data = financeData;
                ozetlerDataList.Add(data);

            }
            model.Ozetler = ozetlerDataList;

            #endregion





            return model;

        }

        public async Task<IActionResult> FinancialSummary()
        {
            var model = new FinancialSummaryModel();
            var cariDate = DateTime.Now;
            model.CariDonemDate = $"{cariDate.Month}/{cariDate.Year}";

            var result = await FinancialSummaryData(model);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, Messages.DontFoundData);
                return View(model);
            }

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> FinancialSummary(FinancialSummaryModel model)
        {

            var result = await FinancialSummaryData(model);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, Messages.DontFoundData);
                return View(model);
            }

            return View(result);

        }
        public async Task<FinancialSummaryModel?> FinancialSummaryData(FinancialSummaryModel model)
        {
            var cariDate = GetDateFromText("/", model.CariDonemDate);

            var monthCariParam = model.ReportTypeId == 1 ? 0 : cariDate.Month;
            var result = await _repoReportService.GetReportDisplayThreeAsync(cariDate.Year, monthCariParam);

            if (result == null)
            {
                return null;
            }
            #region  ### ÖZET VARLIK DURUMU ###

            var ozetVarlikDataList = new List<FinancialSummaryTableData>();
            var ozetItemDic = new Dictionary<string, string>();

            ozetItemDic.Add("S03-E6-01-001-01", "Kasa");
            ozetItemDic.Add("S03-E6-01-001-06", "Banka");
            ozetItemDic.Add("S03-E6-01-001-11", "M.Çek");
            ozetItemDic.Add("S03-E6-01-001-16", "M.Senet");
            ozetItemDic.Add("S03-E6-01-001-21", "K.K.Pos");
            ozetItemDic.Add("S03-E6-01-001-26", "Toplam TL");

            foreach (var item in ozetItemDic)
            {
                var data = new FinancialSummaryTableData();
                var summaryData = new SummaryData();
                data.Title = item.Value;
                summaryData.BalanceTL = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_TL ?? 0).ToString("n2");
                summaryData.BalanceUSD = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_Usd ?? 0).ToString("n2");
                summaryData.BalanceEUR = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_Eur ?? 0).ToString("n2");
                summaryData.BalanceDoviz = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_DDvz ?? 0).ToString("n2");
                summaryData.Toplam = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Total_TL ?? 0).ToString("n2");
                data.Data = summaryData;
                ozetVarlikDataList.Add(data);

            }
            model.SummaryAssetData = ozetVarlikDataList;

            #region ### Varlıklar chart ###
            var varliklarChartData = result.Where(a => a.DisplayId == "S03-E6-02-002").ToList();
            var varliklarTitles = varliklarChartData.Select(a => a.Header).Distinct().ToList();
            var varliklarDataList = new List<SummarySeriesData>();
            foreach (var t in varliklarTitles)
            {
                var smryData = new SummarySeriesData();
                smryData.data = varliklarChartData.Where(a => a.Header == t).ToList().GetDataByMonthArray();
                smryData.name = t;
                varliklarDataList.Add(smryData);
            }

            model.SummaryAssetChartData = varliklarDataList;
            #endregion
            #endregion

            #region ### Alacaklar Borçlar ###
            var alacaklarDataList = new List<FinancialSummaryTableData>();
            var alacaklarItemDic = new Dictionary<string, string>();

            alacaklarItemDic.Add("S03-E6-01-003-06", "Vd.Gelmemiş Alacaklar");
            alacaklarItemDic.Add("S03-E6-01-003-11", "Vd. Geçen Alacaklar");
            alacaklarItemDic.Add("S03-E6-01-003-01", "Toplam Alacaklar");

            foreach (var item in alacaklarItemDic)
            {
                var data = new FinancialSummaryTableData();
                var summaryData = new SummaryData();
                data.Title = item.Value;
                summaryData.BalanceTL = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_TL ?? 0).ToString("n2");
                summaryData.BalanceUSD = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_Usd ?? 0).ToString("n2");
                summaryData.BalanceEUR = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_Eur ?? 0).ToString("n2");
                summaryData.BalanceDoviz = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_DDvz ?? 0).ToString("n2");
                summaryData.Toplam = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Total_TL ?? 0).ToString("n2");
                data.Data = summaryData;
                alacaklarDataList.Add(data);

            }
            model.ReceivablesDebitsData = alacaklarDataList;
            #region ### Alacaklar İlerleme Çubuğu chart ###
            var alcaklarBorclarChartData = result.Where(a => a.DisplayId == "S03-E6-02-003-16").FirstOrDefault();
            if (alcaklarBorclarChartData != null)
            {
                model.ReceivablesDebitsChartData = alcaklarBorclarChartData.Total_TL ?? 0;
            }

            #endregion

            #endregion


            #region ### VADESİ GEÇEN ALACAK YAŞLANDIRMA  ###
            var vdGecenAlacaklarDataList = new List<FinancialSummaryTableData>();
            var vdGecenAlacaklarItemDic = new Dictionary<string, string>();

            vdGecenAlacaklarItemDic.Add("S03-E6-03-003-17", "0-30 Gün");
            vdGecenAlacaklarItemDic.Add("S03-E6-03-003-22", "31-60 Gün");
            vdGecenAlacaklarItemDic.Add("S03-E6-03-003-27", "61-90 Gün");
            vdGecenAlacaklarItemDic.Add("S03-E6-03-003-32", "91-120 Gün");
            vdGecenAlacaklarItemDic.Add("S03-E6-03-003-37", "120+ Gün");
            vdGecenAlacaklarItemDic.Add("S03-E6-03-003-01", "Toplam");

            foreach (var item in vdGecenAlacaklarItemDic)
            {
                var data = new FinancialSummaryTableData();
                var summaryData = new SummaryData();
                data.Title = item.Value;
                summaryData.BalanceTL = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_TL ?? 0).ToString("n2");
                summaryData.BalanceUSD = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_Usd ?? 0).ToString("n2");
                summaryData.BalanceEUR = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_Eur ?? 0).ToString("n2");
                summaryData.BalanceDoviz = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_DDvz ?? 0).ToString("n2");
                summaryData.Toplam = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Total_TL ?? 0).ToString("n2");
                data.Data = summaryData;
                vdGecenAlacaklarDataList.Add(data);

            }
            model.ReceivablesAgingData = vdGecenAlacaklarDataList;
            #region ### VADESİ GEÇEN ALACAK YAŞLANDIRMA chart ###
            var vdGecenalcaklarBorclarChartData = result.Where(a => a.DisplayId == "S03-E6-04-003-42").FirstOrDefault();
            if (vdGecenalcaklarBorclarChartData != null)
            {
                var vdGecenAlacaklarChart = new SummaryArrayData();
                vdGecenAlacaklarChart.data = new double[]{
                    ( vdGecenalcaklarBorclarChartData.Balance_TL ?? 0),
                    ( vdGecenalcaklarBorclarChartData.Balance_Usd ?? 0),
                    ( vdGecenalcaklarBorclarChartData.Balance_Eur ?? 0),
                    ( vdGecenalcaklarBorclarChartData.Balance_DDvz ?? 0),
                    ( vdGecenalcaklarBorclarChartData.Total_TL ?? 0)
                    };
                vdGecenAlacaklarChart.name = new string[] { "TL", "USD", "EUR", "Doviz", "Toplam" };



                model.ReceivablesAgingChartData = vdGecenAlacaklarChart;
            }
            else
            {
                model.ReceivablesAgingChartData = new SummaryArrayData();
            }

            #endregion

            #endregion

            #region ### Borçlar  ###
            var borclarDataList = new List<FinancialSummaryTableData>();
            var borclarItemDic = new Dictionary<string, string>();

            borclarItemDic.Add("S03-E6-03-004-48", "Vd.Gelmemiş Borçlar");
            borclarItemDic.Add("S03-E6-03-004-53", "Vd.Geçen Borçlar");
            borclarItemDic.Add("S03-E6-03-004-43", "Toplam Borçlar");

            foreach (var item in borclarItemDic)
            {
                var data = new FinancialSummaryTableData();
                var summaryData = new SummaryData();
                data.Title = item.Value;
                summaryData.BalanceTL = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_TL ?? 0).ToString("n2");
                summaryData.BalanceUSD = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_Usd ?? 0).ToString("n2");
                summaryData.BalanceEUR = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_Eur ?? 0).ToString("n2");
                summaryData.BalanceDoviz = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_DDvz ?? 0).ToString("n2");
                summaryData.Toplam = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Total_TL ?? 0).ToString("n2");
                data.Data = summaryData;
                borclarDataList.Add(data);

            }
            model.DebtsData = borclarDataList;
            #region ### Borçlar chart ###
            var borclarChartData = result.Where(a => a.DisplayId == "S03-E6-04-003-58").FirstOrDefault();
            if (borclarChartData != null)
            {
                var borclarChart = new SummaryArrayData();
                borclarChart.data = new double[]{
                    ( borclarChartData.Balance_TL ?? 0),
                    ( borclarChartData.Balance_Usd ?? 0),
                    ( borclarChartData.Balance_Eur ?? 0),
                    ( borclarChartData.Balance_DDvz ?? 0),
                    ( borclarChartData.Total_TL ?? 0)
                    };
                borclarChart.name = new string[] { "TL", "USD", "EUR", "Doviz", "Toplam" };



                model.DebtsChartData = borclarChart;
            }
            else
            {
                model.DebtsChartData = new SummaryArrayData();
            }

            #endregion

            #endregion

            #region ### VADESİ GEÇEN BORÇ YAŞLANDIRMA  ###
            var vdGecenBorclarDataList = new List<FinancialSummaryTableData>();
            var vdGecenBorclarItemDic = new Dictionary<string, string>();

            vdGecenBorclarItemDic.Add("S03-E6-05-003-59", "0-30 Gün");
            vdGecenBorclarItemDic.Add("S03-E6-05-003-64", "31-60 Gün");
            vdGecenBorclarItemDic.Add("S03-E6-05-003-69", "61-90 Gün");
            vdGecenBorclarItemDic.Add("S03-E6-05-003-74", "91-120 Gün");
            vdGecenBorclarItemDic.Add("S03-E6-05-003-79", "120+ Gün");
            vdGecenBorclarItemDic.Add("S03-E6-05-004-43", "Toplam");

            foreach (var item in vdGecenBorclarItemDic)
            {
                var data = new FinancialSummaryTableData();
                var summaryData = new SummaryData();
                data.Title = item.Value;
                summaryData.BalanceTL = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_TL ?? 0).ToString("n2");
                summaryData.BalanceUSD = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_Usd ?? 0).ToString("n2");
                summaryData.BalanceEUR = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_Eur ?? 0).ToString("n2");
                summaryData.BalanceDoviz = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Balance_DDvz ?? 0).ToString("n2");
                summaryData.Toplam = (result.FirstOrDefault(a => a.DisplayId == item.Key)?.Total_TL ?? 0).ToString("n2");
                data.Data = summaryData;
                vdGecenBorclarDataList.Add(data);

            }
            model.DebtsAgingData = vdGecenBorclarDataList;
            #region ### VADESİ GEÇEN BORÇ YAŞLANDIRMA chart ###
            var vdGecenBorclarChartData = result.Where(a => a.DisplayId == "S03-E6-06-003-84").FirstOrDefault();
            if (vdGecenBorclarChartData != null)
            {
                var vdGecenBorclarChart = new SummaryArrayData();
                vdGecenBorclarChart.data = new double[]{
                     Math.Round(( vdGecenBorclarChartData.Balance_TL ?? 0),2),
                    Math.Round(( vdGecenBorclarChartData.Balance_Usd ?? 0),2),
                    Math.Round(( vdGecenBorclarChartData.Balance_Eur ?? 0),2),
                    Math.Round(( vdGecenBorclarChartData.Balance_DDvz ?? 0),2),
                    Math.Round(( vdGecenBorclarChartData.Total_TL ?? 0),2)
                    };
                vdGecenBorclarChart.name = new string[] { "TL", "USD", "EUR", "Doviz", "Toplam" };



                model.DebtsAgingChartData = vdGecenBorclarChart;
            }
            else
            {
                model.DebtsAgingChartData = new SummaryArrayData();
            }

            #endregion

            #endregion

            #region ### VADESİ GELMEMİŞ BORÇ-VADESİ GELMEMİŞ ALACAKLARchart ###
            var vdBorcAlacakIds = new List<string>() { "S03-E6-05-004-01", "S03-E6-05-005-01" };

            var vadeBorcAlacakChartData = result.Where(a => vdBorcAlacakIds.Contains(a.DisplayId)).ToList();
            // var vadeAlacakChartData = result.Where(a => a.DisplayId == "S03-E6-05-005-01").FirstOrDefault();
            var vdBorclarChart = new FinancialSummaryChart1();
            var borlarAlacaklarChartData = new List<SummarySeriesData>();
            foreach (var item in vadeBorcAlacakChartData)
            {
                var vdBData = new SummarySeriesData();
                vdBData.name = item.Header;
                vdBData.data = new double[]{
                    ( item.Balance_TL ?? 0),
                    ( item.Balance_Usd ?? 0),
                    ( item.Balance_Eur ?? 0),
                    ( item.Balance_DDvz ?? 0),
                    ( item.Total_TL ?? 0)
                    };
                borlarAlacaklarChartData.Add(vdBData);
            }
            vdBorclarChart.Title = new string[] { "TL", "USD", "EUR", "Doviz", "Toplam" };
            vdBorclarChart.Data = borlarAlacaklarChartData;
            model.DebtsReceivablesChartData = vdBorclarChart;

            #endregion

            return model;

        }

        public async Task<IActionResult> StockIncomeExpenseSummary()
        {
            var model = new StockIncomeExpenseSummaryModel();
            var cariDate = DateTime.Now;
            model.CariDonemDate = $"{cariDate.Month}/{cariDate.Year}";

            var result = await StockIncomeExpenseSummaryData(model);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, Messages.DontFoundData);
                return View(model);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> StockIncomeExpenseSummary(StockIncomeExpenseSummaryModel model)
        {

            var result = await StockIncomeExpenseSummaryData(model);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, Messages.DontFoundData);
                return View(model);
            }

            return View(model);

        }
        public async Task<StockIncomeExpenseSummaryModel?> StockIncomeExpenseSummaryData(StockIncomeExpenseSummaryModel model)
        {
            var cariDate = GetDateFromText("/", model.CariDonemDate);

            var monthCariParam = model.ReportTypeId == 1 ? 0 : cariDate.Month;
            var result = await _repoReportService.GetReportDisplayFourAsync(cariDate.Year, monthCariParam);

            if (result == null)
            {
                return null;
            }
            #region ### Satışlar, Satış Miktarı, Maliyet, Kar
            model.Sales = (result.FirstOrDefault(a => a.DisplayId == "S04-E6-01-001-01")?.Sales ?? 0).ToString("n2");
            model.SalesQuantity = (result.FirstOrDefault(a => a.DisplayId == "S04-E6-01-001-04")?.Sales ?? 0).ToString("n2");
            model.Cost = (result.FirstOrDefault(a => a.DisplayId == "S04-E6-02-001-07")?.Sales ?? 0).ToString("n2");
            model.Profit = (result.FirstOrDefault(a => a.DisplayId == "S04-E6-02-001-10")?.Sales ?? 0).ToString("n2");
            #endregion
            #region ### STOK GRUPLARI
            var stokGruplari = result.Where(a => a.DisplayId == "S04-E6-01-001-16").ToList();
            var displayNames = stokGruplari.Select(a => a.Header).Distinct().ToList();
            var stokGroupList = new List<StockGroup>();
            foreach (var item in displayNames)
            {
                var sgData = stokGruplari.FirstOrDefault(a => a.Header == item);
                if (sgData != null)
                {
                    var sg = new StockGroup();
                    sg.GroupName = item;
                    sg.Sales = (sgData?.Sales ?? 0.0).ToString("n2");
                    sg.SalesQuantity = (sgData?.Amount ?? 0).ToString("n2");
                    sg.Cost = (sgData?.Cost ?? 0).ToString("n2");
                    sg.Profit = (sgData?.Profit ?? 0).ToString("n2");
                    sg.RemainingAmount = (sgData?.Symbol ?? 0).ToString("n2");
                    stokGroupList.Add(sg);
                }

            }
            model.StockGroup = stokGroupList;

            #region  ### aylik chart
            var aylikGroupDataChart = result.Where(a => a.DisplayId == "S04-E6-02-001-18").ToList();
            var displayNameChartData = aylikGroupDataChart.Select(a => a.Header).Distinct().ToList();
            var groubChartList = new List<GroupDataChartData>();
            foreach (var item in displayNameChartData)
            {
                var dataChart = new GroupDataChartData();
                var itemChartData = aylikGroupDataChart.Where(a => a.Header == item).ToList();

                dataChart.name = itemChartData.FirstOrDefault()?.Header ?? "";
                dataChart.data = itemChartData.GetDataByMonthArray();
                groubChartList.Add(dataChart);
            }
            model.StockGroupChartData = groubChartList;

            #endregion

            #endregion
            #region ### Ürün Satış,Ürün Maliyet,DEPO-ŞUBE SATIŞLARI charts
            //Ürün Satış
            var urunSatisData = result.Where(a => a.DisplayId == "S04-E6-03-001-19").ToList();
            var urunSatisDisplayNameChartData = aylikGroupDataChart.Select(a => a.Header).Distinct().OrderBy(b => b).ToList();
            var nameList = new List<string>();
            var dataList = new List<double>();
            var urunSatisChartData = new SeriesChartData();
            foreach (var item in urunSatisDisplayNameChartData)
            {
                var d = urunSatisData.FirstOrDefault(a => a.Header == item);
                if (d != null)
                {
                    nameList.Add(item);
                    dataList.Add((d?.Sales ?? 0.0));

                }

            }
            urunSatisChartData.Data = dataList.ToArray();
            urunSatisChartData.Name = nameList.ToArray();
            model.ProductSalesChartData = urunSatisChartData;
            //Ürün Maliyet S04-E6-03-001-20
            var urunMaliyetData = result.Where(a => a.DisplayId == "S04-E6-03-001-20").ToList();
            var urunMaliyetDisplayNameChartData = aylikGroupDataChart.Select(a => a.Header).Distinct().OrderBy(b => b).ToList();
            var nameMaliyetList = new List<string>();
            var dataMaliyetList = new List<double>();
            var urunMaliyetChartData = new SeriesChartData();
            foreach (var item in urunMaliyetDisplayNameChartData)
            {
                var d = urunMaliyetData.FirstOrDefault(a => a.Header == item);
                if (d != null)
                {
                    nameMaliyetList.Add(item);
                    dataMaliyetList.Add((d?.Sales ?? 0.0));

                }

            }
            urunMaliyetChartData.Data = dataMaliyetList.ToArray();
            urunMaliyetChartData.Name = nameMaliyetList.ToArray();
            model.ProductCostChartData = urunMaliyetChartData;
            //DEPO-ŞUBE SATIŞLARI
            var depoSatisData = result.Where(a => a.DisplayId == "S04-E6-04-001-21").ToList();
            var depoSatisYuzde = result.Where(a => a.DisplayId == "S04-E6-04-001-22").ToList();

            var depoSatisDisplayNameChartData = depoSatisData.Select(a => a.Header).Distinct().OrderBy(b => b).ToList();

            var depoSatisChartData = new List<WarehouseBranchSalesChartData>();
            foreach (var item in depoSatisDisplayNameChartData)
            {
                var sales = depoSatisData.FirstOrDefault(a => a.Header == item);
                var percentageSales = depoSatisYuzde.FirstOrDefault(a => a.Header == item);
                if (sales != null && percentageSales != null)
                {
                    var d = new WarehouseBranchSalesChartData();

                    d.Name = sales.Header;
                    d.Sales = sales.Sales;
                    d.Percentage = percentageSales.Symbol;
                    depoSatisChartData.Add(d);
                }
            }
            model.WarehouseBranchSalesChartData = depoSatisChartData;

            #endregion
            #region  ### Hizmet Satışları, Diğer Gelirler, Faaliyet Giderleri, Net Kar
            //Hizmet Satışları
            model.ServiceSales = (result.FirstOrDefault(a => a.DisplayId == "S04-E6-04-001-24")?.Sales ?? 0).ToString("n2");
            //Diğer Gelirler
            model.OtherProfits = (result.FirstOrDefault(a => a.DisplayId == "S04-E6-04-001-27")?.Sales ?? 0).ToString("n2");
            //Faaliyet Giderleri
            model.FinancialExpenses = (result.FirstOrDefault(a => a.DisplayId == "S04-E6-04-001-30")?.Sales ?? 0).ToString("n2");
            //Net Kar
            model.NetProfitPrice = (result.FirstOrDefault(a => a.DisplayId == "S04-E6-04-001-33")?.Sales ?? 0).ToString("n2");
            model.NetProfitPercent = (result.FirstOrDefault(a => a.DisplayId == "S04-E6-04-001-36")?.Sales ?? 0.0);

            #endregion
            #region #### Hizmet Satışları Grafiği, Gelir-Gider-Kar Grafiği, Toplam Satışlar Dağılımı Stok-Hizmet
            #region  ### Hizmet Satışları Grafiği
            var aylikHizmetSatisDataChart = result.Where(a => a.DisplayId == "S04-E6-01-001-37").ToList();
            var displayNameAylikHizmetSatisChartData = aylikHizmetSatisDataChart.Select(a => a.Header).Distinct().ToList();
            var groubAylikHizmetSatisChartList = new List<GroupDataChartData>();
            foreach (var item in displayNameAylikHizmetSatisChartData)
            {
                var dataChart = new GroupDataChartData();
                var itemChartData = aylikHizmetSatisDataChart.Where(a => a.Header == item).ToList();

                dataChart.name = itemChartData.FirstOrDefault()?.Header ?? "";
                dataChart.data = itemChartData.GetDataByMonthArray();
                groubAylikHizmetSatisChartList.Add(dataChart);
            }
            model.ServiceSalesMonthChartData = groubAylikHizmetSatisChartList;

            #endregion
            #region ###    Gelir-Gider-Kar Grafiği
            var gelirGiderKarChartData = result.Where(a => a.DisplayId == "S04-E6-06-001-47").ToList();
            if (gelirGiderKarChartData != null)
            {
                var gelirGiderKarChart = new SeriesChartData();
                var gelirGiderKarTitles = gelirGiderKarChartData.Select(a => a.Header).Distinct().ToList();
                var gelirGiderTitleList = new List<string>();
                var gelirGiderValueList = new List<double>();
                foreach (var item in gelirGiderKarTitles)
                {
                    var chartData = gelirGiderKarChartData.Where(a => a.Header == item).FirstOrDefault();
                    if (chartData != null)
                    {
                        gelirGiderTitleList.Add(item);
                        gelirGiderValueList.Add(Math.Round(chartData.Sales, 2));
                    }

                }

                gelirGiderKarChart.Name = gelirGiderTitleList.ToArray();
                gelirGiderKarChart.Data = gelirGiderValueList.ToArray();


                model.GelirGiderKarChartData = gelirGiderKarChart;
            }
            else
            {
                model.GelirGiderKarChartData = new SeriesChartData();
            }

            #endregion
            #region ###    Toplam Satışlar Dağılımı Stok-Hizmet
            var toplamSatislarDagilimiChartData = result.Where(a => a.DisplayId == "S04-E6-06-001-51").ToList();
            if (toplamSatislarDagilimiChartData != null)
            {
                var toplamSatislarDagilimiChart = new SeriesChartData();
                var toplamSatislarDagilimiTitles = toplamSatislarDagilimiChartData.Select(a => a.Header).Distinct().ToList();
                var toplamSatislarDagilimiTitleList = new List<string>();
                var toplamSatislarDagilimiValueList = new List<double>();
                foreach (var item in toplamSatislarDagilimiTitles)
                {
                    var chartData = toplamSatislarDagilimiChartData.Where(a => a.Header == item).FirstOrDefault();
                    if (chartData != null)
                    {
                        toplamSatislarDagilimiTitleList.Add(item);
                        toplamSatislarDagilimiValueList.Add(Math.Round(chartData.Sales, 2));
                    }

                }

                toplamSatislarDagilimiChart.Name = toplamSatislarDagilimiTitleList.ToArray();
                toplamSatislarDagilimiChart.Data = toplamSatislarDagilimiValueList.ToArray();


                model.ToplamSatislarDagilimiChartData = toplamSatislarDagilimiChart;
            }
            else
            {
                model.ToplamSatislarDagilimiChartData = new SeriesChartData();
            }

            #endregion

            #endregion

            return model;
        }

        public async Task<IActionResult> SalesPurchaseSummary()
        {
            var model = new SalesPurchaseSummaryModel();
            var cariDate = DateTime.Now;
            model.CariDonemDate = $"{cariDate.Month}/{cariDate.Year}";

            var result = await SalesPurchaseSummaryData(model);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, Messages.DontFoundData);
                return View(model);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SalesPurchaseSummary(SalesPurchaseSummaryModel model)
        {

            var result = await SalesPurchaseSummaryData(model);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, Messages.DontFoundData);
                return View(model);
            }

            return View(model);

        }

        public async Task<SalesPurchaseSummaryModel?> SalesPurchaseSummaryData(SalesPurchaseSummaryModel model)
        {
            var cariDate = GetDateFromText("/", model.CariDonemDate);

            var monthCariParam = model.ReportTypeId == 1 ? 0 : cariDate.Month;
            var result = await _repoReportService.GetReportDisplayFiveAsync(cariDate.Year, monthCariParam);

            if (result == null)
            {
                return null;
            }

            #region  ### Toplam Satışlar, Satış Vergi Tutarı, Müşteri Sayısı, Ortalama Vade 
            //Toplam Satışlar
            model.TotalSalesPrice = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-01-001-04")?.Price ?? 0).ToString("n2");
            model.TotalSalesPercentage = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-01-001-05")?.Symbol ?? 0.0);
            //Satış Vergi Tutarı
            model.SalesTaxPrice = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-01-001-09")?.Price ?? 0).ToString("n2");
            model.SalesTaxPercentage = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-01-001-10")?.Symbol ?? 0.0);
            //Müşteri Sayısı
            model.CustomerCount = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-02-001-14")?.Price ?? 0).ToString();
            model.CustomerCountPercentage = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-02-001-15")?.Symbol ?? 0.0);
            //Ortalama Vade 
            model.AverageMaturity = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-02-001-19")?.Price ?? 0).ToString("n2");

            #endregion
            #region  ###  Satış Tutar Müşteri Sayısı GrafiğiGrafiği
            var satisTutariTedarikDataChart = result.Where(a => a.DisplayId == "S05-E6-01-001-20").ToList();
            var satisTutariTedarikDisplayNameChartData = satisTutariTedarikDataChart.Select(a => a.Header).Distinct().ToList();
            var satisTutariTedariChartList = new List<ChartDataNameDataArray>();
            foreach (var item in satisTutariTedarikDisplayNameChartData)
            {
                var dataChart = new ChartDataNameDataArray();
                var itemChartData = satisTutariTedarikDataChart.Where(a => a.Header == item).ToList();

                dataChart.name = itemChartData.FirstOrDefault()?.Header ?? "";
                dataChart.data = itemChartData.GetDataByMonthArray();
                satisTutariTedariChartList.Add(dataChart);
            }
            model.SatisTutariTedarikciChartData = satisTutariTedariChartList;

            #endregion
            #region ### En Çok Satış Yapılan Müşteriler ###
            var mostCustomerPriceData = new List<double>();
            var mostCustomerName = new List<string>();
            var mostCustomerToplamData = result.Where(a => a.DisplayId == "S05-E6-02-001-24").ToList();
            foreach (var item in mostCustomerToplamData)
            {
                mostCustomerPriceData.Add(Math.Round(item.Price, 2));
                mostCustomerName.Add(item.Header);
            }
            var mostCustomerData = new DoubleArray();
            mostCustomerData.Data = mostCustomerPriceData.ToArray();
            mostCustomerData.Name = mostCustomerName.ToArray();

            model.MostCustomerSalesChartData = mostCustomerData;

            #endregion
            #region ###   Vadeli - Peşin Satış
            var vadeliPesinSatislarChartData = result.Where(a => a.DisplayId == "S05-E6-02-001-28").ToList();
            if (vadeliPesinSatislarChartData != null && vadeliPesinSatislarChartData.Count > 0)
            {
                var vadeliPesinSatislarChart = new SeriesChartData();
                var vadeliPesinSatislarTitles = vadeliPesinSatislarChartData.Select(a => a.Header).Distinct().ToList();
                var vadeliPesinSatislarTitleList = new List<string>();
                var vadeliPesinSatislarValueList = new List<double>();
                foreach (var item in vadeliPesinSatislarTitles)
                {
                    var chartData = vadeliPesinSatislarChartData.Where(a => a.Header == item).FirstOrDefault();
                    if (chartData != null)
                    {
                        vadeliPesinSatislarTitleList.Add(item);
                        vadeliPesinSatislarValueList.Add(Math.Round(chartData.Price, 2));
                    }

                }

                vadeliPesinSatislarChart.Name = vadeliPesinSatislarTitleList.ToArray();
                vadeliPesinSatislarChart.Data = vadeliPesinSatislarValueList.ToArray();


                model.VadeliPesinSatisChartData = vadeliPesinSatislarChart;
            }
            else
            {
                model.VadeliPesinSatisChartData = new SeriesChartData();
            }

            #endregion

            #region  ### Toplam Alışlar, Alış Vergi Tutarı, Tedarikçi Sayısı, Ortalama Vade 
            //Toplam Satışlar
            model.TotalBuyPrice = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-03-002-04")?.Price ?? 0).ToString("n2");
            model.TotalBuyPercentage = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-03-002-05")?.Symbol ?? 0.0);
            //Satış Vergi Tutarı
            model.BuyTaxPrice = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-03-002-09")?.Price ?? 0).ToString("n2");
            model.BuyTaxPercentage = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-03-002-10")?.Symbol ?? 0.0);
            //Müşteri Sayısı
            model.SuppliersCount = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-04-002-14")?.Price ?? 0).ToString();
            model.SuppliersCountPercentage = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-04-002-15")?.Symbol ?? 0.0);
            //Ortalama Vade 
            model.BuyAverageMaturity = (result.FirstOrDefault(a => a.DisplayId == "S05-E6-04-002-19")?.Price ?? 0).ToString("n2");

            #endregion
            #region  ###  Alış Tutarı Tedarikçi Sayısı Grafiği, En Çok Alış Yapılan Tedarikçi, Vadeli-Peşin Alış
            var alisTutariTedarikDataChart = result.Where(a => a.DisplayId == "S05-E6-05-002-20").ToList();
            var alisTutariTedarikDisplayNameChartData = alisTutariTedarikDataChart.Select(a => a.Header).Distinct().ToList();
            var alisTutariTedariChartList = new List<ChartDataNameDataArray>();
            foreach (var item in alisTutariTedarikDisplayNameChartData)
            {
                var dataChart = new ChartDataNameDataArray();
                var itemChartData = alisTutariTedarikDataChart.Where(a => a.Header == item).ToList();

                dataChart.name = itemChartData.FirstOrDefault()?.Header ?? "";
                dataChart.data = itemChartData.GetDataByMonthArray();
                alisTutariTedariChartList.Add(dataChart);
            }
            model.AlisTutariTedarikciChartData = alisTutariTedariChartList;

            #endregion
            ///####
            #region ### En Çok Alış Yapılan Tedarikçi ###
            var mostSupplierPriceData = new List<double>();
            var mostSupplierName = new List<string>();
            var mostSupplierToplamData = result.Where(a => a.DisplayId == "S05-E6-06-002-24").ToList();
            foreach (var item in mostSupplierToplamData)
            {
                mostSupplierPriceData.Add(Math.Round(item.Price, 2));
                mostSupplierName.Add(item.Header);
            }
            var mostSupplierData = new DoubleArray();
            mostSupplierData.Data = mostSupplierPriceData.ToArray();
            mostSupplierData.Name = mostSupplierName.ToArray();

            model.MostSupplierSalesChartData = mostSupplierData;

            #endregion
            #region ###  Vadeli-Peşin Alış
            var vadeliPesinAlislarChartData = result.Where(a => a.DisplayId == "S05-E6-06-002-28").ToList();
            if (vadeliPesinAlislarChartData != null && vadeliPesinAlislarChartData.Count > 0)
            {
                var vadeliPesinAlislarChart = new SeriesChartData();
                var vadeliPesinAlislarTitles = vadeliPesinAlislarChartData.Select(a => a.Header).Distinct().ToList();
                var vadeliPesinAlislarTitleList = new List<string>();
                var vadeliPesinAlislarValueList = new List<double>();
                foreach (var item in vadeliPesinAlislarTitles)
                {
                    var chartData = vadeliPesinAlislarChartData.Where(a => a.Header == item).FirstOrDefault();
                    if (chartData != null)
                    {
                        vadeliPesinAlislarTitleList.Add(item);
                        vadeliPesinAlislarValueList.Add(Math.Round(chartData.Price, 2));
                    }

                }

                vadeliPesinAlislarChart.Name = vadeliPesinAlislarTitleList.ToArray();
                vadeliPesinAlislarChart.Data = vadeliPesinAlislarValueList.ToArray();


                model.VadeliPesinAlisChartData = vadeliPesinAlislarChart;
            }
            else
            {
                model.VadeliPesinAlisChartData = new SeriesChartData();
            }

            #endregion
            #region  ###  Satış Alış Karşılaştırma Grafiği
            var satisAlisKarsilastirmaTedarikDataChart = result.Where(a => a.DisplayId == "S05-E6-05-003-01").ToList();
            var satisAlisKarsilastirmaTedarikDisplayNameChartData = satisAlisKarsilastirmaTedarikDataChart.Select(a => a.Header).Distinct().ToList();
            var satisAlisKarsilastirmaTedariChartList = new List<ChartDataNameDataArray>();
            foreach (var item in satisAlisKarsilastirmaTedarikDisplayNameChartData)
            {
                var dataChart = new ChartDataNameDataArray();
                var itemChartData = satisAlisKarsilastirmaTedarikDataChart.Where(a => a.Header == item).ToList();

                dataChart.name = itemChartData.FirstOrDefault()?.Header ?? "";
                dataChart.data = itemChartData.GetDataByMonthArray();
                satisAlisKarsilastirmaTedariChartList.Add(dataChart);
            }
            model.SellBuyCompare = satisAlisKarsilastirmaTedariChartList;

            #endregion
            #region  ###  Satış -Alış Vergi Karşılaştırma Grafiği
            var satisAlisVergiKarsilastirmaTedarikDataChart = result.Where(a => a.DisplayId == "S05-E6-06-003-02").ToList();
            var satisAlisVergiKarsilastirmaTedarikDisplayNameChartData = satisAlisVergiKarsilastirmaTedarikDataChart.Select(a => a.Header).Distinct().ToList();
            var satisAlisVergiKarsilastirmaTedariChartList = new List<ChartDataNameDataArray>();
            foreach (var item in satisAlisVergiKarsilastirmaTedarikDisplayNameChartData)
            {
                var dataChart = new ChartDataNameDataArray();
                var itemChartData = satisAlisVergiKarsilastirmaTedarikDataChart.Where(a => a.Header == item).ToList();

                dataChart.name = itemChartData.FirstOrDefault()?.Header ?? "";
                dataChart.data = itemChartData.GetDataByMonthArray();
                satisAlisVergiKarsilastirmaTedariChartList.Add(dataChart);
            }
            model.SellBuyTaxCompare = satisAlisVergiKarsilastirmaTedariChartList;

            #endregion


            return model;
        }

        public async Task<IActionResult> GeneralSum2()
        {
            var result = await _repoReportService.GetReportFP_01_01Async();
            var reportInfoList = new GeneralSumModel();
            if (result != null && result.Any())
            {
                reportInfoList.Assets = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-01-001")?.Price ?? 0).ToString("n2");
                reportInfoList.Stocks = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-01-002")?.Price ?? 0).ToString("n2");
                reportInfoList.WillReceive = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-01-003")?.Price ?? 0).ToString("n2");
                reportInfoList.Debt = (result.FirstOrDefault(a => a.DisplayId == "S01-E5-01-004")?.Price ?? 0).ToString();
                #region ### S01-E5-02-005 Kasa-Banka-Çek-Senet-Pos ###
                var kasaBankaCekSenetPosData = new List<SeriesData>();
                var walletData = result.Where(a => a.DisplayId == "S01-E5-02-005").ToList();
                var displayNames = walletData.Select(a => a.DisplayTitle).Distinct().ToList();

                foreach (var displayName in displayNames)
                {
                    var chartData = new SeriesData();
                    chartData.name = displayName;
                    var itemData = new double[12];

                    var items = walletData.Where(a => a.DisplayTitle == displayName).ToList();
                    for (int i = 1; i < 13; i++)
                    {
                        var item = items.FirstOrDefault(a => a.ReportMonth == i);
                        if (item != null)
                        {
                            itemData[i - 1] = item.Price;
                        }
                        else
                        {
                            itemData[i - 1] = 0;
                        }

                    }
                    chartData.data = itemData;
                    kasaBankaCekSenetPosData.Add(chartData);
                }
                reportInfoList.KasaBankaCekSenetPos = kasaBankaCekSenetPosData;
                #endregion

            }
            else
            {
                ModelState.AddModelError(string.Empty, Messages.DontFoundData);
            }


            return View(reportInfoList);
        }


        #region ### Helper ###
        public DateTime GetDateFromText(string sparator, string text)
        {
            var returnDate = DateTime.MinValue;
            var splitedText = text.Split(sparator);
            if (splitedText.Count() < 2)
            {

                return returnDate;
            }

            var year = int.Parse(splitedText[1]);
            var month = int.Parse(splitedText[0]);
            return new DateTime(year, month, 1);
        }
        public int GenerateRandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        #endregion

    }
}
