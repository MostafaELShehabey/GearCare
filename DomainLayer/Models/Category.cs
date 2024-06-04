using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Category
    {
        //PrimaryKey
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
       
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }

        //Navigation Properity 
        public ICollection<Product> Products { get; set; }
    }
}
