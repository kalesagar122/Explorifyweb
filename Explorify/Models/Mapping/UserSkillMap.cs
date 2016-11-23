using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Explorify.Web.Models.Mapping
{
    public class UserSkillMap : EntityTypeConfiguration<UserSkill>
    {
        public UserSkillMap()
        {
            // Primary Key
            this.HasKey(t => t.UserSkillId);

            // Properties
            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("UserSkill");
            this.Property(t => t.UserSkillId).HasColumnName("UserSkillId");
            this.Property(t => t.SkillId).HasColumnName("SkillId");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            //this.HasRequired(t => t.AspNetUser)
            //    .WithMany(t => t.UserSkills)
            //    .HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.Skill)
                .WithMany(t => t.UserSkills)
                .HasForeignKey(d => d.SkillId);

        }
    }
}
