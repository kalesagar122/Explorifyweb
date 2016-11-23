using System;
using System.Collections.Generic;

namespace Explorify.Web.Models
{
    public partial class UserSkill
    {
        public System.Guid UserSkillId { get; set; }
        public System.Guid SkillId { get; set; }
        public string UserId { get; set; }
        //public virtual AspNetUser AspNetUser { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
