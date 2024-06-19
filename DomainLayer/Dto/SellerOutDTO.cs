using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public  class SellerOutDTO
    {
        public string Name { get; set; }
    
        public string Number { get; set; }
        public string? EMail { get; set; }
     
        public string Location { get; set; }
        public string? Photo { get; set; }//photo

        public string? PhotoId { get; set; }

        public List<string> Spezilization { get; set; }
    
        public UserType UserType { get; set; }
    }
}
