﻿using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
