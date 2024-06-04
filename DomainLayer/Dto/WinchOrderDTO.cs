
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class WinchOrderDTO
    {
        public string Id { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
    }
}
