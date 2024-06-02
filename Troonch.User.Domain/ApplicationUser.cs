using Microsoft.AspNetCore.Identity;
using System;

namespace Troonch.User.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
