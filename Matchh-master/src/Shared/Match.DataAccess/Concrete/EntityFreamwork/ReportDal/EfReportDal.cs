using Match.DataAccess.Abstract;
using Match.DataAccess.Concrete.EntityFreamwork.Contexts;
using Match.Entities.Models.ReportDbModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Match.DataAccess.Concrete.EntityFreamwork.ReportDal
{
    public class EfReportDal : IReportDal
    {

        public async Task<List<FP_GeneralSummary_01>> GetReport_FP_01_01_Async(int year)
        {
            var yearParam = new SqlParameter("@Year", year);

            using var context = new ReportDbContext();
            return await context.DisplayOneReportSp
               .FromSqlRaw("EXEC dbo.Match_EKR_01_Ekran1_Sp @Year", yearParam)
               .ToListAsync();
        }
        public async Task<List<FinancialStatement_02>> GetReport_Match_Ekran2_Sp_Async(int year, int month)
        {
            var yearParam = new SqlParameter("@Year", year);
            var monthParam = new SqlParameter("@Month", month);

            using var context = new ReportDbContext();
            return await context.DisplayTwoReportSp
               .FromSqlRaw("EXEC dbo.Match_EKR_02_Ekran2_Sp @Year, @Month", yearParam, monthParam)
               .ToListAsync();
        }

        public async Task<List<FP_GeneralSummary_03>> GetReport_Match_Ekran3_Sp_Async(int year, int month)
        {
            var yearParam = new SqlParameter("@Year", year);
            var monthParam = new SqlParameter("@Month", month);

            using var context = new ReportDbContext();
            return await context.DisplayThreeReportSp
               .FromSqlRaw("EXEC dbo.Match_EKR_03_Ekran3_Sp @Year, @Month", yearParam, monthParam)
               .ToListAsync();
        }
        public async Task<List<FP_GeneralSummary_04>> GetReport_Match_Ekran4_Sp_Async(int year, int month)
        {
            var yearParam = new SqlParameter("@Year", year);
            var monthParam = new SqlParameter("@Month", month);

            using var context = new ReportDbContext();
            return await context.DisplayFourReportSp
               .FromSqlRaw("EXEC dbo.Match_EKR_04_Ekran4_Sp @Year, @Month", yearParam, monthParam)
               .ToListAsync();
        }
        public async Task<List<FP_GeneralSummary_05>> GetReport_Match_Ekran5_Sp_Async(int year, int month)
        {
            var yearParam = new SqlParameter("@Year", year);
            var monthParam = new SqlParameter("@Month", month);

            using var context = new ReportDbContext();
            return await context.DisplayFiveReportSp
               .FromSqlRaw("EXEC dbo.Match_EKR_05_Ekran5_Sp @Year, @Month", yearParam, monthParam)
               .ToListAsync();
        }
    }
}
