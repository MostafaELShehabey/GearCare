using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class WinchDriverOUTDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        
        public string Location { get; set; }
      
        public string winchModel { get; set; }
        public List<string> Spezilization { get; set; }
    }
}
