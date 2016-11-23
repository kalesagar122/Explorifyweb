using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Explorify.Web.Models
{
    public class PostJobViewModel
    {
        public System.Guid Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Posted Date and Time")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public System.DateTime PostedDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Expire Date and Time")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public System.DateTime ExpireDate { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Company Job Id")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string CompanyJobId { get; set; }

        [Required]
        [Display(Name = "Years of Expereince")]
        public int YearExpereince { get; set; }

        [Required]
        [Display(Name = "Months of Experience")]
        public int MonthExperience { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Website")]
        [Url]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Website { get; set; }

        [Required]
        [Display(Name = "Company Address")]
        public string CompanyAddress { get; set; }

        public string Location { get; set; }

        [Required]
        [Display(Name = "Job Details")]
        public string JobDetails { get; set; }

        [Display(Name = "Is notification?")]
        public bool IsNotification { get; set; }

        [Required]
        [Display(Name = "Latitude")]
        public string Lat { get; set; }

        [Required]
        [Display(Name = "Longitude")]
        public string Lan { get; set; }

        [Required]
        [Display(Name = "Radius")]
        public string Radius { get; set; }

        [Display(Name = "Is applicable for whole city?")]
        public bool IsApplicableforWholeCity { get; set; }

        public List<string> SelectedCategory { get; set; }
        public List<string> SelectedSkill { get; set; }

        public MultiSelectList CategoryList { get; set; }
        public MultiSelectList SkillList { get; set; }
        public MultiSelectList YearList { get; set; }
        public MultiSelectList MonthList { get; set; }
    }
}