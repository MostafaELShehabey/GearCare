using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class WinchDriver
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
      
        public string DriveringLicence { get; set; }
      
        [ForeignKey("Winch")]
        public string winchId { get; set; }

        public Winch Winch { get; set; }

        [ForeignKey("DriverId")]
        public string DriverId { get; set; }
        public ApplicationUser Driver { get; set; }
        // Navigation Properties


        // Navigation Properties
        public ICollection<WinchOrder> Orders { get; set; }
    }
}
