﻿using Microsoft.AspNetCore.Identity;

namespace Api.Entities
{
    public class AppUser : IdentityUser
    {
        public Order Order { get; set; }
    }
}
