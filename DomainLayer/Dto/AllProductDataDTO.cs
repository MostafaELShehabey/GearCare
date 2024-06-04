using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class AllProductDataDTO
    {
       
            public string Name { get; set; }
            public List<string> PictureURL { get; set; }
            [RegularExpression(@"\d+(\.\d{1,2})?")] // make number like 15.00 
            public double OriginalPrice { get; set; }
            public string Description { get; set; }
            public string SellerId { get; set; }
            public string? CategoryId { get; set; }
            public DiscountDto? Discount { get; set; }
       
    }
}
