using System;
using Match.Entities.Models.MachDbModel;
using Match.Entities.Models.ReportDbModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Match.DataAccess.Concrete.EntityFreamwork.Contexts
{
    public class ReportDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = configuration["ReportDbConnectionString"];

            optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Keyless entity type tanımlama
            modelBuilder.Entity<FP_GeneralSummary_01>().HasNoKey();
            modelBuilder.Entity<FinancialStatement_02>().HasNoKey();
            modelBuilder.Entity<FP_GeneralSummary_03>().HasNoKey();
            modelBuilder.Entity<FP_GeneralSummary_04>().HasNoKey();
            modelBuilder.Entity<FP_GeneralSummary_05>().HasNoKey();
        }
        public DbSet<FP_GeneralSummary_01> DisplayOneReportSp { get; set; }
        public DbSet<FinancialStatement_02> DisplayTwoReportSp { get; set; }
        public DbSet<FP_GeneralSummary_03> DisplayThreeReportSp { get; set; }
        public DbSet<FP_GeneralSummary_04> DisplayFourReportSp { get; set; }
        public DbSet<FP_GeneralSummary_05> DisplayFiveReportSp { get; set; }

    }

}

