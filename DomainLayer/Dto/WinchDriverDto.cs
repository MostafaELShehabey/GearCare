using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class WinchDriverDto
    {
        public string Name { get; set; }
        public string Number { get; set; }
       // public string? EMail { get; set; }
        public string Location { get; set; }
        //public string? PhotoId { get; set; }
        public string winchModel { get; set; }
        public List<string> Spezilization { get; set; }



    }
}
