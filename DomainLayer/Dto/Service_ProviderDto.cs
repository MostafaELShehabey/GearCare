using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class Service_ProviderDto
    {
        public string id {  get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? EMail { get; set; }
        required
        public string Location { get; set; }
        public string? PhotoId { get; set; }//photo
       
        [Range(0, 5)]
        public int Rate { get; set; }
        public List<string> Spezilization { get; set; }
        public string CarTypeToRepaire { get; set; }
        public bool available { get; set; }
        public UserType UserType  { get; set; }
    }
}
