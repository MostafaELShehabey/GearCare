using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class ServiceProvideroutDTO
    {
        public List<string> ?Spezilization { get; set; }
        public List<string> ?CarTypeToRepaire { get; set; }
      
    }
}
