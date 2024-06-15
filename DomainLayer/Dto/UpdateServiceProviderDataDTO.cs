using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class UpdateServiceProviderDataDTO
    {
        
        public string? Name { get; set; }
        public string ?Number { get; set; }
        public string? EMail { get; set; }
        public string ?Location { get; set; }
        public List<string>? Spezilization { get; set; }
        public string ?CarTypeToRepaire { get; set; }
       
    }
}
