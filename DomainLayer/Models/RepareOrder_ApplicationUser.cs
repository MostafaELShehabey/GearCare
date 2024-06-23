using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class RepareOrder_ApplicationUser
    {
        public string Id {  get; set; }
       // [ForeignKey("UserId")]
        public string userId { get; set; }
        
        
       // [ForeignKey("OrderId")]
        public string orderId { get; set; }

        public ICollection<RepareOrder> repareOrders { get; set; }
        public ICollection<ApplicationUser> applicationUsers { get; set; }
    }
}
