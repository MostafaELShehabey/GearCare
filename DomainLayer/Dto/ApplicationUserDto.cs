﻿using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class ApplicationUserDto
    {
        
        public string Name { get; set; }
       
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
       
        public string Location { get; set; }
        
      // public string CarType { get; set; }
        public string? PhotoId { get; set; }=null;
        
        public string IdPicture { get; set; }
       
        public string Username { get; set; }
        
        public string Password { get; set; }
        public UserType UserType { get; set; }

    }
}
