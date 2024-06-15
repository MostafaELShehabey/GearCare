using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Winch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public List<string> ?Licence { get; set; }//image
        public List<string> ?photo { get; set; }//image
        public string Model { get; set; }
        public bool Availabile { get; set; } = true;
        //Foreign Keys
        [ForeignKey("Driver")]
        public string DriverId { get; set; }
        // Navigation Properties
        public WinchDriver WinchDriver { get; set; }
    }
}
