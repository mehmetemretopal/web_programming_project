﻿using Microsoft.AspNetCore.Identity;

namespace authenticatedapp.Models
{
    public class Users:IdentityUser
    {
        public string FullName { get; set; }
    }
}
