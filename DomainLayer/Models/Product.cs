using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Product
    {
        //PrimaryKey
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> ?PictureURL { get; set; }
        public double ?price { get; set; }
        public string? Description { get; set; }
        public bool instock { get; set; }
        public bool deleted { get; set; } 
        public string SellerId { get; set; }
        public string? Categoryid { get; set; }

        public ApplicationUser Seller { get; set; }        
        public Discount ?Discount { get; set; }
        public Category ?Categorys { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Product_Shoppingcart> product_Shoppingcart { get; set; } 
    }
}
