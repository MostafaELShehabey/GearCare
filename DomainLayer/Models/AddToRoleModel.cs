﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class AddToRoleModel
    {
        [Required] 
        public string UserId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
