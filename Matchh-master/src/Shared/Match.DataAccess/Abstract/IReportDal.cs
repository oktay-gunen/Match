using Match.Entities.Models.ReportDbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match.DataAccess.Abstract
{
    public interface IReportDal
    {
        Task<List<FP_GeneralSummary_01>> GetReport_FP_01_01_Async(int year);
        Task<List<FinancialStatement_02>> GetReport_Match_Ekran2_Sp_Async(int year, int month);
        Task<List<FP_GeneralSummary_03>> GetReport_Match_Ekran3_Sp_Async(int year, int month);
        Task<List<FP_GeneralSummary_04>> GetReport_Match_Ekran4_Sp_Async(int year, int month);
        Task<List<FP_GeneralSummary_05>> GetReport_Match_Ekran5_Sp_Async(int year, int month);
    }
}
