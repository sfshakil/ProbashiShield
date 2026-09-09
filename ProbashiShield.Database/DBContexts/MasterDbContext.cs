using Microsoft.EntityFrameworkCore;
using ProbashiShield.Database.DBEntities;
using ProbashiShield.Database.NonDbEntities.Base;
using System;
using System.Linq;
using System.Reflection;

namespace ProbashiShield.Database.DBContexts;

public partial class MasterDbContext : DbContext
{
    public MasterDbContext() { }
    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options) { }

    public virtual DbSet<Agency> Agency { get; set; }
    public virtual DbSet<AgencySyncLog> AgencySyncLog { get; set; }
    public virtual DbSet<AIAnalysisLog> AIAnalysisLog { get; set; }
    public virtual DbSet<Country> Country { get; set; }
    public virtual DbSet<CountryFeeLimit> CountryFeeLimit { get; set; }
    public virtual DbSet<Document> Document { get; set; }
    public virtual DbSet<ErrorLog> ErrorLog { get; set; }
    public virtual DbSet<JobCategory> JobCategory { get; set; }
    public virtual DbSet<OCRResult> OCRResult { get; set; }
    public virtual DbSet<RiskFinding> RiskFinding { get; set; }
    public virtual DbSet<RiskRule> RiskRule { get; set; }
    public virtual DbSet<SalaryReference> SalaryReference { get; set; }
    public virtual DbSet<VerificationRequest> VerificationRequest { get; set; }
    public virtual DbSet<VerificationResult> VerificationResult { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // mark all non entity types in DBContext
        foreach (Type type in AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(s => s.GetTypes())
        .Where(child => typeof(IMsSqlNonEntityBase).IsAssignableFrom(child) && child.IsClass && !child.IsAbstract))
        {
            modelBuilder.Entity(type).HasNoKey().ToView(null);
        }

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Agency>(entity =>
        {
            entity.ToTable("Agencies");
        });
        modelBuilder.Entity<AgencySyncLog>(entity =>
        {
            entity.ToTable("AgencySyncLogs");
        });
        modelBuilder.Entity<AIAnalysisLog>(entity =>
        {
            entity.ToTable("AIAnalysisLogs");
        });
        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Countries");
        });
        modelBuilder.Entity<CountryFeeLimit>(entity =>
        {
            entity.ToTable("CountryFeeLimits");
        });
        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Documents");
        });
        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.ToTable("ErrorLogs");
        });
        modelBuilder.Entity<JobCategory>(entity =>
        {
            entity.ToTable("JobCategories");
        });
        modelBuilder.Entity<OCRResult>(entity =>
        {
            entity.ToTable("OCRResults");
        });
        modelBuilder.Entity<RiskFinding>(entity =>
        {
            entity.ToTable("RiskFindings");
        });
        modelBuilder.Entity<RiskRule>(entity =>
        {
            entity.ToTable("RiskRules");
        });
        modelBuilder.Entity<SalaryReference>(entity =>
        {
            entity.ToTable("SalaryReferences");
        });
        modelBuilder.Entity<VerificationRequest>(entity =>
        {
            entity.ToTable("VerificationRequests");
        });
        modelBuilder.Entity<VerificationResult>(entity =>
        {
            entity.ToTable("VerificationResults");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
