using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class WinchModel
    {
        public string Model { get; set; }
        public List<string> Spezilization { get; set; }
    }
}
