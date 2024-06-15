 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class LoginDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
     
    }
}
