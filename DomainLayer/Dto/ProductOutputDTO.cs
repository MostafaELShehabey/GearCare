﻿using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Dto
{
    public class ProductOutputDTO
    {
            public string id { get; set; }
            public string Name { get; set; }
            public List<string> PictureURL { get; set; }
            [RegularExpression(@"\d+(\.\d{1,2})?")] // make number like 15.00 
            public double ?Price { get; set; }
            public double? newPrice { get; set; } 
            public int Quantity { get; set; }
            public string? Description { get; set; }
            //public string SellerId { get; set; }
            public string CategoryName { get; set; }
            public Category Category { get; set; }
            public SellerOutDTO Seller { get; set; }
            public DiscountDto? Discount { get; set; }
       
    }
}
