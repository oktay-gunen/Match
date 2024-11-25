using Match.Business.Services;
using Match.DataAccess.Abstract;
using Match.Entities.Models.ReportDbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match.Business.Concrete
{
    public class ReportManager : IReportService
    {
        private readonly IReportDal _reportDal;

        public ReportManager(IReportDal reportDal)
        {
            _reportDal = reportDal;
        }
        public async Task<List<FP_GeneralSummary_01>> GetReportFP_01_01Async()
        {
            var report = await _reportDal.GetReport_FP_01_01_Async(2024);

            return report;
        }
        public async Task<List<FinancialStatement_02>> GetReportDisplayTwoAsync(int year, int month)
        {
            var report = await _reportDal.GetReport_Match_Ekran2_Sp_Async(year, month);

            return report;
        }
        public async Task<List<FP_GeneralSummary_03>> GetReportDisplayThreeAsync(int year, int month)
        {
            var report = await _reportDal.GetReport_Match_Ekran3_Sp_Async(year, month);

            return report;
        }
        public async Task<List<FP_GeneralSummary_04>> GetReportDisplayFourAsync(int year, int month)
        {
            var report = await _reportDal.GetReport_Match_Ekran4_Sp_Async(year, month);

            return report;
        }
        public async Task<List<FP_GeneralSummary_05>> GetReportDisplayFiveAsync(int year, int month)
        {
            var report = await _reportDal.GetReport_Match_Ekran5_Sp_Async(year, month);

            return report;
        }
    }
}
