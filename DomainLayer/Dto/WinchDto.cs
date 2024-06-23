using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class WinchDto
    {
        public List<string> photo { get; set; }//image
        public bool Availabile { get; set; } 
        public WinchDriverOUTDto Driver { get; set; }
    }
}
