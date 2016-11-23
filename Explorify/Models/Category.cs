using System;
using System.Collections.Generic;

namespace Explorify.Web.Models
{
    public partial class Category
    {
        public Category()
        {
            this.JobCategories = new List<JobCategory>();
            this.UserCategories = new List<UserCategory>();
        }

        public System.Guid Id { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<JobCategory> JobCategories { get; set; }
        public virtual ICollection<UserCategory> UserCategories { get; set; }
    }
}
