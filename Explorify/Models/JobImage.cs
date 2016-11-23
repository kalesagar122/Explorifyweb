using System;
using System.Collections.Generic;

namespace Explorify.Web.Models
{
    public partial class JobImage
    {
        public System.Guid Id { get; set; }
        public System.Guid JobId { get; set; }
        public string ImagePath { get; set; }
        public virtual Job Job { get; set; }
    }
}
