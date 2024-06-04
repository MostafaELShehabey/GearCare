using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string photoURL { get; set; }
       
        //[ForeignKey("Userid")]
        //public string userid { get; set; }

        [ForeignKey("productID")]
        public string productID { get; set; }
        //public ApplicationUser user { get; set; }

        public Product Product { get; set; }
    }
}
