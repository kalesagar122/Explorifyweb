using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Explorify.Web.Models.Mapping
{
    public class JobSkillMap : EntityTypeConfiguration<JobSkill>
    {
        public JobSkillMap()
        {
            // Primary Key
            this.HasKey(t => t.JobSkillId);

            // Properties
            // Table & Column Mappings
            this.ToTable("JobSkill");
            this.Property(t => t.JobSkillId).HasColumnName("JobSkillId");
            this.Property(t => t.SkillId).HasColumnName("SkillId");
            this.Property(t => t.JobId).HasColumnName("JobId");

            // Relationships
            this.HasRequired(t => t.Job)
                .WithMany(t => t.JobSkills)
                .HasForeignKey(d => d.JobId);
            this.HasRequired(t => t.Skill)
                .WithMany(t => t.JobSkills)
                .HasForeignKey(d => d.SkillId);

        }
    }
}
