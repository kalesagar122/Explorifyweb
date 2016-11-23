using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explorify.Models
{
    public class JobDetailModel
    {
        public Guid Id { get; set; }
        public string PostedDate { get; set; }
        public string ExpireDate { get; set; }
        public string JobTitle { get; set; }
        public string CompanyJobId { get; set; }
        public int YearExpereince { get; set; }
        public int MonthExperience { get; set; }
        public string JobDetails { get; set; }
        public string CompanyName { get; set; }
        public string Website { get; set; }
        //public bool IsNotification { get; set; }
        public string CompanyAddress { get; set; }
        public string Lat { get; set; }
        public string Lan { get; set; }
        public string Radius { get; set; }
        public string Skills { get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }
    }
}