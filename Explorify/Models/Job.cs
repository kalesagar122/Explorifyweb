using System;
using System.Collections.Generic;

namespace Explorify.Web.Models
{
    public partial class Job
    {
        public Job()
        {
            this.JobSkills = new List<JobSkill>();
            this.JobCategories = new List<JobCategory>();
            this.JobImages = new List<JobImage>();
        }

        public System.Guid Id { get; set; }
        public System.DateTime PostedDate { get; set; }
        public System.DateTime ExpireDate { get; set; }
        public string JobTitle { get; set; }
        public string CompanyJobId { get; set; }
        public int YearExpereince { get; set; }
        public int MonthExperience { get; set; }
        public string JobDetails { get; set; }
        public string CompanyName { get; set; }
        public string Website { get; set; }
        public bool IsNotification { get; set; }
        public string CompanyAddress { get; set; }
        public string Lat { get; set; }
        public string Lan { get; set; }
        public string Radius { get; set; }
        public bool IsApplicableforWholeCity { get; set; }
        public virtual ICollection<JobSkill> JobSkills { get; set; }
        public virtual ICollection<JobCategory> JobCategories { get; set; }
        public virtual ICollection<JobImage> JobImages { get; set; }
    }
}
