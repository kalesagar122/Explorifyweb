using System;
using System.Collections.Generic;

namespace Explorify.Web.Models
{
    public partial class UserCategory
    {
        public System.Guid Id { get; set; }
        public string UserId { get; set; }
        public System.Guid CategoryId { get; set; }
        //public virtual AspNetUser AspNetUser { get; set; }
        public virtual Category Category { get; set; }
    }
}
