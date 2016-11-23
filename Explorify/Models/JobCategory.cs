using System;
using System.Collections.Generic;

namespace Explorify.Web.Models
{
    public partial class JobCategory
    {
        public System.Guid Id { get; set; }
        public System.Guid JobId { get; set; }
        public Nullable<System.Guid> CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual Job Job { get; set; }
    }
}
