using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Explorify.Web.Models
{
    public class UserDetailModel
    {
        //public string UserId { get; set; }
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Year of Experience")]
        public int YearExperience { get; set; }

        [Display(Name = "Month of Experience")]
        public int MonthExperience { get; set; }

        [Display(Name = "Categories")]
        public string Category { get; set; }

        [Display(Name = "Skills")]
        public string Skill { get; set; }
    }
}