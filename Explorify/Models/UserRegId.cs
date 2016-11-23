using System;
using System.Collections.Generic;

namespace Explorify.Web.Models
{
    public partial class UserRegId
    {
        public System.Guid Id { get; set; }
        public string UserId { get; set; }
        public string RegId { get; set; }
        //public virtual AspNetUser AspNetUser { get; set; }
    }
}
