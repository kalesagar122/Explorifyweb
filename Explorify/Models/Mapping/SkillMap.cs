using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Explorify.Web.Models.Mapping
{
    public class SkillMap : EntityTypeConfiguration<Skill>
    {
        public SkillMap()
        {
            // Primary Key
            this.HasKey(t => t.SkillId);

            // Properties
            this.Property(t => t.SkillName)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Skill");
            this.Property(t => t.SkillId).HasColumnName("SkillId");
            this.Property(t => t.SkillName).HasColumnName("SkillName");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
        }
    }
}
