using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class WinchOutputDTO
    {
       
        public List<string> photo { get; set; }//image
        public string Model { get; set; }
        public List<string> Spezilization { get; set; }
        public bool Availabile { get; set; }
    }
}
