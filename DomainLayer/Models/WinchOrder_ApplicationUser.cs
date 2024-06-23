using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class WinchOrder_ApplicationUser
    {
        public string Id { get; set; }
        [ForeignKey("OrderId")]
        public string OrderId { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public WinchOrder winchOrder { get; set; }
        public ApplicationUser user { get; set; }
    }
}
