
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class RegisterServiceProviderDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Number { get; set; }
        public string? EMail { get; set; } 
        [Required]
        public string Location{ get; set; }
        public string? Photo { get; set; }//photo
        [Required]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public List<string> Spezilization { get; set; }
        [Required]
        public string CarTypeToRepaire { get; set; }
        [Required]
        public string IdPhoto { get; set; }
        public UserType UserType { get; set; }
    }
}
