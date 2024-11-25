using Match.Entities.Models.ReportDbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match.Business.Services
{
    public interface IReportService
    {
        Task<List<FP_GeneralSummary_01>> GetReportFP_01_01Async();
        Task<List<FinancialStatement_02>> GetReportDisplayTwoAsync(int year, int month);
        Task<List<FP_GeneralSummary_03>> GetReportDisplayThreeAsync(int year, int month);
        Task<List<FP_GeneralSummary_04>> GetReportDisplayFourAsync(int year, int month);
        Task<List<FP_GeneralSummary_05>> GetReportDisplayFiveAsync(int year, int month);
    }
}
