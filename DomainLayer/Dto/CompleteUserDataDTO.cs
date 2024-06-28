using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class CompleteUserDataDTO
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public string Email { get; set; }
        public string? PhotoId { get; set; }
        public string Cartype { get; set; }
        public string PhoneNumber { get; set; }
        public UserType UserType { get; set; }
    }
}