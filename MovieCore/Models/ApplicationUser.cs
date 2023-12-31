﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesCore.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(50)]
        public string FiratName { get; set; } = null!;

        [MaxLength(50)]
        public string LastName { get; set; } = null!;

    }
}
