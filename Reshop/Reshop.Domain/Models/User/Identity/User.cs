using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Reshop.Domain.Models.User.Cart;

namespace Reshop.Domain.Models.User.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string Address { get; set; }

        public IList<Order> Orders { get; set; }
    }
}