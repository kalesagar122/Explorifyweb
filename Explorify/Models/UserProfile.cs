using System;
using System.Collections.Generic;

namespace Explorify.Web.Models
{
    public partial class UserProfile
    {
        public System.Guid UserProfileId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public int YearExperience { get; set; }
        public int MonthExperience { get; set; }
        //public virtual AspNetUser AspNetUser { get; set; }
    }
}
