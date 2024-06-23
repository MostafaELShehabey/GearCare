using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static DomainLayer.Helpers.Enums;

namespace DomainLayer.Models
{
    public class WinchOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
       // public int Price { get; set; }
        [Range(0,5)]
        public int Rate { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
      // [ForeignKey("WinchDriver")]
        public string DriverId { get; set; }
        //[ForeignKey("WinchClient")]
        public string ClientId  { get; set; }

       public ApplicationUser Client { get; set; }
       public ApplicationUser Driver { get; set; }

        //public WinchOrder_ApplicationUser user { get; set; }
        //public WinchOrder_ApplicationUser user { get; set; }
    }
}
