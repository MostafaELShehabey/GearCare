using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class SellerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public string? PhotoId { get; set; }
        public UserType UserType { get; set; }
        public int Rate { get; set; }
        public int NumberOfRates { get; set; }
        public bool Available { get; set; }
        public List<string>? Specialization { get; set; }
        
    }
}
