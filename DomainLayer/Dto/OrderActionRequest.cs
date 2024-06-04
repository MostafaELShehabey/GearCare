using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Dto
{
    public class OrderActionRequest
    {
        public string OrderId { get; set; }
        public OrderAction Action { get; set; }
    }
}
