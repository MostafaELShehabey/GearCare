using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DomainLayer.Models
{
    public class ShoppingCart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int TotalPrice { get; set; }
        //Foreign Keys
        [ForeignKey("Client")]
        public string ClientId { get; set; }
        //Foreign Keys
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        
        // Navigation Properties
        //[JsonIgnore]
        //[IgnoreDataMember]
        public ApplicationUser Client { get; set; }
       
        // Navigation Properties
        //[JsonIgnore]
        //[IgnoreDataMember]
        public ICollection<Product_Shoppingcart> product_Shoppingcart { get; set; }

        //public Product Products { get; set; }
    }
}

