using System;
using System.Collections.Generic;

namespace Explorify.Web.Models
{
    public partial class JobSkill
    {
        public System.Guid JobSkillId { get; set; }
        public System.Guid SkillId { get; set; }
        public System.Guid JobId { get; set; }
        public virtual Job Job { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
