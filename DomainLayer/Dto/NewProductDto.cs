using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class NewProductDto
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> PictureURL { get; set; }
        public double? Price { get; set; }
        public double ?NewPrice { get; set; }
        public string Description { get; set; }
        public bool InStock { get; set; }
        public string SellerId { get; set; }
        public string CategoryName { get; set; }
        public SellerDto Seller { get; set; }
        public Category Category { get; set; }
        public Discount Discount { get; set; }
       
    }
}
