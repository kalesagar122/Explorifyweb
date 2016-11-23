using System;
using System.Collections.Generic;

namespace Explorify.Web.Models
{
    public partial class Skill
    {
        public Skill()
        {
            this.JobSkills = new List<JobSkill>();
            this.UserSkills = new List<UserSkill>();
        }

        public System.Guid SkillId { get; set; }
        public string SkillName { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<JobSkill> JobSkills { get; set; }
        public virtual ICollection<UserSkill> UserSkills { get; set; }
    }
}
