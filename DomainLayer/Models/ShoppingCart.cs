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
       
       // public bool flag { get; set; }=false;
        
        //Foreign Keys
        [ForeignKey("Client")]
        public string ClientId { get; set; }
       
        //Foreign Keys
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        
        // Navigation Properties
        public ApplicationUser Client { get; set; }
       
        // Navigation Properties
        public ICollection<Product_Shoppingcart> product_Shoppingcart { get; set; }

       
    }
}

