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
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(11)]
        public int PhoneNumber { get; set; }
        public string DriverLicence { get; set; }//photo 
        
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
