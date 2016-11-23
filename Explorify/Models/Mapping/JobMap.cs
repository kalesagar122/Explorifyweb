using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Explorify.Web.Models.Mapping
{
    public class JobMap : EntityTypeConfiguration<Job>
    {
        public JobMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.JobTitle)
                .IsRequired();

            this.Property(t => t.CompanyJobId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.JobDetails)
                .IsRequired();

            this.Property(t => t.CompanyName)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.Website)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.CompanyAddress)
                .IsRequired();

            this.Property(t => t.Lat)
                .IsRequired();

            this.Property(t => t.Lan)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Job");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PostedDate).HasColumnName("PostedDate");
            this.Property(t => t.ExpireDate).HasColumnName("ExpireDate");
            this.Property(t => t.JobTitle).HasColumnName("JobTitle");
            this.Property(t => t.CompanyJobId).HasColumnName("CompanyJobId");
            this.Property(t => t.YearExpereince).HasColumnName("YearExpereince");
            this.Property(t => t.MonthExperience).HasColumnName("MonthExperience");
            this.Property(t => t.JobDetails).HasColumnName("JobDetails");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.Website).HasColumnName("Website");
            this.Property(t => t.IsNotification).HasColumnName("IsNotification");
            this.Property(t => t.CompanyAddress).HasColumnName("CompanyAddress");
            this.Property(t => t.Lat).HasColumnName("Lat");
            this.Property(t => t.Lan).HasColumnName("Lan");
            this.Property(t => t.Radius).HasColumnName("Radius");
            this.Property(t => t.IsApplicableforWholeCity).HasColumnName("IsApplicableforWholeCity");
        }
    }
}
